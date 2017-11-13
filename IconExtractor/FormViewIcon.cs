using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IconExtractor
{
    public partial class FormViewIcon : Form
    {
        public int icon_id;
        private FormIcon formIcon;

        public FormViewIcon(FormIcon formIcon)
        {
            this.formIcon = formIcon;
            InitializeComponent();
        }

        private void FormViewIcon_Load(object sender, EventArgs e)
        {
            // Nothing to do here
        }

        public void showContextMenu(IconApi.GRPICONDIRENTRY icometa, string group)
        {
            this.icon_id = icometa.ID;
            toolStripMenuItem2.Text = String.Format("Icon {0}  (group {1})", icometa.ID, group);
            toolStripMenuItem3.Text = String.Format("Height: {0}, width: {1}",icometa.Width == 0 ? 256 : icometa.Width, icometa.Height == 0 ? 256 : icometa.Height);
            toolStripMenuItem4.Text = icometa.ColorCount == 0 ? "Colors: Unknown" : String.Format("Colors: {0} ({1} bits)", icometa.ColorCount, icometa.BitCount);
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            formIcon.SaveIconById(this, icon_id);
        }
    }
}
