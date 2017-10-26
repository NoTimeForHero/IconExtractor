using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IconExtractor
{
    public partial class FormMain : Form
    {
        public static readonly int MAX_LATEST_FILES = 10;

        protected static FormMain self;
        protected List<FormIcon> childs = new List<FormIcon>();
        protected Color child_color = SystemColors.Control;
        protected string[] system_dlls =
        {
            "lol_test_okay.dll", "shell32.dll", "compstui.dll", "ieframe.dll", "mmcndmgr.dll", "moricons.dll", "netshell.dll", "pifmgr.dll", "setupapi.dll", "wmploc.dll", "wpdshext.dll",
            "imageres.dll", "ddores.dll", "AccessibilityCpl.dll", "gameux.dll", "mmRes.dll", "NetCenter.dll", "networkexplorer.dll", "pnidui.dll", "SensorsCpl.dll",
        };

        public FormMain()
        {
            self = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Properties.Settings.Default.X, Properties.Settings.Default.Y);
            this.Height = Properties.Settings.Default.Heigth;
            this.Width = Properties.Settings.Default.Width;

            if (Properties.Settings.Default.Heigth == 0) this.Height = 400;
            if (Properties.Settings.Default.Width == 0) this.Width = 600;

            foreach (var dll in system_dlls)
            {
                string var = "%WINDIR%\\system32\\";
                string path = Environment.ExpandEnvironmentVariables(var + dll);
                if (!File.Exists(path)) continue;

                ToolStripMenuItem menu = new ToolStripMenuItem();
                menu.Text = var + dll;
                menu.Click += new EventHandler(delegate (object obj, EventArgs args) {
                    OpenChild(path);
                });
                tsMS_System.DropDownItems.Insert(0, menu);
            }

            ToolStrip_MenuColor.DropDownItems.Insert(0, createItemColor(SystemColors.Menu, "COLOR_MENU"));
            ToolStrip_MenuColor.DropDownItems.Insert(0, createItemColor(SystemColors.Window, "COLOR_WINDOW"));
            ToolStrip_MenuColor.DropDownItems.Insert(0, createItemColor(SystemColors.ButtonFace, "COLOR_BTNFACE"));

            PropertyInfo[] properties = typeof(SystemColors).GetProperties();
            foreach (PropertyInfo property in properties) {
                if (property.PropertyType != typeof(Color)) continue; // Если это не цвет, то пропускаем это свойство
                Color color = (Color)property.GetValue(null,null);
                toolStripItem_System.DropDownItems.Add(createItemColor(color, property.Name));
            }
        }

        protected ToolStripMenuItem createItemColor(Color color, String text, String name = null)
        {
            ToolStripMenuItem menu = new ToolStripMenuItem();
            menu.BackColor = color;
            menu.Text = text;
            menu.Click += new EventHandler(delegate(object obj, EventArgs args) {
                updateChildColor(color);
            });
            return menu;
        }

        private void toolStripItem_Palette_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            updateChildColor(dialog.Color);
        }

        protected void updateChildColor(Color color)
        {
            child_color = color;
            foreach (FormIcon form in childs)
            {
                form.updateColors(color);
            }
        }

        public void onChildClose(FormIcon child)
        {
            if (!childs.Contains(child)) return;
            childs.Remove(child);
        }

        protected void OpenChild(String filename)
        {
            if (Properties.Settings.Default.LatestFiles == null)
            {
                Properties.Settings.Default.LatestFiles = new System.Collections.Specialized.StringCollection();
            }
            var latest = Properties.Settings.Default.LatestFiles;
            if (latest.Count > MAX_LATEST_FILES) latest.RemoveAt(latest.Count - 1);
            if (latest.Contains(filename)) latest.Remove(filename);
            latest.Insert(0, filename);
            Properties.Settings.Default.Save();

            FormIcon icon = new FormIcon(this, filename);
            icon.MdiParent = this;
            icon.Show();
            icon.updateColors(child_color);
            childs.Add(icon);
        }

        private void ToolStripMenu_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Executables (DLL/EXE)|*.exe;*.dll";
            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.OK) return;
            OpenChild(dialog.FileName);
        }

        private void ToolStrip_MenuSelect_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Wat: " + Properties.Settings.Default.LatestFiles);
            Console.WriteLine("null? " + (Properties.Settings.Default.LatestFiles == null ? "yes" : "no"));
            if (Properties.Settings.Default.LatestFiles == null) return;

            var items = ToolStrip_MenuSelect.DropDownItems;

            for (int i=items.Count-1;i>=0;i--)
            {
                var item = items[i];
                if (item.Name.StartsWith("Temp")) items.RemoveAt(i);
            }

            foreach (var item in Properties.Settings.Default.LatestFiles)
            {
                ToolStripMenuItem menu = new ToolStripMenuItem();
                menu.Text = item;
                menu.Name = "Temp";
                menu.Click += new EventHandler(delegate (object obj, EventArgs args) {
                    OpenChild(item);
                });
                items.Insert(0, menu);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }

            Properties.Settings.Default.X = Location.X;
            Properties.Settings.Default.Y = Location.Y;           
            Properties.Settings.Default.Heigth = Height;
            Properties.Settings.Default.Width = Width;
            Properties.Settings.Default.Save();
        }
    }
}
