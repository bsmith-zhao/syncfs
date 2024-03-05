namespace vfs.mgr
{
    partial class VfsManager
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.msgUI = new System.Windows.Forms.TextBox();
            this.propUI = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.listUI = new System.Windows.Forms.ListView();
            this.mountedHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sourceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.languageBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionBtn = new System.Windows.Forms.ToolStripButton();
            this.addAeadFSBtn = new System.Windows.Forms.ToolStripButton();
            this.mountBtn = new System.Windows.Forms.ToolStripButton();
            this.unmountBtn = new System.Windows.Forms.ToolStripButton();
            this.openDirBtn = new System.Windows.Forms.ToolStripButton();
            this.deleteBtn = new System.Windows.Forms.ToolStripButton();
            this.modifyPwdBtn = new System.Windows.Forms.ToolStripButton();
            this.refreshBtn = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.descSplit = new System.Windows.Forms.Splitter();
            this.descUI = new System.Windows.Forms.TextBox();
            this.toolbar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msgUI.Location = new System.Drawing.Point(0, 642);
            this.msgUI.Multiline = true;
            this.msgUI.Name = "msgUI";
            this.msgUI.Size = new System.Drawing.Size(1057, 179);
            this.msgUI.TabIndex = 0;
            // 
            // propUI
            // 
            this.propUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propUI.HelpVisible = false;
            this.propUI.Location = new System.Drawing.Point(0, 0);
            this.propUI.Name = "propUI";
            this.propUI.Size = new System.Drawing.Size(387, 562);
            this.propUI.TabIndex = 1;
            this.propUI.ToolbarVisible = false;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(1057, 70);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 751);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 632);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1057, 10);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // listUI
            // 
            this.listUI.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mountedHeader,
            this.mountHeader,
            this.typeHeader,
            this.sourceHeader});
            this.listUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUI.FullRowSelect = true;
            this.listUI.HideSelection = false;
            this.listUI.Location = new System.Drawing.Point(0, 70);
            this.listUI.Name = "listUI";
            this.listUI.ShowItemToolTips = true;
            this.listUI.Size = new System.Drawing.Size(1057, 562);
            this.listUI.TabIndex = 6;
            this.listUI.UseCompatibleStateImageBehavior = false;
            this.listUI.View = System.Windows.Forms.View.Details;
            // 
            // mountedHeader
            // 
            this.mountedHeader.Text = "Mounted";
            this.mountedHeader.Width = 280;
            // 
            // mountHeader
            // 
            this.mountHeader.Text = "Mount";
            this.mountHeader.Width = 277;
            // 
            // typeHeader
            // 
            this.typeHeader.Text = "Type";
            this.typeHeader.Width = 174;
            // 
            // sourceHeader
            // 
            this.sourceHeader.Text = "Source";
            this.sourceHeader.Width = 315;
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAeadFSBtn,
            this.mountBtn,
            this.unmountBtn,
            this.openDirBtn,
            this.deleteBtn,
            this.modifyPwdBtn,
            this.refreshBtn,
            this.optionBtn,
            this.languageBtn});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(1454, 70);
            this.toolbar.TabIndex = 7;
            this.toolbar.Text = "toolStrip1";
            // 
            // languageBtn
            // 
            this.languageBtn.Image = global::vfs.mgr.Properties.Resources.Language;
            this.languageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.languageBtn.Name = "languageBtn";
            this.languageBtn.Size = new System.Drawing.Size(148, 67);
            this.languageBtn.Text = "Language";
            this.languageBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // optionBtn
            // 
            this.optionBtn.Image = global::vfs.mgr.Properties.Resources.Option;
            this.optionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optionBtn.Name = "optionBtn";
            this.optionBtn.Size = new System.Drawing.Size(98, 67);
            this.optionBtn.Text = "Option";
            this.optionBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.optionBtn.Click += new System.EventHandler(this.optionBtn_Click);
            // 
            // addAeadFSBtn
            // 
            this.addAeadFSBtn.Image = global::vfs.mgr.Properties.Resources.AddAeadFS;
            this.addAeadFSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addAeadFSBtn.Name = "addAeadFSBtn";
            this.addAeadFSBtn.Size = new System.Drawing.Size(151, 67);
            this.addAeadFSBtn.Text = "AddAeadFS";
            this.addAeadFSBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addAeadFSBtn.Click += new System.EventHandler(this.addAeadFSBtn_Click);
            // 
            // mountBtn
            // 
            this.mountBtn.Image = global::vfs.mgr.Properties.Resources.Mount;
            this.mountBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mountBtn.Name = "mountBtn";
            this.mountBtn.Size = new System.Drawing.Size(95, 67);
            this.mountBtn.Text = "Mount";
            this.mountBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.mountBtn.Click += new System.EventHandler(this.mountBtn_Click);
            // 
            // unmountBtn
            // 
            this.unmountBtn.Image = global::vfs.mgr.Properties.Resources.Unmount;
            this.unmountBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.unmountBtn.Name = "unmountBtn";
            this.unmountBtn.Size = new System.Drawing.Size(127, 67);
            this.unmountBtn.Text = "Unmount";
            this.unmountBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.unmountBtn.Click += new System.EventHandler(this.unmountBtn_Click);
            // 
            // openDirBtn
            // 
            this.openDirBtn.Image = global::vfs.mgr.Properties.Resources.OpenDir;
            this.openDirBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openDirBtn.Name = "openDirBtn";
            this.openDirBtn.Size = new System.Drawing.Size(115, 67);
            this.openDirBtn.Text = "OpenDir";
            this.openDirBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.openDirBtn.Click += new System.EventHandler(this.openDirBtn_Click);
            // 
            // deleteBtn
            // 
            this.deleteBtn.Image = global::vfs.mgr.Properties.Resources.Delete;
            this.deleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(93, 67);
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // modifyPwdBtn
            // 
            this.modifyPwdBtn.Image = global::vfs.mgr.Properties.Resources.SetPwd;
            this.modifyPwdBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modifyPwdBtn.Name = "modifyPwdBtn";
            this.modifyPwdBtn.Size = new System.Drawing.Size(147, 67);
            this.modifyPwdBtn.Text = "ModifyPwd";
            this.modifyPwdBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.modifyPwdBtn.Click += new System.EventHandler(this.modifyPwdBtn_Click);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Image = global::vfs.mgr.Properties.Resources.Refresh;
            this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(105, 67);
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propUI);
            this.panel1.Controls.Add(this.descSplit);
            this.panel1.Controls.Add(this.descUI);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1067, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 751);
            this.panel1.TabIndex = 9;
            // 
            // descSplit
            // 
            this.descSplit.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.descSplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descSplit.Location = new System.Drawing.Point(0, 562);
            this.descSplit.Name = "descSplit";
            this.descSplit.Size = new System.Drawing.Size(387, 10);
            this.descSplit.TabIndex = 4;
            this.descSplit.TabStop = false;
            // 
            // descUI
            // 
            this.descUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.descUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descUI.Location = new System.Drawing.Point(0, 572);
            this.descUI.Multiline = true;
            this.descUI.Name = "descUI";
            this.descUI.ReadOnly = true;
            this.descUI.Size = new System.Drawing.Size(387, 179);
            this.descUI.TabIndex = 2;
            // 
            // VfsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 821);
            this.Controls.Add(this.listUI);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.msgUI);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolbar);
            this.Name = "VfsManager";
            this.Text = "VfsManager";
            this.Load += new System.EventHandler(this.VfsManager_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.PropertyGrid propUI;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ListView listUI;
        private System.Windows.Forms.ToolStrip toolbar;
        public System.Windows.Forms.ToolStripButton addAeadFSBtn;
        public System.Windows.Forms.ToolStripButton mountBtn;
        public System.Windows.Forms.ToolStripButton deleteBtn;
        public System.Windows.Forms.ToolStripButton unmountBtn;
        public System.Windows.Forms.ToolStripButton modifyPwdBtn;
        public System.Windows.Forms.ToolStripButton optionBtn;
        public System.Windows.Forms.ToolStripButton refreshBtn;
        public System.Windows.Forms.ToolStripButton openDirBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter descSplit;
        private System.Windows.Forms.TextBox descUI;
        public System.Windows.Forms.ColumnHeader typeHeader;
        public System.Windows.Forms.ColumnHeader sourceHeader;
        public System.Windows.Forms.ColumnHeader mountedHeader;
        public System.Windows.Forms.ColumnHeader mountHeader;
        private System.Windows.Forms.ToolStripDropDownButton languageBtn;
    }
}

