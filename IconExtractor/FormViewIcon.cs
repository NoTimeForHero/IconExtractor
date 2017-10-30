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

        public void showContextMenu(int icon_id)
        {
            this.icon_id = icon_id;
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            formIcon.SaveIconById(this, icon_id);
        }
    }
}
