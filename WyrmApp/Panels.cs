using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WyrmApp
{
    // ═══════════════════════════════════════════════════════════════
    // 1. Get Root Place ID
    // ═══════════════════════════════════════════════════════════════
    public class GetRootPlaceIdPanel : UserControl
    {
        public GetRootPlaceIdPanel()
        {
            BackColor = UiHelper.BgDark;
            var lbl = UiHelper.MakeLabel("Universe ID", 20, 20);
            var input = UiHelper.MakeInput(20, 40);
            var btn = UiHelper.MakeButton("Get Root Place ID", 20, 76);
            var output = UiHelper.MakeOutputBox(20, 124, 600, 320);

            btn.Click += async (s, e) =>
            {
                var uid = input.Text.Trim();
                if (string.IsNullOrEmpty(uid)) { UiHelper.AppendLog(output, "Universe ID required.", UiHelper.Error); return; }
                btn.Enabled = false;
                output.Clear();
                UiHelper.AppendLog(output, $"Fetching root place ID for universe {uid}...", UiHelper.FgMuted);
                try
                {
                    var result = await RobloxApi.GetRootPlaceIdAsync(uid);
                    UiHelper.AppendLog(output, $"Root Place ID: {result}", UiHelper.Success);
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { lbl, input, btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 2. Get CSRF Token
    // ═══════════════════════════════════════════════════════════════
    public class GetCsrfPanel : UserControl
    {
        public GetCsrfPanel()
        {
            BackColor = UiHelper.BgDark;
            var btn = UiHelper.MakeButton("Get CSRF Token", 20, 20);
            var output = UiHelper.MakeOutputBox(20, 68, 600, 360);

            btn.Click += async (s, e) =>
            {
                btn.Enabled = false;
                output.Clear();
                UiHelper.AppendLog(output, "Fetching CSRF token...", UiHelper.FgMuted);
                try
                {
                    var token = await RobloxApi.GetCsrfTokenAsync();
                    UiHelper.AppendLog(output, $"CSRF Token: {token}", UiHelper.Success);
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 3. Create Server
    // ═══════════════════════════════════════════════════════════════
    public class CreateServerPanel : UserControl
    {
        public CreateServerPanel()
        {
            BackColor = UiHelper.BgDark;

            var lblU = UiHelper.MakeLabel("Universe ID", 20, 20);
            var inputU = UiHelper.MakeInput(20, 40);

            var lblC = UiHelper.MakeLabel("CSRF Token", 20, 76);
            var inputC = UiHelper.MakeInput(20, 96);

            var lblN = UiHelper.MakeLabel("Server Name", 20, 132);
            var inputN = UiHelper.MakeInput(20, 152);

            var btn = UiHelper.MakeButton("Create Server", 20, 188);
            var output = UiHelper.MakeOutputBox(20, 236, 680, 240);

            btn.Click += async (s, e) =>
            {
                var uid = inputU.Text.Trim();
                var csrf = inputC.Text.Trim();
                var name = inputN.Text.Trim();
                if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(csrf) || string.IsNullOrEmpty(name))
                { UiHelper.AppendLog(output, "All fields are required.", UiHelper.Error); return; }

                btn.Enabled = false; output.Clear();
                UiHelper.AppendLog(output, "Creating server...", UiHelper.FgMuted);
                try
                {
                    var json = await RobloxApi.CreateServerAsync(uid, csrf, name);
                    UiHelper.AppendJson(output, json);
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { lblU, inputU, lblC, inputC, lblN, inputN, btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 4. Rename Server
    // ═══════════════════════════════════════════════════════════════
    public class RenameServerPanel : UserControl
    {
        public RenameServerPanel()
        {
            BackColor = UiHelper.BgDark;

            var lblS = UiHelper.MakeLabel("Private Server ID", 20, 20);
            var inputS = UiHelper.MakeInput(20, 40);

            var lblN = UiHelper.MakeLabel("New Name", 20, 76);
            var inputN = UiHelper.MakeInput(20, 96);

            var lblC = UiHelper.MakeLabel("CSRF Token", 20, 132);
            var inputC = UiHelper.MakeInput(20, 152);

            var btn = UiHelper.MakeButton("Rename Server", 20, 188);
            var output = UiHelper.MakeOutputBox(20, 236, 680, 240);

            btn.Click += async (s, e) =>
            {
                var sid = inputS.Text.Trim();
                var name = inputN.Text.Trim();
                var csrf = inputC.Text.Trim();
                if (string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(csrf))
                { UiHelper.AppendLog(output, "All fields are required.", UiHelper.Error); return; }

                btn.Enabled = false; output.Clear();
                UiHelper.AppendLog(output, "Renaming server...", UiHelper.FgMuted);
                try
                {
                    var json = await RobloxApi.RenameServerAsync(sid, name, csrf);
                    UiHelper.AppendJson(output, json);
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { lblS, inputS, lblN, inputN, lblC, inputC, btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 5. Generate Join Link
    // ═══════════════════════════════════════════════════════════════
    public class GenerateLinkPanel : UserControl
    {
        public GenerateLinkPanel()
        {
            BackColor = UiHelper.BgDark;

            var lblS = UiHelper.MakeLabel("Private Server ID", 20, 20);
            var inputS = UiHelper.MakeInput(20, 40);

            var lblC = UiHelper.MakeLabel("CSRF Token", 20, 76);
            var inputC = UiHelper.MakeInput(20, 96);

            var lblCk = UiHelper.MakeLabel("Roblox Cookie (.ROBLOSECURITY)", 20, 132);
            var inputCk = UiHelper.MakeInput(20, 152);
            inputCk.UseSystemPasswordChar = true;
            inputCk.PlaceholderText = ".ROBLOSECURITY cookie...";

            var btn = UiHelper.MakeButton("Generate Link", 20, 188);
            var output = UiHelper.MakeOutputBox(20, 236, 680, 244);

            btn.Click += async (s, e) =>
            {
                var sid = inputS.Text.Trim();
                var csrf = inputC.Text.Trim();
                var cookie = inputCk.Text.Trim();
                if (string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(csrf) || string.IsNullOrEmpty(cookie))
                { UiHelper.AppendLog(output, "All fields are required.", UiHelper.Error); return; }

                btn.Enabled = false; output.Clear();
                UiHelper.AppendLog(output, "Generating join link...", UiHelper.FgMuted);
                try
                {
                    var json = await RobloxApi.GenerateLinkAsync(sid, csrf, cookie);
                    UiHelper.AppendJson(output, json);

                    // Also extract and show the link directly
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("joinCode", out var jc))
                    {
                        var link = $"https://www.roblox.com/games/15532962292?privateServerLinkCode={jc.GetString()}";
                        UiHelper.AppendLog(output, $"\nJoin link: {link}", UiHelper.Accent);
                    }
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { lblS, inputS, lblC, inputC, lblCk, inputCk, btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 6. Get Metadata
    // ═══════════════════════════════════════════════════════════════
    public class GetMetadataPanel : UserControl
    {
        public GetMetadataPanel()
        {
            BackColor = UiHelper.BgDark;

            var lbl = UiHelper.MakeLabel("Private Server ID", 20, 20);
            var input = UiHelper.MakeInput(20, 40);
            var btn = UiHelper.MakeButton("Get Metadata", 20, 76);
            var output = UiHelper.MakeOutputBox(20, 124, 680, 360);

            btn.Click += async (s, e) =>
            {
                var sid = input.Text.Trim();
                if (string.IsNullOrEmpty(sid)) { UiHelper.AppendLog(output, "Server ID required.", UiHelper.Error); return; }

                btn.Enabled = false; output.Clear();
                UiHelper.AppendLog(output, "Fetching metadata...", UiHelper.FgMuted);
                try
                {
                    var json = await RobloxApi.GetMetadataAsync(sid);
                    UiHelper.AppendJson(output, json);
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { lbl, input, btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 7. Get Private Servers
    // ═══════════════════════════════════════════════════════════════
    public class GetPrivateServersPanel : UserControl
    {
        public GetPrivateServersPanel()
        {
            BackColor = UiHelper.BgDark;

            var btn = UiHelper.MakeButton("Get My Private Servers", 20, 20, 180);
            var output = UiHelper.MakeOutputBox(20, 68, 680, 420);

            btn.Click += async (s, e) =>
            {
                btn.Enabled = false; output.Clear();
                UiHelper.AppendLog(output, "Fetching private servers...", UiHelper.FgMuted);
                try
                {
                    var json = await RobloxApi.GetPrivateServersAsync();
                    UiHelper.AppendJson(output, json);
                }
                catch (Exception ex) { UiHelper.AppendLog(output, $"Error: {ex.Message}", UiHelper.Error); }
                finally { btn.Enabled = true; }
            };

            Controls.AddRange(new Control[] { btn, output });
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 8. Update Users (multi-cookie)
    // ═══════════════════════════════════════════════════════════════
    public class UpdateUsersPanel : UserControl
    {
        private FlowLayoutPanel _cookieRows;
        private int _cookieCount = 1;

        public UpdateUsersPanel()
        {
            BackColor = UiHelper.BgDark;
            AutoScroll = true;

            // Windows username
            var lblAcc = UiHelper.MakeLabel("Windows Username (for Jaram path)", 20, 16);
            var inputAcc = UiHelper.MakeInput(20, 36, 220);

            // Universe ID
            var lblUni = UiHelper.MakeLabel("Universe ID", 254, 16);
            var inputUni = UiHelper.MakeInput(254, 36, 200);

            var solsBtn = new Button
            {
                Text = "Sols RNG",
                Location = new Point(462, 34),
                Width = 80,
                Height = 24,
                BackColor = Color.FromArgb(40, 60, 100),
                ForeColor = Color.FromArgb(120, 180, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                Cursor = Cursors.Hand
            };
            solsBtn.FlatAppearance.BorderSize = 0;
            solsBtn.Click += (s, e) => { inputUni.Text = "5361032378"; };

            // Cookie list header
            var lblCookies = UiHelper.MakeLabel("Roblox Cookies (.ROBLOSECURITY)", 20, 82);

            var addBtn = new Button
            {
                Text = "+ Add Cookie",
                Location = new Point(20, 100),
                Width = 120,
                Height = 26,
                BackColor = Color.FromArgb(40, 80, 60),
                ForeColor = UiHelper.Success,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand
            };
            addBtn.FlatAppearance.BorderSize = 0;

            _cookieRows = new FlowLayoutPanel
            {
                Location = new Point(20, 132),
                Width = 700,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = UiHelper.BgDark
            };

            AddCookieRow();

            addBtn.Click += (s, e) => AddCookieRow();

            var runBtn = UiHelper.MakeButton("Run Update", 20, 0, 130);
            runBtn.BackColor = Color.FromArgb(60, 140, 80);

            var output = UiHelper.MakeOutputBox(20, 0, 700, 220);

            // Use a bottom panel so output stays visible
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 260,
                BackColor = UiHelper.BgDark,
                Padding = new Padding(20, 8, 20, 8)
            };
            runBtn.Location = new Point(20, 8);
            output.Location = new Point(20, 48);
            output.Size = new Size(700, 200);
            bottomPanel.Controls.Add(runBtn);
            bottomPanel.Controls.Add(output);

            runBtn.Click += async (s, e) =>
            {
                var accName = inputAcc.Text.Trim();
                var uniId = inputUni.Text.Trim();
                if (string.IsNullOrEmpty(accName) || string.IsNullOrEmpty(uniId))
                { UiHelper.AppendLog(output, "Windows username and Universe ID are required.", UiHelper.Error); return; }

                var cookies = GetCookies();
                if (cookies.Length == 0)
                { UiHelper.AppendLog(output, "Enter at least one cookie.", UiHelper.Error); return; }

                runBtn.Enabled = false; output.Clear();
                UiHelper.AppendLog(output, $"Starting update for {cookies.Length} cookie(s)...", UiHelper.Accent);

                int ok = 0, fail = 0;
                for (int i = 0; i < cookies.Length; i++)
                {
                    try
                    {
                        var result = await RobloxApi.UpdateSingleUserAsync(
                            cookies[i], uniId, i + 1, cookies.Length,
                            msg => UiHelper.AppendLog(output, msg, UiHelper.FgMuted));

                        RobloxApi.WriteUsersJson(accName, result);
                        UiHelper.AppendLog(output, $"✓ {result.Username} — {result.JoinLink}", UiHelper.Success);
                        ok++;
                    }
                    catch (Exception ex)
                    {
                        UiHelper.AppendLog(output, $"✗ Cookie #{i + 1}: {ex.Message}", UiHelper.Error);
                        fail++;
                    }
                }

                UiHelper.AppendLog(output,
                    $"\nDone. {ok} succeeded, {fail} failed.",
                    ok > 0 ? UiHelper.Success : UiHelper.Error);
                runBtn.Enabled = true;
            };

            var topScroll = new Panel
            {
                Location = new Point(0, 0),
                Width = 760,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            topScroll.Controls.AddRange(new Control[] { lblAcc, inputAcc, lblUni, inputUni, solsBtn, lblCookies, addBtn, _cookieRows });

            Controls.Add(topScroll);
            Controls.Add(bottomPanel);
        }

        private void AddCookieRow()
        {
            var row = new Panel
            {
                Width = 700,
                Height = 30,
                BackColor = UiHelper.BgDark,
                Margin = new Padding(0, 0, 0, 4)
            };

            var lbl = new Label
            {
                Text = $"#{_cookieCount}",
                Location = new Point(0, 7),
                Width = 28,
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Consolas", 8.5f)
            };

            var tb = new TextBox
            {
                Location = new Point(32, 3),
                Width = 600,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8.5f),
                UseSystemPasswordChar = true,
                PlaceholderText = ".ROBLOSECURITY cookie..."
            };

            var del = new Button
            {
                Text = "✕",
                Location = new Point(638, 3),
                Width = 26,
                Height = 24,
                BackColor = Color.FromArgb(80, 30, 30),
                ForeColor = UiHelper.Error,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                Cursor = Cursors.Hand
            };
            del.FlatAppearance.BorderSize = 0;
            del.Click += (s, e) => { _cookieRows.Controls.Remove(row); };

            row.Controls.AddRange(new Control[] { lbl, tb, del });
            _cookieRows.Controls.Add(row);
            _cookieCount++;
        }

        private string[] GetCookies()
        {
            var list = new System.Collections.Generic.List<string>();
            foreach (Control row in _cookieRows.Controls)
            {
                foreach (Control c in row.Controls)
                {
                    if (c is TextBox tb && !string.IsNullOrWhiteSpace(tb.Text))
                        list.Add(tb.Text.Trim());
                }
            }
            return list.ToArray();
        }
    }
}
