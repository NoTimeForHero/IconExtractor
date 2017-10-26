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

        protected class IconData : IDisposable
        {
            public SortedDictionary<int, IconApi.GRPICONDIRENTRY> icons = new SortedDictionary<int, IconApi.GRPICONDIRENTRY>();
            public SortedDictionary<int, string> icon_sizes = new SortedDictionary<int, string>();
            public SortedDictionary<int, int> icon_to_group = new SortedDictionary<int, int>();
            public SortedDictionary<int, List<int>> group_icons = new SortedDictionary<int, List<int>>();
            public SortedDictionary<int, List<int>> size_icons = new SortedDictionary<int, List<int>>();

            public IconApi api;

            public IconData(String filename)
            {
                api = new IconApi(filename);
                List<IconApi.ICON_GROUP> icon_groups = api.getIconGroups();

                foreach (var group in icon_groups)
                {
                    foreach (var icon in group.icon_groups)
                    {
                        int size = (icon.Height == 0) ? 256 : icon.Height;
                        if (!icon_sizes.ContainsKey(size)) icon_sizes.Add(size, size + "x" + size);
                        icons.Add(icon.ID,icon);
                        icon_to_group.Add(icon.ID, group.ID);

                        if (!group_icons.ContainsKey(group.ID)) group_icons.Add(group.ID, new List<int>());
                        group_icons[group.ID].Add(icon.ID);

                        if (!size_icons.ContainsKey(size)) size_icons.Add(size, new List<int>());
                        size_icons[size].Add(icon.ID);
                    }
                }
            }

            public void Dispose()
            {
                api.Dispose();
            }
        }

        public List<PictureBox> disposeList = new List<PictureBox>();

        protected FormMain parent;

        protected string filename;
        protected IconData icons;
        protected long dllReadTime;

        public FormIcon(FormMain parent, String filename)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            this.icons = new IconData(filename);
            watch.Stop();
            dllReadTime = watch.ElapsedMilliseconds;

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

        protected void setIconSizes()
        {
            //tabControl1.TabPages.Clear();
            
            foreach (var obj in icons.icon_sizes)
            {
                TabPage tab = new TabPage();
                tab.AutoScroll = true;
                tab.Text = obj.Value;

                /*

                */

                tabControl1.TabPages.Add(tab);
            }
            

            this.Invalidate();
            //MessageBox.Show("Data: " + String.Join(", ",icon_sizes.Values.ToArray()));
        }

        private void Tab_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Pbox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            PictureBox pbox = (PictureBox)sender;
            if (me.Button != MouseButtons.Right) return;

            int id = Convert.ToInt32(pbox.Name.Split(new string[] { "Icon_" }, StringSplitOptions.None)[1]);
            tSM_Info.Text = String.Format("Group {0}      (icon {1})",icons.icon_to_group[id],id);

            // Удаляем элементы из предыдущего меню
            for (int i=tSM.Items.Count-1;i>=0;i--)
            {
                ToolStripItem item = tSM.Items[i];
                if (!item.Name.StartsWith("Temp")) continue;
                tSM.Items.Remove(item);
            }

            //var all_icons = icon_to_group.Where(x => x.Value == icon_to_group[id]).Select(x => x.Key).ToList();

            ToolStripMenuItem value = new ToolStripMenuItem();
            value.Name = "Temp";
            value.Text = "Hello " + id;
            tSM.Items.Insert(tSM.Items.IndexOf(tSM_Line1) + 1, value);

            tSM.Show(Cursor.Position);
        }

        private void FormIcon_Load(object sender, EventArgs e)
        {
            label1.Text = String.Format("DLL loaded in {0} seconds ({1}ms)", dllReadTime, dllReadTime);
        }

        private void FormIcon_FormClosed(object sender, FormClosedEventArgs e)
        {
            icons.Dispose();
            parent.onChildClose(this);
        }

        private void FormIcon_Shown(object sender, EventArgs e)
        {
            setIconSizes();
            var data = "{0} (групп={1}, иконок={2})";
            this.Text = String.Format(data, Path.GetFileName(filename), icons.group_icons.Count, icons.icons.Count);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabcontrol = (sender as TabControl);
            TabPage tab = tabcontrol.TabPages[tabcontrol.SelectedIndex];

            bool contains = icons.icon_sizes.ContainsValue(tab.Text);
            if (!contains) return;

            // Удаляем предыдущие иконки
            foreach (var obj in disposeList) obj.Dispose();
            disposeList.Clear();

            int size = icons.icon_sizes.First(obj => obj.Value == tab.Text).Key;

            int x = 0;
            int y = 0;

            int coordOffset = 10;

            foreach (var id in icons.size_icons[size])
            {
                var icometa = icons.icons[id];
                Icon icon = icons.api.getIcon(icometa.ID);

                int width = icometa.Width;
                if (width == 0) width = 256;

                int height = icometa.Height;
                if (height == 0) height = 256;

                PictureBox pbox = new PictureBox();
                pbox.Name = "Icon_" + icometa.ID;
                pbox.Image = icon.ToBitmap();
                pbox.Location = new Point(x, y);
                pbox.Size = new Size(width, height);
                pbox.Click += Pbox_Click;
                tab.Controls.Add(pbox);
                disposeList.Add(pbox);

                x += width + coordOffset;
                if (x > 1600)
                {
                    x = 0;
                    y += height + coordOffset;
                }
            }
        }
    }
}
