
namespace sync.ui
{
    partial class SyncManager
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
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.treeUI = new System.Windows.Forms.TreeView();
            this.propUI = new System.Windows.Forms.PropertyGrid();
            this.msgUI = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.descSplit = new System.Windows.Forms.Splitter();
            this.descUI = new System.Windows.Forms.TextBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.linkUI = new link.LinkView();
            this.languageBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionBtn = new System.Windows.Forms.ToolStripButton();
            this.upBtn = new System.Windows.Forms.ToolStripButton();
            this.downBtn = new System.Windows.Forms.ToolStripButton();
            this.addSpaceBtn = new System.Windows.Forms.ToolStripButton();
            this.addNormalDirBtn = new System.Windows.Forms.ToolStripButton();
            this.addAeadFSBtn = new System.Windows.Forms.ToolStripButton();
            this.createViewBtn = new System.Windows.Forms.ToolStripButton();
            this.linkViewBtn = new System.Windows.Forms.ToolStripButton();
            this.masterSyncBtn = new System.Windows.Forms.ToolStripButton();
            this.roundSyncBtn = new System.Windows.Forms.ToolStripButton();
            this.editLabelBtn = new System.Windows.Forms.ToolStripButton();
            this.deleteBtn = new System.Windows.Forms.ToolStripButton();
            this.browseFilesBtn = new System.Windows.Forms.ToolStripButton();
            this.manageFilesBtn = new System.Windows.Forms.ToolStripButton();
            this.copyArgsBtn = new System.Windows.Forms.ToolStripButton();
            this.pasteArgsBtn = new System.Windows.Forms.ToolStripButton();
            this.spreadArgsBtn = new System.Windows.Forms.ToolStripButton();
            this.modifyPwdBtn = new System.Windows.Forms.ToolStripButton();
            this.mountBtn = new System.Windows.Forms.ToolStripButton();
            this.unmountBtn = new System.Windows.Forms.ToolStripButton();
            this.openDirBtn = new System.Windows.Forms.ToolStripButton();
            this.runBtn = new System.Windows.Forms.ToolStripButton();
            this.batchRunBtn = new System.Windows.Forms.ToolStripButton();
            this.toolbar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageBtn,
            this.optionBtn,
            this.upBtn,
            this.downBtn,
            this.addSpaceBtn,
            this.addNormalDirBtn,
            this.addAeadFSBtn,
            this.createViewBtn,
            this.linkViewBtn,
            this.masterSyncBtn,
            this.roundSyncBtn,
            this.editLabelBtn,
            this.deleteBtn,
            this.browseFilesBtn,
            this.manageFilesBtn,
            this.copyArgsBtn,
            this.pasteArgsBtn,
            this.spreadArgsBtn,
            this.modifyPwdBtn,
            this.mountBtn,
            this.unmountBtn,
            this.openDirBtn,
            this.runBtn,
            this.batchRunBtn});
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.toolbar.Size = new System.Drawing.Size(1611, 152);
            this.toolbar.TabIndex = 1;
            // 
            // treeUI
            // 
            this.treeUI.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeUI.Location = new System.Drawing.Point(0, 152);
            this.treeUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeUI.Name = "treeUI";
            this.treeUI.Size = new System.Drawing.Size(377, 663);
            this.treeUI.TabIndex = 2;
            // 
            // propUI
            // 
            this.propUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propUI.HelpVisible = false;
            this.propUI.Location = new System.Drawing.Point(0, 0);
            this.propUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.propUI.Name = "propUI";
            this.propUI.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propUI.Size = new System.Drawing.Size(376, 496);
            this.propUI.TabIndex = 4;
            this.propUI.ToolbarVisible = false;
            // 
            // msgUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msgUI.Location = new System.Drawing.Point(387, 658);
            this.msgUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.msgUI.Multiline = true;
            this.msgUI.Name = "msgUI";
            this.msgUI.Size = new System.Drawing.Size(838, 157);
            this.msgUI.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(377, 152);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 663);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propUI);
            this.panel1.Controls.Add(this.descSplit);
            this.panel1.Controls.Add(this.descUI);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1235, 152);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 663);
            this.panel1.TabIndex = 9;
            // 
            // descSplit
            // 
            this.descSplit.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.descSplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descSplit.Location = new System.Drawing.Point(0, 496);
            this.descSplit.Margin = new System.Windows.Forms.Padding(2);
            this.descSplit.Name = "descSplit";
            this.descSplit.Size = new System.Drawing.Size(376, 10);
            this.descSplit.TabIndex = 1;
            this.descSplit.TabStop = false;
            // 
            // descUI
            // 
            this.descUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descUI.Location = new System.Drawing.Point(0, 506);
            this.descUI.Margin = new System.Windows.Forms.Padding(2);
            this.descUI.Multiline = true;
            this.descUI.Name = "descUI";
            this.descUI.ReadOnly = true;
            this.descUI.Size = new System.Drawing.Size(376, 157);
            this.descUI.TabIndex = 0;
            // 
            // splitter3
            // 
            this.splitter3.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter3.Location = new System.Drawing.Point(1225, 152);
            this.splitter3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(10, 663);
            this.splitter3.TabIndex = 10;
            this.splitter3.TabStop = false;
            // 
            // splitter4
            // 
            this.splitter4.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(387, 648);
            this.splitter4.Margin = new System.Windows.Forms.Padding(2);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(838, 10);
            this.splitter4.TabIndex = 11;
            this.splitter4.TabStop = false;
            // 
            // linkUI
            // 
            this.linkUI.AutoScroll = true;
            this.linkUI.BackColor = System.Drawing.SystemColors.Window;
            this.linkUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkUI.Graph = null;
            this.linkUI.Location = new System.Drawing.Point(387, 152);
            this.linkUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.linkUI.Name = "linkUI";
            this.linkUI.Size = new System.Drawing.Size(838, 496);
            this.linkUI.TabIndex = 3;
            // 
            // languageBtn
            // 
            this.languageBtn.Image = global::sync.mgr.Properties.Resources.Language;
            this.languageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.languageBtn.Name = "languageBtn";
            this.languageBtn.Size = new System.Drawing.Size(148, 67);
            this.languageBtn.Text = "Language";
            this.languageBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // optionBtn
            // 
            this.optionBtn.Image = global::sync.mgr.Properties.Resources.Option;
            this.optionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optionBtn.Name = "optionBtn";
            this.optionBtn.Size = new System.Drawing.Size(98, 67);
            this.optionBtn.Text = "Option";
            this.optionBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.optionBtn.Click += new System.EventHandler(this.optionBtn_Click);
            // 
            // upBtn
            // 
            this.upBtn.Image = global::sync.mgr.Properties.Resources.Up;
            this.upBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(51, 67);
            this.upBtn.Text = "Up";
            this.upBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.upBtn.Click += new System.EventHandler(this.moveUpBtn_Click);
            // 
            // downBtn
            // 
            this.downBtn.Image = global::sync.mgr.Properties.Resources.Down;
            this.downBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(85, 67);
            this.downBtn.Text = "Down";
            this.downBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.downBtn.Click += new System.EventHandler(this.moveDownBtn_Click);
            // 
            // addSpaceBtn
            // 
            this.addSpaceBtn.Image = global::sync.mgr.Properties.Resources.AddSpace;
            this.addSpaceBtn.Name = "addSpaceBtn";
            this.addSpaceBtn.Size = new System.Drawing.Size(133, 67);
            this.addSpaceBtn.Text = "AddSpace";
            this.addSpaceBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addSpaceBtn.Click += new System.EventHandler(this.addSpaceBtn_Click);
            // 
            // addNormalDirBtn
            // 
            this.addNormalDirBtn.Image = global::sync.mgr.Properties.Resources.AddNormalDir;
            this.addNormalDirBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addNormalDirBtn.Name = "addNormalDirBtn";
            this.addNormalDirBtn.Size = new System.Drawing.Size(183, 67);
            this.addNormalDirBtn.Text = "AddNormalDir";
            this.addNormalDirBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addNormalDirBtn.Click += new System.EventHandler(this.addDirRepBtn_Click);
            // 
            // addAeadFSBtn
            // 
            this.addAeadFSBtn.Image = global::sync.mgr.Properties.Resources.AddAeadFS;
            this.addAeadFSBtn.Name = "addAeadFSBtn";
            this.addAeadFSBtn.Size = new System.Drawing.Size(151, 67);
            this.addAeadFSBtn.Text = "AddAeadFS";
            this.addAeadFSBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addAeadFSBtn.Click += new System.EventHandler(this.addAeadRepBtn_Click);
            // 
            // createViewBtn
            // 
            this.createViewBtn.Image = global::sync.mgr.Properties.Resources.CreateView;
            this.createViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createViewBtn.Name = "createViewBtn";
            this.createViewBtn.Size = new System.Drawing.Size(148, 67);
            this.createViewBtn.Text = "CreateView";
            this.createViewBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.createViewBtn.Click += new System.EventHandler(this.addViewBtn_Click);
            // 
            // linkViewBtn
            // 
            this.linkViewBtn.Image = global::sync.mgr.Properties.Resources.LinkView;
            this.linkViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.linkViewBtn.Name = "linkViewBtn";
            this.linkViewBtn.Size = new System.Drawing.Size(119, 67);
            this.linkViewBtn.Text = "LinkView";
            this.linkViewBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.linkViewBtn.Click += new System.EventHandler(this.linkViewBtn_Click);
            // 
            // masterSyncBtn
            // 
            this.masterSyncBtn.Image = global::sync.mgr.Properties.Resources.MasterSync;
            this.masterSyncBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.masterSyncBtn.Name = "masterSyncBtn";
            this.masterSyncBtn.Size = new System.Drawing.Size(151, 67);
            this.masterSyncBtn.Text = "MasterSync";
            this.masterSyncBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.masterSyncBtn.Click += new System.EventHandler(this.addMasterBtn_Click);
            // 
            // roundSyncBtn
            // 
            this.roundSyncBtn.Image = global::sync.mgr.Properties.Resources.RoundSync;
            this.roundSyncBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.roundSyncBtn.Name = "roundSyncBtn";
            this.roundSyncBtn.Size = new System.Drawing.Size(148, 67);
            this.roundSyncBtn.Text = "RoundSync";
            this.roundSyncBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.roundSyncBtn.Click += new System.EventHandler(this.addInterBtn_Click);
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
            // browseFilesBtn
            // 
            this.browseFilesBtn.Image = global::sync.mgr.Properties.Resources.BrowseFiles;
            this.browseFilesBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.browseFilesBtn.Name = "browseFilesBtn";
            this.browseFilesBtn.Size = new System.Drawing.Size(151, 67);
            this.browseFilesBtn.Text = "BrowseFiles";
            this.browseFilesBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.browseFilesBtn.Click += new System.EventHandler(this.viewRepBtn_Click);
            // 
            // manageFilesBtn
            // 
            this.manageFilesBtn.Image = global::sync.mgr.Properties.Resources.ManageFiles;
            this.manageFilesBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.manageFilesBtn.Name = "manageFilesBtn";
            this.manageFilesBtn.Size = new System.Drawing.Size(161, 67);
            this.manageFilesBtn.Text = "ManageFiles";
            this.manageFilesBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.manageFilesBtn.Click += new System.EventHandler(this.manageRepBtn_Click);
            // 
            // copyArgsBtn
            // 
            this.copyArgsBtn.Image = global::sync.mgr.Properties.Resources.CopyArgs;
            this.copyArgsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyArgsBtn.Name = "copyArgsBtn";
            this.copyArgsBtn.Size = new System.Drawing.Size(129, 67);
            this.copyArgsBtn.Text = "CopyArgs";
            this.copyArgsBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.copyArgsBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // pasteArgsBtn
            // 
            this.pasteArgsBtn.Image = global::sync.mgr.Properties.Resources.PasteArgs;
            this.pasteArgsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteArgsBtn.Name = "pasteArgsBtn";
            this.pasteArgsBtn.Size = new System.Drawing.Size(132, 67);
            this.pasteArgsBtn.Text = "PasteArgs";
            this.pasteArgsBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pasteArgsBtn.Click += new System.EventHandler(this.pasteBtn_Click);
            // 
            // spreadArgsBtn
            // 
            this.spreadArgsBtn.Image = global::sync.mgr.Properties.Resources.SpreadArgs;
            this.spreadArgsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.spreadArgsBtn.Name = "spreadArgsBtn";
            this.spreadArgsBtn.Size = new System.Drawing.Size(150, 67);
            this.spreadArgsBtn.Text = "SpreadArgs";
            this.spreadArgsBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.spreadArgsBtn.Click += new System.EventHandler(this.spreadBtn_Click);
            // 
            // modifyPwdBtn
            // 
            this.modifyPwdBtn.Image = global::sync.mgr.Properties.Resources.SetPwd;
            this.modifyPwdBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modifyPwdBtn.Name = "modifyPwdBtn";
            this.modifyPwdBtn.Size = new System.Drawing.Size(147, 67);
            this.modifyPwdBtn.Text = "ModifyPwd";
            this.modifyPwdBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.modifyPwdBtn.Click += new System.EventHandler(this.modifyPwdBtn_Click);
            // 
            // mountBtn
            // 
            this.mountBtn.Image = global::sync.mgr.Properties.Resources.Mount;
            this.mountBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mountBtn.Name = "mountBtn";
            this.mountBtn.Size = new System.Drawing.Size(95, 67);
            this.mountBtn.Text = "Mount";
            this.mountBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.mountBtn.Click += new System.EventHandler(this.mountBtn_Click);
            // 
            // unmountBtn
            // 
            this.unmountBtn.Image = global::sync.mgr.Properties.Resources.Unmount;
            this.unmountBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.unmountBtn.Name = "unmountBtn";
            this.unmountBtn.Size = new System.Drawing.Size(127, 67);
            this.unmountBtn.Text = "Unmount";
            this.unmountBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.unmountBtn.Click += new System.EventHandler(this.unmountBtn_Click);
            // 
            // openDirBtn
            // 
            this.openDirBtn.Image = global::sync.mgr.Properties.Resources.OpenDir;
            this.openDirBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openDirBtn.Name = "openDirBtn";
            this.openDirBtn.Size = new System.Drawing.Size(115, 67);
            this.openDirBtn.Text = "OpenDir";
            this.openDirBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.openDirBtn.Click += new System.EventHandler(this.openDirBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.Image = global::sync.mgr.Properties.Resources.Run;
            this.runBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(64, 67);
            this.runBtn.Text = "Run";
            this.runBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // batchRunBtn
            // 
            this.batchRunBtn.Image = global::sync.mgr.Properties.Resources.BatchRun;
            this.batchRunBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.batchRunBtn.Name = "batchRunBtn";
            this.batchRunBtn.Size = new System.Drawing.Size(128, 67);
            this.batchRunBtn.Text = "BatchRun";
            this.batchRunBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.batchRunBtn.Click += new System.EventHandler(this.batchRunBtn_Click);
            // 
            // SyncManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1611, 815);
            this.Controls.Add(this.linkUI);
            this.Controls.Add(this.splitter4);
            this.Controls.Add(this.msgUI);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.treeUI);
            this.Controls.Add(this.toolbar);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SyncManager";
            this.Text = "SyncManager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SyncManager_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.TreeView treeUI;
        private System.Windows.Forms.PropertyGrid propUI;
        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.Splitter splitter1;
        private link.LinkView linkUI;
        public System.Windows.Forms.ToolStripButton upBtn;
        public System.Windows.Forms.ToolStripButton downBtn;
        public System.Windows.Forms.ToolStripButton addSpaceBtn;
        public System.Windows.Forms.ToolStripButton editLabelBtn;
        public System.Windows.Forms.ToolStripButton deleteBtn;
        public System.Windows.Forms.ToolStripButton openDirBtn;
        public System.Windows.Forms.ToolStripButton runBtn;
        public System.Windows.Forms.ToolStripButton batchRunBtn;
        public System.Windows.Forms.ToolStripButton addAeadFSBtn;
        public System.Windows.Forms.ToolStripButton addNormalDirBtn;
        public System.Windows.Forms.ToolStripButton masterSyncBtn;
        public System.Windows.Forms.ToolStripButton roundSyncBtn;
        public System.Windows.Forms.ToolStripButton copyArgsBtn;
        public System.Windows.Forms.ToolStripButton pasteArgsBtn;
        public System.Windows.Forms.ToolStripButton spreadArgsBtn;
        public System.Windows.Forms.ToolStripButton browseFilesBtn;
        public System.Windows.Forms.ToolStripButton manageFilesBtn;
        public System.Windows.Forms.ToolStripButton mountBtn;
        public System.Windows.Forms.ToolStripButton unmountBtn;
        public System.Windows.Forms.ToolStripButton modifyPwdBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter descSplit;
        private System.Windows.Forms.TextBox descUI;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Splitter splitter4;
        public System.Windows.Forms.ToolStripButton optionBtn;
        public System.Windows.Forms.ToolStripButton createViewBtn;
        public System.Windows.Forms.ToolStripButton linkViewBtn;
        private System.Windows.Forms.ToolStripDropDownButton languageBtn;
    }
}