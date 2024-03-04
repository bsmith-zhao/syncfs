namespace sync.ui
{
    partial class ViewPanel
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
            this.treeUI = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.listUI = new System.Windows.Forms.ListView();
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bytesHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.msgUI = new System.Windows.Forms.TextBox();
            this.msgSplit = new System.Windows.Forms.Splitter();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.refreshBtn = new System.Windows.Forms.ToolStripButton();
            this.createDirBtn = new System.Windows.Forms.ToolStripButton();
            this.importBtn = new System.Windows.Forms.ToolStripButton();
            this.exportBtn = new System.Windows.Forms.ToolStripButton();
            this.editLabelBtn = new System.Windows.Forms.ToolStripButton();
            this.deleteBtn = new System.Windows.Forms.ToolStripButton();
            this.manageBtn = new System.Windows.Forms.ToolStripButton();
            this.upLevelBtn = new System.Windows.Forms.ToolStripButton();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeUI
            // 
            this.treeUI.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeUI.Location = new System.Drawing.Point(0, 80);
            this.treeUI.Name = "treeUI";
            this.treeUI.Size = new System.Drawing.Size(340, 697);
            this.treeUI.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(340, 80);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 697);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // listUI
            // 
            this.listUI.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.sizeHeader,
            this.bytesHeader,
            this.timeHeader});
            this.listUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUI.HideSelection = false;
            this.listUI.Location = new System.Drawing.Point(350, 80);
            this.listUI.Name = "listUI";
            this.listUI.Size = new System.Drawing.Size(1026, 515);
            this.listUI.TabIndex = 2;
            this.listUI.UseCompatibleStateImageBehavior = false;
            this.listUI.View = System.Windows.Forms.View.Details;
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            this.nameHeader.Width = 456;
            // 
            // sizeHeader
            // 
            this.sizeHeader.Text = "Size";
            this.sizeHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sizeHeader.Width = 126;
            // 
            // bytesHeader
            // 
            this.bytesHeader.Text = "Bytes";
            this.bytesHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bytesHeader.Width = 183;
            // 
            // timeHeader
            // 
            this.timeHeader.Text = "Time";
            this.timeHeader.Width = 250;
            // 
            // msgUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msgUI.Location = new System.Drawing.Point(350, 605);
            this.msgUI.Multiline = true;
            this.msgUI.Name = "msgUI";
            this.msgUI.Size = new System.Drawing.Size(1026, 172);
            this.msgUI.TabIndex = 3;
            this.msgUI.Visible = false;
            // 
            // msgSplit
            // 
            this.msgSplit.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.msgSplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msgSplit.Location = new System.Drawing.Point(350, 595);
            this.msgSplit.Name = "msgSplit";
            this.msgSplit.Size = new System.Drawing.Size(1026, 10);
            this.msgSplit.TabIndex = 4;
            this.msgSplit.TabStop = false;
            this.msgSplit.Visible = false;
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshBtn,
            this.createDirBtn,
            this.importBtn,
            this.exportBtn,
            this.editLabelBtn,
            this.deleteBtn,
            this.manageBtn,
            this.upLevelBtn});
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Padding = new System.Windows.Forms.Padding(5);
            this.toolbar.Size = new System.Drawing.Size(1376, 80);
            this.toolbar.TabIndex = 5;
            this.toolbar.Text = "toolStrip1";
            // 
            // refreshBtn
            // 
            this.refreshBtn.Image = global::sync.mgr.Properties.Resources.Refresh;
            this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(105, 67);
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // createDirBtn
            // 
            this.createDirBtn.Image = global::sync.mgr.Properties.Resources.AddNormalDir;
            this.createDirBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createDirBtn.Name = "createDirBtn";
            this.createDirBtn.Size = new System.Drawing.Size(126, 67);
            this.createDirBtn.Text = "CreateDir";
            this.createDirBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.createDirBtn.Click += new System.EventHandler(this.createDirBtn_Click);
            // 
            // importBtn
            // 
            this.importBtn.Image = global::sync.mgr.Properties.Resources.ImportFile;
            this.importBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(95, 67);
            this.importBtn.Text = "Import";
            this.importBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // exportBtn
            // 
            this.exportBtn.Image = global::sync.mgr.Properties.Resources.ExportFile;
            this.exportBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(91, 67);
            this.exportBtn.Text = "Export";
            this.exportBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // editLabelBtn
            // 
            this.editLabelBtn.Image = global::sync.mgr.Properties.Resources.EditLabel;
            this.editLabelBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editLabelBtn.Name = "editLabelBtn";
            this.editLabelBtn.Size = new System.Drawing.Size(121, 67);
            this.editLabelBtn.Text = "EditLabel";
            this.editLabelBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.editLabelBtn.Click += new System.EventHandler(this.editLabelBtn_Click);
            // 
            // deleteBtn
            // 
            this.deleteBtn.Image = global::sync.mgr.Properties.Resources.Delete;
            this.deleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(93, 67);
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // manageBtn
            // 
            this.manageBtn.CheckOnClick = true;
            this.manageBtn.Image = global::sync.mgr.Properties.Resources.ManageFiles;
            this.manageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.manageBtn.Name = "manageBtn";
            this.manageBtn.Size = new System.Drawing.Size(111, 67);
            this.manageBtn.Text = "Manage";
            this.manageBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // upLevelBtn
            // 
            this.upLevelBtn.Image = global::sync.mgr.Properties.Resources.UpLevel;
            this.upLevelBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.upLevelBtn.Name = "upLevelBtn";
            this.upLevelBtn.Size = new System.Drawing.Size(110, 67);
            this.upLevelBtn.Text = "UpLevel";
            this.upLevelBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.upLevelBtn.Click += new System.EventHandler(this.upLevelBtn_Click);
            // 
            // ViewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listUI);
            this.Controls.Add(this.msgSplit);
            this.Controls.Add(this.msgUI);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.treeUI);
            this.Controls.Add(this.toolbar);
            this.Name = "ViewPanel";
            this.Size = new System.Drawing.Size(1376, 777);
            this.Load += new System.EventHandler(this.SyncRepView_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeUI;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.Splitter msgSplit;
        private System.Windows.Forms.ToolStrip toolbar;
        public System.Windows.Forms.ToolStripButton refreshBtn;
        public System.Windows.Forms.ToolStripButton upLevelBtn;
        public System.Windows.Forms.ToolStripButton editLabelBtn;
        public System.Windows.Forms.ToolStripButton createDirBtn;
        public System.Windows.Forms.ToolStripButton deleteBtn;
        public System.Windows.Forms.ToolStripButton exportBtn;
        public System.Windows.Forms.ToolStripButton importBtn;
        public System.Windows.Forms.ListView listUI;
        public System.Windows.Forms.ToolStripButton manageBtn;
        public System.Windows.Forms.ColumnHeader nameHeader;
        public System.Windows.Forms.ColumnHeader sizeHeader;
        public System.Windows.Forms.ColumnHeader bytesHeader;
        public System.Windows.Forms.ColumnHeader timeHeader;
    }
}