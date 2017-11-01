namespace IconExtractor
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStrip_MenuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMS_Line1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMS_System = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMS_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_MenuColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripItem_System = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripItem_Palette = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_MenuSize = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_About = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStrip_MenuSelect,
            this.ToolStrip_MenuColor,
            this.ToolStrip_MenuSize,
            this.ToolStrip_About});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(786, 48);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStrip_MenuSelect
            // 
            this.ToolStrip_MenuSelect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMS_Line1,
            this.tsMS_System,
            this.tsMS_OpenFile});
            this.ToolStrip_MenuSelect.Image = global::IconExtractor.Properties.Resources.iEye48x1;
            this.ToolStrip_MenuSelect.Name = "ToolStrip_MenuSelect";
            this.ToolStrip_MenuSelect.Size = new System.Drawing.Size(88, 44);
            this.ToolStrip_MenuSelect.Text = "Load";
            this.ToolStrip_MenuSelect.ToolTipText = "Choose EXE/ICO container to view icons";
            this.ToolStrip_MenuSelect.Click += new System.EventHandler(this.ToolStrip_MenuSelect_Click);
            // 
            // tsMS_Line1
            // 
            this.tsMS_Line1.Name = "tsMS_Line1";
            this.tsMS_Line1.Size = new System.Drawing.Size(217, 6);
            // 
            // tsMS_System
            // 
            this.tsMS_System.Name = "tsMS_System";
            this.tsMS_System.Size = new System.Drawing.Size(220, 26);
            this.tsMS_System.Text = "System executables";
            // 
            // tsMS_OpenFile
            // 
            this.tsMS_OpenFile.Name = "tsMS_OpenFile";
            this.tsMS_OpenFile.Size = new System.Drawing.Size(220, 26);
            this.tsMS_OpenFile.Text = "Open executable file";
            this.tsMS_OpenFile.Click += new System.EventHandler(this.ToolStripMenu_OpenFile_Click);
            // 
            // ToolStrip_MenuColor
            // 
            this.ToolStrip_MenuColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripItem_System,
            this.toolStripItem_Palette});
            this.ToolStrip_MenuColor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ToolStrip_MenuColor.Image = global::IconExtractor.Properties.Resources.iColor48x1;
            this.ToolStrip_MenuColor.Name = "ToolStrip_MenuColor";
            this.ToolStrip_MenuColor.Size = new System.Drawing.Size(116, 44);
            this.ToolStrip_MenuColor.Text = "BG Color";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // toolStripItem_System
            // 
            this.toolStripItem_System.Name = "toolStripItem_System";
            this.toolStripItem_System.Size = new System.Drawing.Size(177, 26);
            this.toolStripItem_System.Text = "System colors";
            // 
            // toolStripItem_Palette
            // 
            this.toolStripItem_Palette.Name = "toolStripItem_Palette";
            this.toolStripItem_Palette.Size = new System.Drawing.Size(177, 26);
            this.toolStripItem_Palette.Text = "Custom color";
            this.toolStripItem_Palette.Click += new System.EventHandler(this.toolStripItem_Palette_Click);
            // 
            // ToolStrip_MenuSize
            // 
            this.ToolStrip_MenuSize.Image = global::IconExtractor.Properties.Resources.iSize;
            this.ToolStrip_MenuSize.Name = "ToolStrip_MenuSize";
            this.ToolStrip_MenuSize.Size = new System.Drawing.Size(112, 44);
            this.ToolStrip_MenuSize.Text = "Close all";
            this.ToolStrip_MenuSize.ToolTipText = "Change Icon Size";
            this.ToolStrip_MenuSize.Click += new System.EventHandler(this.ToolStrip_MenuSize_Click);
            // 
            // ToolStrip_About
            // 
            this.ToolStrip_About.Image = global::IconExtractor.Properties.Resources.main;
            this.ToolStrip_About.Name = "ToolStrip_About";
            this.ToolStrip_About.Size = new System.Drawing.Size(96, 44);
            this.ToolStrip_About.Text = "About";
            this.ToolStrip_About.Click += new System.EventHandler(this.ToolStrip_About_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 504);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "IconExtractor - Tool to view and extract embedded icons";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_MenuColor;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_MenuSize;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_MenuSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripItem_Palette;
        private System.Windows.Forms.ToolStripMenuItem toolStripItem_System;
        private System.Windows.Forms.ToolStripSeparator tsMS_Line1;
        private System.Windows.Forms.ToolStripMenuItem tsMS_OpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsMS_System;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_About;
    }
}

