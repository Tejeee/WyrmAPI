using System;
using System.Drawing;
using System.Windows.Forms;

namespace WyrmApp
{
    public class MainForm : Form
    {
        public MainForm()
        {
            this.Text = "Wyrm API";
            this.Size = new Size(820, 600);
            this.MinimumSize = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 24);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f);

            BuildUI();
            _ = CookieManager.LoadAsync();
        }

        private TabControl tabs = null!;

        private void BuildUI()
        {
            // Header
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = Color.FromArgb(30, 30, 40)
            };
            var title = new Label
            {
                Text = "🐍  Wyrm API",
                ForeColor = Color.FromArgb(100, 200, 255),
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 10)
            };
            header.Controls.Add(title);

            // Cookie strip
            var cookieStrip = new CookieStrip { Dock = DockStyle.Top };

            // Tabs
            tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                DrawMode = TabDrawMode.OwnerDrawFixed,
                ItemSize = new Size(130, 34),
                SizeMode = TabSizeMode.Fixed,
                Padding = new Point(0, 0)
            };
            tabs.DrawItem += Tabs_DrawItem;

            tabs.TabPages.Add(new TabPage("Get Root Place ID") { Tag = new GetRootPlaceIdPanel() });
            tabs.TabPages.Add(new TabPage("CSRF Token") { Tag = new GetCsrfPanel() });
            tabs.TabPages.Add(new TabPage("Create Server") { Tag = new CreateServerPanel() });
            tabs.TabPages.Add(new TabPage("Rename Server") { Tag = new RenameServerPanel() });
            tabs.TabPages.Add(new TabPage("Generate Link") { Tag = new GenerateLinkPanel() });
            tabs.TabPages.Add(new TabPage("Get Metadata") { Tag = new GetMetadataPanel() });
            tabs.TabPages.Add(new TabPage("Private Servers") { Tag = new GetPrivateServersPanel() });
            tabs.TabPages.Add(new TabPage("Update Users") { Tag = new UpdateUsersPanel() });

            foreach (TabPage tp in tabs.TabPages)
            {
                tp.BackColor = Color.FromArgb(22, 22, 30);
                var panel = (UserControl)tp.Tag!;
                panel.Dock = DockStyle.Fill;
                tp.Controls.Add(panel);
            }

            tabs.SelectedIndexChanged += (s, e) =>
            {
                foreach (TabPage tp in tabs.TabPages)
                    tp.BackColor = Color.FromArgb(22, 22, 30);
            };

            var main = new Panel { Dock = DockStyle.Fill };
            main.Controls.Add(tabs);

            this.Controls.Add(main);
            this.Controls.Add(cookieStrip);
            this.Controls.Add(header);
        }

        private void Tabs_DrawItem(object? sender, DrawItemEventArgs e)
        {
            var tab = tabs.TabPages[e.Index];
            bool selected = e.Index == tabs.SelectedIndex;

            var bg = selected ? Color.FromArgb(40, 40, 58) : Color.FromArgb(26, 26, 36);
            var fg = selected ? Color.FromArgb(100, 200, 255) : Color.FromArgb(160, 160, 180);

            using var brush = new SolidBrush(bg);
            e.Graphics.FillRectangle(brush, e.Bounds);

            if (selected)
            {
                using var accent = new SolidBrush(Color.FromArgb(100, 200, 255));
                e.Graphics.FillRectangle(accent, new Rectangle(e.Bounds.X, e.Bounds.Bottom - 2, e.Bounds.Width, 2));
            }

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using var fgBrush = new SolidBrush(fg);
            e.Graphics.DrawString(tab.Text, new Font("Segoe UI", 8.5f, selected ? FontStyle.Bold : FontStyle.Regular), fgBrush, e.Bounds, sf);
        }
    }
}
