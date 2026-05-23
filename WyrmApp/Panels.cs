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

            var lblAcc = UiHelper.MakeLabel("Windows User (for Jaram path)", 20, 16);
            var inputAcc = new ComboBox
            {
                Location = new Point(20, 36),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 8.5f)
            };
            try
            {
                var skip = new[] { "Public", "Default", "Default User", "All Users" };
                foreach (var d in System.IO.Directory.GetDirectories(@"C:\Users"))
                {
                    var name = System.IO.Path.GetFileName(d);
                    if (!Array.Exists(skip, s => s.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        inputAcc.Items.Add(name);
                }
                var current = Environment.UserName;
                if (inputAcc.Items.Contains(current))
                    inputAcc.SelectedItem = current;
                else if (inputAcc.Items.Count > 0)
                    inputAcc.SelectedIndex = 0;
            }
            catch { }

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

            // Load previously saved cookies, or start with one blank row
            var saved = CookieManager.LoadUpdateCookies();
            if (saved.Count > 0)
                foreach (var c in saved) AddCookieRow(c);
            else
                AddCookieRow();
            addBtn.Click += (s, e) => { AddCookieRow(); SaveCookies(); };

            var runBtn = UiHelper.MakeButton("Run Update", 20, 0, 130);
            runBtn.BackColor = Color.FromArgb(60, 140, 80);

            var output = UiHelper.MakeOutputBox(20, 0, 700, 220);

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

        private void SaveCookies()
        {
            var list = new System.Collections.Generic.List<string>();
            foreach (Control row in _cookieRows.Controls)
                foreach (Control c in row.Controls)
                    if (c is TextBox tb && !string.IsNullOrWhiteSpace(tb.Text))
                        list.Add(tb.Text.Trim());
            CookieManager.SaveUpdateCookies(list);
        }

        private void AddCookieRow(string? prefill = null)
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
                Width = 554,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8.5f),
                UseSystemPasswordChar = true,
                PlaceholderText = ".ROBLOSECURITY cookie..."
            };
            if (prefill != null) tb.Text = prefill;
            tb.TextChanged += (s, e) => SaveCookies();

            var copy = new Button
            {
                Text = "Copy",
                Location = new Point(592, 3),
                Width = 46,
                Height = 24,
                BackColor = Color.FromArgb(40, 60, 100),
                ForeColor = UiHelper.Accent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                Cursor = Cursors.Hand
            };
            copy.FlatAppearance.BorderSize = 0;
            copy.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    Clipboard.SetText(tb.Text);
                    copy.Text = "✓";
                    var t = new System.Windows.Forms.Timer { Interval = 1500 };
                    t.Tick += (_, __) => { copy.Text = "Copy"; t.Stop(); t.Dispose(); };
                    t.Start();
                }
            };

            var del = new Button
            {
                Text = "✕",
                Location = new Point(644, 3),
                Width = 26,
                Height = 24,
                BackColor = Color.FromArgb(80, 30, 30),
                ForeColor = UiHelper.Error,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8f),
                Cursor = Cursors.Hand
            };
            del.FlatAppearance.BorderSize = 0;
            del.Click += (s, e) => { _cookieRows.Controls.Remove(row); SaveCookies(); };

            row.Controls.AddRange(new Control[] { lbl, tb, copy, del });
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

        // Called from MainForm.SendCookieToUpdateUsers
        public void AddOrReplaceCookie(string cookie)
        {
            // Fill the first empty row if one exists, otherwise add a new row
            foreach (Control row in _cookieRows.Controls)
            {
                foreach (Control c in row.Controls)
                {
                    if (c is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
                    {
                        tb.Text = cookie; // TextChanged fires SaveCookies automatically
                        return;
                    }
                }
            }
            AddCookieRow(cookie); // prefill triggers TextChanged -> SaveCookies
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // 9. Login & Cookie Extractor
    // ═══════════════════════════════════════════════════════════════
    public class LoginPanel : UserControl
    {
        public LoginPanel()
        {
            BackColor = UiHelper.BgDark;

            // ── Instructions ────────────────────────────────────────
            var lblInfo = new Label
            {
                Text = "Log in to Roblox in the browser below. Once signed in, click \"Extract Cookie\". Use \"Fresh Session\" to wipe the browser state (same as closing an incognito window).",
                Location = new Point(20, 12),
                AutoSize = false,
                Width = 820,
                Height = 34,
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Segoe UI", 9f)
            };

            // ── Login / Signup toggle ───────────────────────────────
            var loginRadio = new RadioButton
            {
                Text = "Login",
                Location = new Point(20, 48),
                AutoSize = true,
                Checked = true,
                ForeColor = UiHelper.FgNormal,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            var signupRadio = new RadioButton
            {
                Text = "Signup",
                Location = new Point(90, 48),
                AutoSize = true,
                ForeColor = UiHelper.FgNormal,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            // ── Cookie row ──────────────────────────────────────────
            var lblCookie = UiHelper.MakeLabel("Extracted Cookie", 20, 72);

            var cookieBox = new TextBox
            {
                Location = new Point(20, 90),
                Width = 560,
                Height = 24,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8f),
                ReadOnly = true,
                UseSystemPasswordChar = true,
                PlaceholderText = "Cookie will appear here after extraction..."
            };

            var showBtn = new Button
            {
                Text = "👁",
                Location = new Point(586, 89),
                Width = 30,
                Height = 26,
                BackColor = Color.FromArgb(40, 40, 58),
                ForeColor = UiHelper.FgMuted,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f)
            };
            showBtn.FlatAppearance.BorderSize = 0;
            showBtn.Click += (s, e) =>
            {
                cookieBox.UseSystemPasswordChar = !cookieBox.UseSystemPasswordChar;
                showBtn.ForeColor = cookieBox.UseSystemPasswordChar ? UiHelper.FgMuted : UiHelper.Accent;
            };

            var copyBtn = new Button
            {
                Text = "Copy",
                Location = new Point(622, 89),
                Width = 60,
                Height = 26,
                BackColor = Color.FromArgb(40, 60, 100),
                ForeColor = UiHelper.Accent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5f)
            };
            copyBtn.FlatAppearance.BorderSize = 0;
            copyBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(cookieBox.Text))
                {
                    Clipboard.SetText(cookieBox.Text);
                    copyBtn.Text = "✓ Copied";
                    var t = new System.Windows.Forms.Timer { Interval = 1500 };
                    t.Tick += (_, __) => { copyBtn.Text = "Copy"; t.Stop(); t.Dispose(); };
                    t.Start();
                }
            };

            var saveBtn = new Button
            {
                Text = "Save as Main Cookie",
                Location = new Point(688, 89),
                Width = 150,
                Height = 26,
                BackColor = Color.FromArgb(40, 80, 60),
                ForeColor = UiHelper.Success,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5f)
            };
            saveBtn.FlatAppearance.BorderSize = 0;
            saveBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(cookieBox.Text))
                {
                    CookieManager.Save(cookieBox.Text);
                    saveBtn.Text = "✓ Saved!";
                    var t = new System.Windows.Forms.Timer { Interval = 1500 };
                    t.Tick += (_, __) => { saveBtn.Text = "Save as Main Cookie"; t.Stop(); t.Dispose(); };
                    t.Start();
                }
            };

            // ── Buttons row ─────────────────────────────────────────
            var extractBtn = new Button
            {
                Text = "⟳  Extract Cookie",
                Location = new Point(20, 126),
                Width = 150,
                Height = 30,
                BackColor = Color.FromArgb(60, 40, 100),
                ForeColor = Color.FromArgb(180, 140, 255),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            extractBtn.FlatAppearance.BorderSize = 0;

            var freshBtn = new Button
            {
                Text = "↺  Fresh Session",
                Location = new Point(180, 126),
                Width = 140,
                Height = 30,
                BackColor = Color.FromArgb(60, 40, 30),
                ForeColor = Color.FromArgb(255, 160, 80),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f)
            };
            freshBtn.FlatAppearance.BorderSize = 0;

            var sendOnExtractChk = new CheckBox
            {
                Text = "Send to Update Users on Extract",
                Location = new Point(330, 131),
                AutoSize = true,
                ForeColor = UiHelper.FgNormal,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand
            };

            var disclaimerLbl = new Label
            {
                Text = "⚠ Do not enable if you are changing passwords — disable this and use the checkbox in the password section instead.",
                Location = new Point(330, 153),
                AutoSize = false,
                Width = 510,
                Height = 28,
                ForeColor = Color.FromArgb(255, 180, 60),
                Font = new Font("Segoe UI", 7.5f)
            };

            var statusLbl = new Label
            {
                Location = new Point(20, 164),
                AutoSize = false,
                Width = 820,
                Height = 20,
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Segoe UI", 8.5f),
                Text = ""
            };

            // ── Change My Password section ───────────────────────────
            var divider = new Label
            {
                Location = new Point(20, 192),
                Size = new Size(820, 1),
                BackColor = Color.FromArgb(50, 50, 70),
                AutoSize = false
            };

            var lblPwTitle = new Label
            {
                Text = "Change My Password",
                Location = new Point(20, 201),
                AutoSize = true,
                ForeColor = UiHelper.Accent,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };

            var lblPwHint = new Label
            {
                Text = "Cookie is extracted automatically if not already done. After the password change, the new cookie replaces the one above and the session is refreshed.",
                Location = new Point(20, 221),
                AutoSize = false,
                Width = 820,
                Height = 28,
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Segoe UI", 8.5f)
            };

            var lblCurPw = UiHelper.MakeLabel("Current Password", 20, 256);

            var curPwBox = new TextBox
            {
                Location = new Point(20, 274),
                Width = 260,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8.5f),
                UseSystemPasswordChar = true,
                PlaceholderText = "Enter current password…"
            };

            var showCurPwBtn = new Button
            {
                Text = "👁",
                Location = new Point(286, 273),
                Width = 30,
                Height = 26,
                BackColor = Color.FromArgb(40, 40, 58),
                ForeColor = UiHelper.FgMuted,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f)
            };
            showCurPwBtn.FlatAppearance.BorderSize = 0;
            showCurPwBtn.Click += (s, e) =>
            {
                curPwBox.UseSystemPasswordChar = !curPwBox.UseSystemPasswordChar;
                showCurPwBtn.ForeColor = curPwBox.UseSystemPasswordChar ? UiHelper.FgMuted : UiHelper.Accent;
            };

            var lblNewPw = UiHelper.MakeLabel("New Password", 20, 308);

            var newPwBox = new TextBox
            {
                Location = new Point(20, 326),
                Width = 260,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8.5f),
                UseSystemPasswordChar = true,
                PlaceholderText = "Enter new password…"
            };

            var showPwBtn = new Button
            {
                Text = "👁",
                Location = new Point(286, 325),
                Width = 30,
                Height = 26,
                BackColor = Color.FromArgb(40, 40, 58),
                ForeColor = UiHelper.FgMuted,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f)
            };
            showPwBtn.FlatAppearance.BorderSize = 0;
            showPwBtn.Click += (s, e) =>
            {
                newPwBox.UseSystemPasswordChar = !newPwBox.UseSystemPasswordChar;
                showPwBtn.ForeColor = newPwBox.UseSystemPasswordChar ? UiHelper.FgMuted : UiHelper.Accent;
            };

            var changePwBtn = new Button
            {
                Text = "🔑  Change Password",
                Location = new Point(20, 362),
                Width = 160,
                Height = 30,
                BackColor = Color.FromArgb(60, 50, 20),
                ForeColor = Color.FromArgb(255, 210, 80),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            changePwBtn.FlatAppearance.BorderSize = 0;

            // Checkbox: send new cookie to Update Users after change
            var sendToUpdateChk = new CheckBox
            {
                Text = "Send new cookie to Update Users tab",
                Location = new Point(192, 368),
                AutoSize = true,
                ForeColor = UiHelper.FgMuted,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand
            };

            var pwStatusLbl = new Label
            {
                Location = new Point(20, 400),
                AutoSize = false,
                Width = 820,
                Height = 20,
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Segoe UI", 8.5f),
                Text = ""
            };

            // ── Browser panel ───────────────────────────────────────
            var browserPanel = new Panel
            {
                Location = new Point(20, 428),
                Width = 820,
                Height = 480,
                BackColor = Color.FromArgb(10, 10, 16),
                BorderStyle = BorderStyle.FixedSingle
            };

            // WebView2 held in a closure variable so Fresh Session can replace it
            Microsoft.Web.WebView2.WinForms.WebView2? webView = null;

            // Switching mode navigates the already-open browser
            loginRadio.CheckedChanged += (s, e) =>
            {
                if (!loginRadio.Checked || webView?.CoreWebView2 == null) return;
                webView.CoreWebView2.Navigate("https://www.roblox.com/login");
            };
            signupRadio.CheckedChanged += (s, e) =>
            {
                if (!signupRadio.Checked || webView?.CoreWebView2 == null) return;
                webView.CoreWebView2.Navigate("https://www.roblox.com/account/signupredir");
            };

            async Task InitWebViewAsync()
            {
                try
                {
                    var tempDir = System.IO.Path.Combine(
                        System.IO.Path.GetTempPath(),
                        "WyrmSession_" + Guid.NewGuid().ToString("N"));

                    var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment
                        .CreateAsync(null, tempDir);

                    webView = new Microsoft.Web.WebView2.WinForms.WebView2 { Dock = DockStyle.Fill };
                    browserPanel.Controls.Add(webView);
                    await webView.EnsureCoreWebView2Async(env);

                    bool goSignup = signupRadio.Checked;
                    string startUrl = goSignup
                        ? "https://www.roblox.com/account/signupredir"
                        : "https://www.roblox.com/login";
                    webView.CoreWebView2.Navigate(startUrl);


                    statusLbl.ForeColor = UiHelper.FgMuted;
                    statusLbl.Text = goSignup
                        ? "Browser ready — sign up then click Extract Cookie."
                        : "Browser ready — log in then click Extract Cookie.";
                }
                catch (Exception ex)
                {
                    statusLbl.ForeColor = UiHelper.Error;
                    statusLbl.Text = $"WebView2 error: {ex.Message}";
                }
            }

            // ── Shared Fresh Session logic ───────────────────────────
            async Task DoFreshSessionAsync()
            {
                freshBtn.Enabled = false;
                statusLbl.ForeColor = UiHelper.FgMuted;
                statusLbl.Text = "Resetting session…";
                cookieBox.Text = "";

                if (webView != null)
                {
                    browserPanel.Controls.Remove(webView);
                    webView.Dispose();
                    webView = null;
                }

                await InitWebViewAsync();
                freshBtn.Enabled = true;
            }

            // ── Shared Extract logic ─────────────────────────────────
            async Task<string?> DoExtractAsync()
            {
                if (webView?.CoreWebView2 == null)
                {
                    statusLbl.ForeColor = UiHelper.Error;
                    statusLbl.Text = "Browser not ready yet.";
                    return null;
                }

                statusLbl.ForeColor = UiHelper.FgMuted;
                statusLbl.Text = "Extracting…";

                try
                {
                    var cookieList = await webView.CoreWebView2.CookieManager
                        .GetCookiesAsync("https://www.roblox.com");

                    string? found = null;
                    foreach (var c in cookieList)
                        if (c.Name == ".ROBLOSECURITY") { found = c.Value; break; }

                    if (found == null)
                    {
                        statusLbl.ForeColor = UiHelper.Error;
                        statusLbl.Text = "Cookie not found — are you logged in?";
                        return null;
                    }

                    cookieBox.Text = found;
                    statusLbl.ForeColor = UiHelper.Success;
                    statusLbl.Text = "✓ Cookie extracted successfully.";
                    return found;
                }
                catch (Exception ex)
                {
                    statusLbl.ForeColor = UiHelper.Error;
                    statusLbl.Text = $"Error: {ex.Message}";
                    return null;
                }
            }

            // Lazy init on first visit
            this.VisibleChanged += async (s, e) =>
            {
                if (!Visible || webView != null) return;
                await InitWebViewAsync();
            };

            // ── Fresh Session button ─────────────────────────────────
            freshBtn.Click += async (s, e) => await DoFreshSessionAsync();

            // ── Extract Cookie button ────────────────────────────────
            extractBtn.Click += async (s, e) =>
            {
                extractBtn.Enabled = false;
                var extracted = await DoExtractAsync();
                if (extracted != null && sendOnExtractChk.Checked)
                {
                    Control? cur = this.Parent;
                    while (cur != null && cur is not MainForm) cur = cur.Parent;
                    if (cur is MainForm mf) mf.SendCookieToUpdateUsers(extracted);
                    statusLbl.Text += "  →  Sent to Update Users.";
                }
                extractBtn.Enabled = true;
            };

            // ── Change Password button ───────────────────────────────
            changePwBtn.Click += async (s, e) =>
            {
                var curPw = curPwBox.Text;
                var newPw = newPwBox.Text;
                if (string.IsNullOrEmpty(curPw))
                {
                    pwStatusLbl.ForeColor = UiHelper.Error;
                    pwStatusLbl.Text = "Enter your current password first.";
                    return;
                }
                if (string.IsNullOrEmpty(newPw))
                {
                    pwStatusLbl.ForeColor = UiHelper.Error;
                    pwStatusLbl.Text = "Enter a new password first.";
                    return;
                }

                changePwBtn.Enabled = false;
                pwStatusLbl.ForeColor = UiHelper.FgMuted;
                pwStatusLbl.Text = "Extracting cookie…";

                // Auto-extract the cookie if the box is empty
                string? cookie = cookieBox.Text.Trim();
                if (string.IsNullOrEmpty(cookie))
                {
                    cookie = await DoExtractAsync();
                    if (string.IsNullOrEmpty(cookie))
                    {
                        pwStatusLbl.ForeColor = UiHelper.Error;
                        pwStatusLbl.Text = "Could not extract cookie — are you logged in?";
                        changePwBtn.Enabled = true;
                        return;
                    }
                }

                try
                {
                    pwStatusLbl.ForeColor = UiHelper.FgMuted;
                    pwStatusLbl.Text = "Changing password…";

                    var newCookie = await RobloxApi.ChangePasswordAndGetCookieAsync(cookie, curPw, newPw);

                    // Place the new cookie back into the extracted cookie box
                    cookieBox.Text = newCookie;
                    statusLbl.ForeColor = UiHelper.Success;
                    statusLbl.Text = "✓ New cookie stored above.";

                    pwStatusLbl.ForeColor = UiHelper.Success;
                    pwStatusLbl.Text = "✓ Password changed successfully. Session refreshing…";

                    // If the checkbox is ticked, pass cookie to Update Users tab
                    if (sendToUpdateChk.Checked)
                    {
                        Control? cur = this.Parent;
                        while (cur != null && cur is not MainForm) cur = cur.Parent;
                        if (cur is MainForm mf) mf.SendCookieToUpdateUsers(newCookie);
                    }

                    // Refresh the browser session automatically
                    await DoFreshSessionAsync();

                    pwStatusLbl.ForeColor = UiHelper.Success;
                    pwStatusLbl.Text = "✓ Password changed. New cookie extracted. Session refreshed.";
                }
                catch (Exception ex)
                {
                    pwStatusLbl.ForeColor = UiHelper.Error;
                    pwStatusLbl.Text = $"Error: {ex.Message}";
                }
                finally { changePwBtn.Enabled = true; }
            };

            Controls.AddRange(new Control[]
            {
                lblInfo,
                loginRadio, signupRadio,
                lblCookie, cookieBox, showBtn, copyBtn, saveBtn,
                extractBtn, freshBtn, sendOnExtractChk, disclaimerLbl,
                statusLbl,
                divider,
                lblPwTitle, lblPwHint,
                lblCurPw, curPwBox, showCurPwBtn,
                lblNewPw, newPwBox, showPwBtn,
                changePwBtn, sendToUpdateChk,
                pwStatusLbl,
                browserPanel
            });
        }
    }
}
