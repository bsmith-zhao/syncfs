namespace sync.ui
{
    partial class BatchRunner
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
            this.msgUI = new System.Windows.Forms.TextBox();
            this.listUI = new System.Windows.Forms.ListView();
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actionUI = new System.Windows.Forms.TextBox();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.runBtn = new System.Windows.Forms.ToolStripButton();
            this.stopBtn = new System.Windows.Forms.ToolStripButton();
            this.openDirBtn = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.actPanel = new System.Windows.Forms.Panel();
            this.statusUI = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.toolbar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.actPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgUI.Location = new System.Drawing.Point(0, 0);
            this.msgUI.MaxLength = 256;
            this.msgUI.Multiline = true;
            this.msgUI.Name = "msgUI";
            this.msgUI.Size = new System.Drawing.Size(815, 331);
            this.msgUI.TabIndex = 109;
            // 
            // listUI
            // 
            this.listUI.CheckBoxes = true;
            this.listUI.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.statusHeader});
            this.listUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUI.FullRowSelect = true;
            this.listUI.HideSelection = false;
            this.listUI.Location = new System.Drawing.Point(0, 44);
            this.listUI.MultiSelect = false;
            this.listUI.Name = "listUI";
            this.listUI.ShowItemToolTips = true;
            this.listUI.Size = new System.Drawing.Size(815, 343);
            this.listUI.TabIndex = 111;
            this.listUI.UseCompatibleStateImageBehavior = false;
            this.listUI.View = System.Windows.Forms.View.Details;
            this.listUI.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listUI_ItemCheck);
            this.listUI.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listUI_MouseDoubleClick);
            this.listUI.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listUI_MouseDown);
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            this.nameHeader.Width = 717;
            // 
            // statusHeader
            // 
            this.statusHeader.Text = "Status";
            this.statusHeader.Width = 211;
            // 
            // actionUI
            // 
            this.actionUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.actionUI.Dock = System.Windows.Forms.DockStyle.Left;
            this.actionUI.Location = new System.Drawing.Point(0, 0);
            this.actionUI.Name = "actionUI";
            this.actionUI.ReadOnly = true;
            this.actionUI.Size = new System.Drawing.Size(331, 28);
            this.actionUI.TabIndex = 0;
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runBtn,
            this.stopBtn,
            this.openDirBtn});
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Padding = new System.Windows.Forms.Padding(0, 0, 1, 5);
            this.toolbar.Size = new System.Drawing.Size(815, 44);
            this.toolbar.TabIndex = 118;
            this.toolbar.Text = "toolbar";
            // 
            // runBtn
            // 
            this.runBtn.Image = global::sync.mgr.Properties.Resources.RunColor;
            this.runBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.runBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(92, 35);
            this.runBtn.Text = "Run";
            this.runBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Image = global::sync.mgr.Properties.Resources.StopColor;
            this.stopBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stopBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(95, 35);
            this.stopBtn.Text = "Stop";
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // openDirBtn
            // 
            this.openDirBtn.Image = global::sync.mgr.Properties.Resources.OpenDirColor;
            this.openDirBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openDirBtn.Name = "openDirBtn";
            this.openDirBtn.Size = new System.Drawing.Size(147, 36);
            this.openDirBtn.Text = "OpenDir";
            this.openDirBtn.Click += new System.EventHandler(this.openDirBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.msgUI);
            this.panel1.Controls.Add(this.actPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(815, 361);
            this.panel1.TabIndex = 119;
            // 
            // actPanel
            // 
            this.actPanel.Controls.Add(this.statusUI);
            this.actPanel.Controls.Add(this.splitter1);
            this.actPanel.Controls.Add(this.actionUI);
            this.actPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actPanel.Location = new System.Drawing.Point(0, 331);
            this.actPanel.Name = "actPanel";
            this.actPanel.Size = new System.Drawing.Size(815, 30);
            this.actPanel.TabIndex = 110;
            // 
            // statusUI
            // 
            this.statusUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusUI.Location = new System.Drawing.Point(339, 0);
            this.statusUI.Name = "statusUI";
            this.statusUI.ReadOnly = true;
            this.statusUI.Size = new System.Drawing.Size(476, 28);
            this.statusUI.TabIndex = 122;
            this.statusUI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(331, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 30);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listUI);
            this.panel3.Controls.Add(this.toolbar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 366);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(815, 387);
            this.panel3.TabIndex = 120;
            // 
            // splitter2
            // 
            this.splitter2.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 361);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(815, 5);
            this.splitter2.TabIndex = 121;
            this.splitter2.TabStop = false;
            // 
            // BatchRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 753);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel3);
            this.Name = "BatchRunner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "true";
            this.Text = "BatchRunner";
            this.Load += new System.EventHandler(this.BatchRunner_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.actPanel.ResumeLayout(false);
            this.actPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.ListView listUI;
        private System.Windows.Forms.TextBox actionUI;
        private System.Windows.Forms.ToolStrip toolbar;
        public System.Windows.Forms.ToolStripButton runBtn;
        public System.Windows.Forms.ToolStripButton stopBtn;
        public System.Windows.Forms.ColumnHeader nameHeader;
        public System.Windows.Forms.ColumnHeader statusHeader;
        public System.Windows.Forms.ToolStripButton openDirBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TextBox statusUI;
        private System.Windows.Forms.Panel actPanel;
        private System.Windows.Forms.Splitter splitter1;
    }
}