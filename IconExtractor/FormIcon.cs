using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IconExtractor
{
    public partial class FormIcon : Form
    {
        protected FormMain parent;

        protected IconApi api;
        protected List<IconApi.GRPICONDIRENTRY> icons = new List<IconApi.GRPICONDIRENTRY>();
        protected SortedDictionary<int, int> icon_to_group = new SortedDictionary<int, int>();
        protected SortedDictionary<int,string> icon_sizes = new SortedDictionary<int,string>();
        protected string filename;
        protected int groups_count;

        public FormIcon(FormMain parent, String filename)
        {
            this.parent = parent;
            this.filename = filename;

            InitializeComponent();
        }

        public bool updateColors(Color color)
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                tab.BackColor = color;
            }

            this.Invalidate();
            return true;
        }

        protected void InitData()
        {
            api = new IconApi(filename);
            List<IconApi.ICON_GROUP> icon_groups = api.getIconGroups();
            groups_count = icon_groups.Count;

            foreach (var group in icon_groups)
            {
                foreach (var icon in group.icon_groups)
                {
                    int size = (icon.Height == 0) ? 256 : icon.Height;
                    if (!icon_sizes.ContainsKey(size)) icon_sizes.Add(size, size + "x" + size);
                    icons.Add(icon);
                    icon_to_group.Add(icon.ID, group.ID);
                }
            }
        }

        protected void setIconSizes()
        {
            //tabControl1.TabPages.Clear();
            foreach (var obj in icon_sizes)
            {
                TabPage tab = new TabPage();
                tab.AutoScroll = true;
                tab.Text = obj.Value;

                var sorticons = icons.Where(ico => ico.Width == ((obj.Key == 256) ? 0 : obj.Key));
                int x = 0;
                int y = 0;

                int coordOffset = 10;

                foreach (var icometa in sorticons) {
                    Icon icon = api.getIcon(icometa.ID);

                    int width = icometa.Width;
                    if (width == 0) width = 256;

                    int height = icometa.Height;
                    if (height == 0) height = 256;

                    PictureBox pbox = new PictureBox();
                    pbox.Name = "Icon_" + icometa.ID;
                    pbox.Image = icon.ToBitmap();
                    pbox.Location = new Point(x,y);
                    pbox.Size = new Size(width, height);
                    pbox.Click += Pbox_Click;
                    tab.Controls.Add(pbox);

                    x += width + coordOffset;
                    if (x > 1600)
                    {
                        x = 0;
                        y += height + coordOffset;
                    }
                }

                tabControl1.TabPages.Add(tab);
            }

            this.Invalidate();
            //MessageBox.Show("Data: " + String.Join(", ",icon_sizes.Values.ToArray()));
        }

        private void Pbox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            PictureBox pbox = (PictureBox)sender;
            if (me.Button != MouseButtons.Right) return;

            int id = Convert.ToInt32(pbox.Name.Split(new string[] { "Icon_" }, StringSplitOptions.None)[1]);
            tSM_Info.Text = String.Format("Group {0}      (icon {1})",icon_to_group[id],id);

            // Удаляем элементы из предыдущего меню
            for (int i=tSM.Items.Count-1;i>=0;i--)
            {
                ToolStripItem item = tSM.Items[i];
                if (!item.Name.StartsWith("Temp")) continue;
                tSM.Items.Remove(item);
            }

            var all_icons = icon_to_group.Where(x => x.Value == icon_to_group[id]).Select(x => x.Key).ToList();

            ToolStripMenuItem value = new ToolStripMenuItem();
            value.Name = "Temp";
            value.Text = "Hello " + id;
            tSM.Items.Insert(tSM.Items.IndexOf(tSM_Line1) + 1, value);

            tSM.Show(Cursor.Position);
        }

        private void FormIcon_Load(object sender, EventArgs e)
        {
        }

        private void FormIcon_FormClosed(object sender, FormClosedEventArgs e)
        {
            api.Dispose();
            parent.onChildClose(this);
        }

        private void FormIcon_Shown(object sender, EventArgs e)
        {
            InitData();
            setIconSizes();
            var data = "{0} (групп={1}, иконок={2})";
            this.Text = String.Format(data, Path.GetFileName(filename), groups_count, icons.Count);
        }
    }
}
