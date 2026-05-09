using System;
using System.Drawing;
using System.Windows.Forms;

namespace WyrmApp
{
    /// <summary>Reusable dark-themed label + textbox row.</summary>
    public static class UiHelper
    {
        public static readonly Color BgDark    = Color.FromArgb(22, 22, 30);
        public static readonly Color BgInput   = Color.FromArgb(32, 32, 45);
        public static readonly Color Accent    = Color.FromArgb(100, 200, 255);
        public static readonly Color FgNormal  = Color.FromArgb(220, 220, 230);
        public static readonly Color FgMuted   = Color.FromArgb(130, 130, 150);
        public static readonly Color Success   = Color.FromArgb(100, 220, 130);
        public static readonly Color Error     = Color.FromArgb(255, 100, 100);
        public static readonly Color Warning   = Color.FromArgb(255, 200, 80);

        public static Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = FgMuted,
                Font = new Font("Segoe UI", 8.5f)
            };
        }

        public static TextBox MakeInput(int x, int y, int width = 340, bool password = false)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                BackColor = BgInput,
                ForeColor = FgNormal,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9f),
                UseSystemPasswordChar = password
            };
        }

        public static Button MakeButton(string text, int x, int y, int width = 150)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Width = width,
                Height = 32,
                BackColor = Color.FromArgb(50, 130, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 155, 225);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(40, 110, 175);
            return btn;
        }

        public static RichTextBox MakeOutputBox(int x, int y, int width, int height)
        {
            return new RichTextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.FromArgb(14, 14, 20),
                ForeColor = FgNormal,
                Font = new Font("Consolas", 8.5f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                WordWrap = true
            };
        }

        public static void AppendLog(RichTextBox box, string text, Color? color = null)
        {
            if (box.InvokeRequired)
            {
                box.Invoke(() => AppendLog(box, text, color));
                return;
            }
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color ?? FgNormal;
            box.AppendText(text + "\n");
            box.ScrollToCaret();
        }

        public static void AppendJson(RichTextBox box, string raw)
        {
            try
            {
                var pretty = System.Text.Json.JsonSerializer.Serialize(
                    System.Text.Json.JsonDocument.Parse(raw).RootElement,
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                AppendLog(box, pretty, Color.FromArgb(180, 230, 180));
            }
            catch
            {
                AppendLog(box, raw);
            }
        }

        public static Panel MakeSectionPanel() => new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = BgDark,
            Padding = new Padding(20)
        };
    }
}
