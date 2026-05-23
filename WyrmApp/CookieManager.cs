using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WyrmApp
{
    public static class CookieManager
    {
        private static string _configFolder;
        private static string _configPath;
        public static string Cookie { get; private set; } = "";

        public static event Action? CookieChanged;

        static CookieManager()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _configFolder = Path.Combine(baseDir, "config");
            _configPath = Path.Combine(_configFolder, "WyrmCookiesConfig.txt");
        }

        // ── Internal helpers ────────────────────────────────────────

        private static void EnsureFolder()
        {
            if (!Directory.Exists(_configFolder))
                Directory.CreateDirectory(_configFolder);
        }

        /// <summary>
        /// Parse the config file into a dictionary of sections.
        /// Sections start with [SectionName] and contain key=value lines.
        /// Lines without '=' are treated as bare values under the key "value".
        /// </summary>
        private static Dictionary<string, List<string>> ReadSections()
        {
            var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(_configPath)) return result;

            string? section = null;
            foreach (var raw in File.ReadAllLines(_configPath))
            {
                var line = raw.Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line[1..^1];
                    if (!result.ContainsKey(section))
                        result[section] = new List<string>();
                }
                else if (section != null && line.Length > 0 && !line.StartsWith("#"))
                {
                    result[section].Add(line);
                }
            }
            return result;
        }

        private static void WriteSections(Dictionary<string, List<string>> sections)
        {
            EnsureFolder();
            var lines = new List<string>();
            foreach (var (sec, values) in sections)
            {
                lines.Add($"[{sec}]");
                lines.AddRange(values);
                lines.Add("");
            }
            File.WriteAllLines(_configPath, lines);
        }

        // ── Main cookie (used by all other panels) ──────────────────

        public static async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                var sections = ReadSections();
                if (sections.TryGetValue("MainCookie", out var vals) && vals.Count > 0)
                    Cookie = StripPrefix(vals[0]);
            });
            CookieChanged?.Invoke();
        }

        public static void Save(string raw)
        {
            Cookie = StripPrefix(raw.Trim());
            var sections = ReadSections();
            sections["MainCookie"] = new List<string> { Cookie };
            WriteSections(sections);
            CookieChanged?.Invoke();
        }

        // ── Update-Users multi-cookie list ──────────────────────────

        public static List<string> LoadUpdateCookies()
        {
            var sections = ReadSections();
            if (!sections.TryGetValue("UpdateCookies", out var vals))
                return new List<string>();
            return vals.Select(StripPrefix).Where(c => c.Length > 0).ToList();
        }

        // Debounce timer so rapid keystrokes don't thrash the file
        private static System.Threading.Timer? _saveDebounce;
        private static readonly object _saveLock = new();

        public static void SaveUpdateCookies(IEnumerable<string> cookies)
        {
            // Snapshot immediately (the TextBoxes may change again before the timer fires)
            var clean = cookies.Select(c => StripPrefix(c.Trim()))
                               .Where(c => c.Length > 0)
                               .ToList();

            lock (_saveLock)
            {
                _saveDebounce?.Dispose();
                _saveDebounce = new System.Threading.Timer(_ =>
                {
                    lock (_saveLock)
                    {
                        try
                        {
                            var sections = ReadSections();
                            sections["UpdateCookies"] = clean;
                            WriteSections(sections);
                        }
                        catch { /* non-fatal — next keystroke will retry */ }
                        _saveDebounce = null;
                    }
                }, null, 400, System.Threading.Timeout.Infinite);
            }
        }

        // ── Shared helpers ──────────────────────────────────────────

        public static string StripPrefix(string c) =>
            c.StartsWith(".ROBLOSECURITY=") ? c[".ROBLOSECURITY=".Length..] : c;

        public static System.Net.CookieContainer BuildCookieContainer()
        {
            var jar = new System.Net.CookieContainer();
            jar.Add(new Uri("https://roblox.com"), new System.Net.Cookie(".ROBLOSECURITY", Cookie, "/", ".roblox.com"));
            return jar;
        }

        public static System.Net.CookieContainer BuildCookieContainer(string rawCookie)
        {
            var clean = StripPrefix(rawCookie.Trim());
            var jar = new System.Net.CookieContainer();
            jar.Add(new Uri("https://roblox.com"), new System.Net.Cookie(".ROBLOSECURITY", clean, "/", ".roblox.com"));
            return jar;
        }
    }
}
