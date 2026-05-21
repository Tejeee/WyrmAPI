using System;
using System.Drawing;
using System.Windows.Forms;

namespace WyrmApp
{
    public class CookieStrip : UserControl
    {
        private TextBox _cookieBox;
        private Label _statusLabel;

        public CookieStrip()
        {
            Height = 46;
            BackColor = Color.FromArgb(26, 26, 38);
            Padding = new Padding(12, 6, 12, 6);

            var lbl = new Label
            {
                Text = "Default Cookie:",
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Segoe UI", 8.5f),
                AutoSize = true,
                Location = new Point(14, 14)
            };

            _cookieBox = new TextBox
            {
                Location = new Point(110, 11),
                Width = 400,
                BackColor = UiHelper.BgInput,
                ForeColor = UiHelper.FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 8.5f),
                UseSystemPasswordChar = true,
                PlaceholderText = "Paste .ROBLOSECURITY cookie here..."
            };

            var saveBtn = UiHelper.MakeButton("Save Cookie", 518, 9, 110);
            saveBtn.Height = 26;
            saveBtn.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            saveBtn.Click += (s, e) =>
            {
                var val = _cookieBox.Text.Trim();
                if (string.IsNullOrEmpty(val)) { SetStatus("Cookie cannot be empty.", UiHelper.Error); return; }
                CookieManager.Save(val);
                SetStatus("Cookie saved!", UiHelper.Success);
            };

            _statusLabel = new Label
            {
                Location = new Point(636, 14),
                AutoSize = true,
                ForeColor = UiHelper.FgMuted,
                Font = new Font("Segoe UI", 8.5f)
            };

            CookieManager.CookieChanged += () =>
            {
                if (InvokeRequired) { Invoke(() => OnCookieChanged()); return; }
                OnCookieChanged();
            };

            Controls.AddRange(new Control[] { lbl, _cookieBox, saveBtn, _statusLabel });
        }

        private void OnCookieChanged()
        {
            if (!string.IsNullOrEmpty(CookieManager.Cookie))
            {
                _cookieBox.Text = CookieManager.Cookie;
                SetStatus("Cookie loaded ✓", UiHelper.Success);
            }
        }

        private void SetStatus(string msg, Color color)
        {
            _statusLabel.Text = msg;
            _statusLabel.ForeColor = color;
        }
    }
}
