using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WyrmApp
{
    public static class RobloxApi
    {
        private static HttpClient MakeClient(System.Net.CookieContainer jar)
        {
            var handler = new HttpClientHandler { CookieContainer = jar };
            var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(30);
            return client;
        }

        // ── CSRF ──────────────────────────────────────────────────────────────
        public static async Task<string> GetCsrfTokenAsync(string? rawCookie = null)
        {
            var jar = rawCookie == null
                ? CookieManager.BuildCookieContainer()
                : CookieManager.BuildCookieContainer(rawCookie);

            // Roblox returns 403 with the token in the header — capture it
            using var client = MakeClient(jar);
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, "https://auth.roblox.com/v2/logout");
                req.Content = new StringContent("", Encoding.UTF8, "application/json");
                var resp = await client.SendAsync(req);
                if (resp.Headers.TryGetValues("x-csrf-token", out var vals))
                    return string.Join("", vals);
            }
            catch { }

            // Retry with fresh client
            using var client2 = MakeClient(jar);
            var req2 = new HttpRequestMessage(HttpMethod.Post, "https://auth.roblox.com/v2/logout");
            req2.Content = new StringContent("", Encoding.UTF8, "application/json");
            var resp2 = await client2.SendAsync(req2);
            if (resp2.Headers.TryGetValues("x-csrf-token", out var vals2))
                return string.Join("", vals2);

            throw new Exception("No x-csrf-token header returned.");
        }

        // ── Root Place ID ─────────────────────────────────────────────────────
        public static async Task<string> GetRootPlaceIdAsync(string universeId)
        {
            using var client = MakeClient(CookieManager.BuildCookieContainer());
            var json = await client.GetStringAsync(
                $"https://games.roblox.com/v1/games?universeIds={universeId}");
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("data")[0].GetProperty("rootPlaceId").ToString();
        }

        // ── Create Server ─────────────────────────────────────────────────────
        public static async Task<string> CreateServerAsync(string universeId, string csrfToken, string serverName)
        {
            using var client = MakeClient(CookieManager.BuildCookieContainer());
            var body = JsonSerializer.Serialize(new { name = serverName, expectedPrice = 0 });
            var req = new HttpRequestMessage(HttpMethod.Post,
                $"https://games.roblox.com/v1/games/vip-servers/{universeId}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            req.Headers.Add("X-CSRF-TOKEN", csrfToken);
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        // ── Rename Server ─────────────────────────────────────────────────────
        public static async Task<string> RenameServerAsync(string privateServerId, string newName, string csrfToken)
        {
            using var client = MakeClient(CookieManager.BuildCookieContainer());
            var body = JsonSerializer.Serialize(new { name = newName });
            var req = new HttpRequestMessage(HttpMethod.Patch,
                $"https://games.roblox.com/v1/vip-servers/{privateServerId}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            req.Headers.Add("X-CSRF-TOKEN", csrfToken);
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        // ── Generate Link ─────────────────────────────────────────────────────
        public static async Task<string> GenerateLinkAsync(string privateServerId, string csrfToken, string? rawCookie = null)
        {
            var jar = rawCookie == null
                ? CookieManager.BuildCookieContainer()
                : CookieManager.BuildCookieContainer(CookieManager.StripPrefix(rawCookie.Trim()));
            using var client = MakeClient(jar);
            var body = JsonSerializer.Serialize(new { newJoinCode = true });
            var req = new HttpRequestMessage(HttpMethod.Patch,
                $"https://games.roblox.com/v1/vip-servers/{privateServerId}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            req.Headers.Add("X-CSRF-TOKEN", csrfToken);
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        // ── Get Metadata ──────────────────────────────────────────────────────
        public static async Task<string> GetMetadataAsync(string privateServerId)
        {
            using var client = MakeClient(CookieManager.BuildCookieContainer());
            return await client.GetStringAsync(
                $"https://games.roblox.com/v1/vip-servers/{privateServerId}");
        }

        // ── Get Private Servers ───────────────────────────────────────────────
        public static async Task<string> GetPrivateServersAsync()
        {
            using var client = MakeClient(CookieManager.BuildCookieContainer());
            return await client.GetStringAsync(
                "https://games.roblox.com/v1/private-servers/my-private-servers");
        }

        // ── Update Single User ────────────────────────────────────────────────
        public static async Task<UpdateUserResult> UpdateSingleUserAsync(
            string rawCookie, string universeId, int index, int total,
            Action<string> log)
        {
            var clean = CookieManager.StripPrefix(rawCookie.Trim());
            var jar   = CookieManager.BuildCookieContainer(clean);

            log($"[{index}/{total}] Fetching user info...");
            string userId, username;
            using (var client = MakeClient(jar))
            {
                var json = await client.GetStringAsync("https://users.roblox.com/v1/users/authenticated");
                using var doc = JsonDocument.Parse(json);
                userId   = doc.RootElement.GetProperty("id").ToString();
                username = doc.RootElement.GetProperty("name").GetString() ?? $"user_{userId}";
            }
            log($"[{index}/{total}] Logged in as {username} (id={userId})");

            log($"[{index}/{total}] Fetching CSRF token...");
            var csrf = await GetCsrfTokenAsync(clean);

            log($"[{index}/{total}] Checking for existing private server...");
            var privateServerId = await FindPrivateServer(jar, universeId);

            if (string.IsNullOrEmpty(privateServerId))
            {
                log($"[{index}/{total}] Creating private server...");
                privateServerId = await CreatePrivateServer(jar, csrf, universeId, username);
            }

            if (string.IsNullOrEmpty(privateServerId))
            {
                log($"[{index}/{total}] Retrying server lookup...");
                privateServerId = await FindPrivateServer(jar, universeId);
            }

            if (string.IsNullOrEmpty(privateServerId))
                throw new Exception($"Could not find or create private server for universe {universeId}");

            log($"[{index}/{total}] Generating join link...");
            string joinLink;
            {
                var linkJson = await GenerateLinkAsync(privateServerId, csrf, clean);
                using var doc = JsonDocument.Parse(linkJson);
                var joinCode = doc.RootElement.GetProperty("joinCode").GetString() ?? "";
                joinLink = $"https://www.roblox.com/games/15532962292?privateServerLinkCode={joinCode}";
            }

            return new UpdateUserResult
            {
                UserId   = userId,
                Username = username,
                Cookie   = clean,
                JoinLink = joinLink
            };
        }

        private static async Task<string?> FindPrivateServer(System.Net.CookieContainer jar, string universeId)
        {
            using var client = MakeClient(jar);
            try
            {
                var json = await client.GetStringAsync(
                    "https://games.roblox.com/v1/private-servers/my-private-servers");
                using var doc  = JsonDocument.Parse(json);
                var root = doc.RootElement;

                JsonElement list;
                if (!root.TryGetProperty("privateServerResponses", out list) &&
                    !root.TryGetProperty("data", out list))
                    list = root;

                if (list.ValueKind == JsonValueKind.Array)
                {
                    foreach (var srv in list.EnumerateArray())
                    {
                        var uid = srv.TryGetProperty("universeId", out var u) ? u.ToString() : "";
                        if (uid != universeId) continue;
                        if (srv.TryGetProperty("id",            out var id))  return id.ToString();
                        if (srv.TryGetProperty("privateServerId", out var pid)) return pid.ToString();
                    }
                }
            }
            catch { }
            return null;
        }

        private static async Task<string?> CreatePrivateServer(
            System.Net.CookieContainer jar, string csrf, string universeId, string name)
        {
            using var client = MakeClient(jar);
            int price = 0;
            try
            {
                var priceJson = await client.GetStringAsync(
                    $"https://games.roblox.com/v1/games/vip-servers/{universeId}");
                using var pd = JsonDocument.Parse(priceJson);
                if (pd.RootElement.TryGetProperty("price", out var pv))
                    price = pv.GetInt32();
            }
            catch { }

            var body = JsonSerializer.Serialize(new { name, expectedPrice = price });
            var req  = new HttpRequestMessage(HttpMethod.Post,
                $"https://games.roblox.com/v1/games/vip-servers/{universeId}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            req.Headers.Add("X-CSRF-TOKEN", csrf);

            try
            {
                var resp = await client.SendAsync(req);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("id",              out var id))  return id.ToString();
                if (doc.RootElement.TryGetProperty("vipServerId",     out var vid)) return vid.ToString();
                if (doc.RootElement.TryGetProperty("privateServerId", out var pid)) return pid.ToString();
            }
            catch { }
            return null;
        }

        // ── Write users.json ──────────────────────────────────────────────────
        public static void WriteUsersJson(string accountName, UpdateUserResult result)
        {
            var path = $@"C:\Users\{accountName}\AppData\Roaming\Jaram\users.json";
            var dir  = System.IO.Path.GetDirectoryName(path) ?? string.Empty;

            if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            var existing = System.IO.File.Exists(path)
                ? System.IO.File.ReadAllText(path)
                : "{}";

            using var doc = JsonDocument.Parse(
                string.IsNullOrWhiteSpace(existing) ? "{}" : existing);

            var dict = new System.Collections.Generic.Dictionary<string, object>();
            foreach (var prop in doc.RootElement.EnumerateObject())
                dict[prop.Name] = prop.Value.Clone();

            dict[result.UserId] = new
            {
                username                         = result.Username,
                cookie                           = result.Cookie,
                private_server_link              = result.JoinLink,
                place                            = "",
                server_type                      = "private",
                bad                              = false,
                cap                              = false,
                disabled                         = false,
                description                      = "",
                alternate_launch                 = false,
                skip_reconnect_on_log_disconnect = false,
                discord_user_ids                 = Array.Empty<string>()
            };

            var output = JsonSerializer.Serialize(dict,
                    new JsonSerializerOptions { WriteIndented = true })
                .Replace("\\u0026", "&");

            System.IO.File.WriteAllText(path, output);
        }
    }

    public class UpdateUserResult
    {
        public string UserId   { get; set; } = "";
        public string Username { get; set; } = "";
        public string Cookie   { get; set; } = "";
        public string JoinLink { get; set; } = "";
    }
}
