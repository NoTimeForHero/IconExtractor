namespace IconExtractor
{
    partial class FormIcon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageInfo = new System.Windows.Forms.TabPage();
            this.tSM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tSM_Info = new System.Windows.Forms.ToolStripMenuItem();
            this.tSM_Line1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSM_Line2 = new System.Windows.Forms.ToolStripSeparator();
            this.tSM_SaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tSM_SaveOneFile = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageInfo.SuspendLayout();
            this.tSM.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(538, 369);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageInfo
            // 
            this.tabPageInfo.AutoScroll = true;
            this.tabPageInfo.Controls.Add(this.label1);
            this.tabPageInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageInfo.Name = "tabPageInfo";
            this.tabPageInfo.Size = new System.Drawing.Size(530, 343);
            this.tabPageInfo.TabIndex = 0;
            this.tabPageInfo.Text = "INFO";
            this.tabPageInfo.UseVisualStyleBackColor = true;
            // 
            // tSM
            // 
            this.tSM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSM_Info,
            this.tSM_Line1,
            this.tSM_Line2,
            this.tSM_SaveAll,
            this.tSM_SaveOneFile});
            this.tSM.Name = "contextMenuStrip1";
            this.tSM.Size = new System.Drawing.Size(201, 82);
            // 
            // tSM_Info
            // 
            this.tSM_Info.Enabled = false;
            this.tSM_Info.Name = "tSM_Info";
            this.tSM_Info.Size = new System.Drawing.Size(200, 22);
            this.tSM_Info.Text = "Group: 101      (icon 5)";
            // 
            // tSM_Line1
            // 
            this.tSM_Line1.Name = "tSM_Line1";
            this.tSM_Line1.Size = new System.Drawing.Size(197, 6);
            // 
            // tSM_Line2
            // 
            this.tSM_Line2.Name = "tSM_Line2";
            this.tSM_Line2.Size = new System.Drawing.Size(197, 6);
            // 
            // tSM_SaveAll
            // 
            this.tSM_SaveAll.Name = "tSM_SaveAll";
            this.tSM_SaveAll.Size = new System.Drawing.Size(200, 22);
            this.tSM_SaveAll.Text = "Save all size icons";
            // 
            // tSM_SaveOneFile
            // 
            this.tSM_SaveOneFile.Name = "tSM_SaveOneFile";
            this.tSM_SaveOneFile.Size = new System.Drawing.Size(200, 22);
            this.tSM_SaveOneFile.Text = "Save all icons to one file";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(474, 288);
            this.label1.TabIndex = 0;
            // 
            // FormIcon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(538, 369);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormIcon";
            this.ShowIcon = false;
            this.Text = "FormIcon";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormIcon_FormClosed);
            this.Load += new System.EventHandler(this.FormIcon_Load);
            this.Shown += new System.EventHandler(this.FormIcon_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPageInfo.ResumeLayout(false);
            this.tSM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageInfo;
        private System.Windows.Forms.ContextMenuStrip tSM;
        private System.Windows.Forms.ToolStripMenuItem tSM_Info;
        private System.Windows.Forms.ToolStripSeparator tSM_Line1;
        private System.Windows.Forms.ToolStripSeparator tSM_Line2;
        private System.Windows.Forms.ToolStripMenuItem tSM_SaveAll;
        private System.Windows.Forms.ToolStripMenuItem tSM_SaveOneFile;
        private System.Windows.Forms.Label label1;
    }
}