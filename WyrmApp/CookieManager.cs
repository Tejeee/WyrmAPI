using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WyrmApp
{
    public static class CookieManager
    {
        private static string _configFolder;
        private static string _cookiePath;
        public static string Cookie { get; private set; } = "";

        public static event Action? CookieChanged;

        static CookieManager()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _configFolder = Path.Combine(baseDir, "config");
            _cookiePath = Path.Combine(_configFolder, "cookies.txt");
        }

        public static async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(_configFolder))
                    Directory.CreateDirectory(_configFolder);

                if (File.Exists(_cookiePath))
                {
                    var raw = File.ReadAllText(_cookiePath).Trim();
                    Cookie = StripPrefix(raw);
                }
            });
            CookieChanged?.Invoke();
        }

        public static void Save(string raw)
        {
            if (!Directory.Exists(_configFolder))
                Directory.CreateDirectory(_configFolder);

            Cookie = StripPrefix(raw.Trim());
            File.WriteAllText(_cookiePath, Cookie);
            CookieChanged?.Invoke();
        }

        public static string StripPrefix(string c) =>
            c.StartsWith(".ROBLOSECURITY=") ? c.Substring(".ROBLOSECURITY=".Length) : c;

        public static System.Net.CookieContainer BuildCookieContainer()
        {
            var jar = new System.Net.CookieContainer();
            jar.Add(new Uri("https://roblox.com"), new System.Net.Cookie(".ROBLOSECURITY", Cookie, "/", ".roblox.com"));
            return jar;
        }

        /// <summary>Build a cookie container from an arbitrary raw cookie string.</summary>
        public static System.Net.CookieContainer BuildCookieContainer(string rawCookie)
        {
            var clean = StripPrefix(rawCookie.Trim());
            var jar = new System.Net.CookieContainer();
            jar.Add(new Uri("https://roblox.com"), new System.Net.Cookie(".ROBLOSECURITY", clean, "/", ".roblox.com"));
            return jar;
        }
    }
}
