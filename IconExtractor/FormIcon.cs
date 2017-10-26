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

        public List<IDisposable> disposeList = new List<IDisposable>();

        protected FormMain parent;

        protected string filename;
        protected IconData icons;
        protected long dllReadTime;

        protected int current_icon_group;

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
            foreach (var obj in icons.icon_sizes)
            {
                TabPage tab = new TabPage();
                tab.AutoScroll = true;
                tab.Text = obj.Value;
                tabControl1.TabPages.Add(tab);
            }            
            this.Invalidate();
        }

        private void Pbox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            PictureBox pbox = (PictureBox)sender;
            if (me.Button != MouseButtons.Right) return;

            int id = Convert.ToInt32(pbox.Name.Split(new string[] { "Icon_" }, StringSplitOptions.None)[1]);
            int group = icons.icon_to_group[id];

            tSM_InfoGroup.Text = "Group " + group;
            tSM_InfoID.Text = "Icon " + id;

            // Удаляем элементы из предыдущего меню
            for (int i=tSM.Items.Count-1;i>=0;i--)
            {
                ToolStripItem item = tSM.Items[i];
                if (!item.Name.StartsWith("Temp")) continue;
                tSM.Items.Remove(item);
            }

            var sizes = icons.icons.Where(k => icons.group_icons[group].Contains(k.Key)).Select(k => (k.Value.Width == 0 ? 256 : k.Value.Height)).Distinct().OrderBy(k => k).ToList();

            foreach (var size in sizes)
            {
                ToolStripMenuItem value = new ToolStripMenuItem();
                value.Name = "Temp_" + size + "_" + group;
                value.Text = "Save icon " + size + "x" + size;
                value.Click += SaveIconSize_Click;
                tSM.Items.Insert(tSM.Items.IndexOf(tSM_Line2), value);
            }

            current_icon_group = group;
            tSM.Show(Cursor.Position);
        }

        private void SaveIconSize_Click(object sender, EventArgs e)
        {
            var parts = ((ToolStripMenuItem)sender).Name.Split('_');
            int size = Convert.ToInt32(parts[1]);
            if (size == 256) size = 0;

            int group = Convert.ToInt32(parts[2]);

            var list = icons.icons.Where(k => icons.group_icons[group].Contains(k.Key)).Where(k => k.Value.Height == size).Select(k => k.Value).ToList();
            foreach (var item in list) {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Icon_[group]_[id]_x[size].ico|*.ico";
                dialog.FileName = String.Format("Icon_{0}_{1}_x{2}.ico",group,item.ID,(size==0?256:size));
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK) continue;

                Icon icon = icons.api.getIcon(item.ID);
                using (FileStream fs = new FileStream(dialog.FileName, FileMode.Create))
                    icon.Save(fs);
            }
        }

        private void FormIcon_Load(object sender, EventArgs e)
        {
            label1.Text = String.Format("Resources loaded in {0} seconds ({1}ms)", dllReadTime, dllReadTime);
            var data = "\n\nExecutable name: {0}\n" + "Groups: {1}\n" + "Icons: {2}\n";
            label1.Text += String.Format(data, Path.GetFileName(filename), icons.group_icons.Count, icons.icons.Count);
        }

        private void FormIcon_FormClosed(object sender, FormClosedEventArgs e)
        {
            icons.Dispose();
            parent.onChildClose(this);
        }

        private void FormIcon_Shown(object sender, EventArgs e)
        {
            setIconSizes();
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

                ToolTip tooltip = new ToolTip();
                tooltip.SetToolTip(pbox, "Icon: " + icometa.ID);
                disposeList.Add(tooltip);

                x += width + coordOffset;
                if (x > this.Width - width)
                {
                    x = 0;
                    y += height + coordOffset;
                }
            }
        }

        private void tSM_ViewGroup_Click(object sender, EventArgs e)
        {
            var data = icons.icons.Where(k => icons.group_icons[current_icon_group].Contains(k.Key)).Select(k => k.Value).OrderBy(k => (k.Width == 0 ? 256 : k.Width)).ToList();
            FormViewIcon form = new FormViewIcon();

            int x = 20;
            int y = 20;
            int max_y = 0;

            foreach (var icometa in data)
            {
                Icon icon = icons.api.getIcon(icometa.ID);
                int size = (icometa.Height == 0) ? 256 : icometa.Height;

                int real_size = size;
                if (size < 40) real_size = 40;

                Label label = new Label();
                label.AutoSize = false;
                label.Location = new Point(x, y);
                label.Size = new Size(real_size, 20);
                label.TextAlign = ContentAlignment.TopCenter;
                label.Text = size + "x" + size;

                Label label2 = new Label();
                label2.AutoSize = false;
                label2.Location = new Point(x, y + real_size + 20);
                label2.Size = new Size(real_size, 20);
                label2.TextAlign = ContentAlignment.TopCenter;
                label2.Text = icometa.ID.ToString();

                PictureBox pic = new PictureBox();
                pic.Location = new Point(x, y + 20);
                pic.Size = new Size(real_size, real_size);
                pic.SizeMode = PictureBoxSizeMode.CenterImage;
                pic.Image = icon.ToBitmap();

                form.Controls.Add(label);
                form.Controls.Add(label2);
                form.Controls.Add(pic);

                x += real_size + 20;
                max_y = size < max_y ? max_y : size;
            }

            form.Text = "Icon group #" + current_icon_group + " [" + Path.GetFileName(filename) + "]";
            form.Location = Cursor.Position;
            form.Size = new Size(x + 40, max_y + 100);
            form.Show(this);
        }

        private void FormIcon_ResizeEnd(object sender, EventArgs e)
        {
            tabControl1_SelectedIndexChanged(tabControl1, null);
        }

        private void FormIcon_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized) {
                tabControl1_SelectedIndexChanged(tabControl1, null);
            }
        }
    }
}
