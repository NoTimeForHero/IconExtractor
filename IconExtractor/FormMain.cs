using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        protected static FormMain self;
        protected List<FormIcon> childs = new List<FormIcon>();
        protected Color child_color = SystemColors.Control;
        protected string[] system_dlls =
        {
            "shell32.dll", "compstui.dll", "ieframe.dll", "mmcndmgr.dll", "moricons.dll", "netshell.dll", "pifmgr.dll", "setupapi.dll", "wmploc.dll", "wpdshext.dll",
            "imageres.dll", "ddores.dll", "AccessibilityCpl.dll", "gameux.dll", "mmRes.dll", "NetCenter.dll", "networkexplorer.dll", "pnidui.dll", "SensorsCpl.dll",
        };

        public FormMain()
        {
            self = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {        
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

        private void ToolStrip_MenuSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Executables (DLL/EXE)|*.exe;*.dll";
            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.OK) return;
            FormIcon icon = new FormIcon(this,dialog.FileName);
            icon.MdiParent = this;
            icon.Show();
            icon.updateColors(child_color);
            childs.Add(icon);
        }
    }
}
