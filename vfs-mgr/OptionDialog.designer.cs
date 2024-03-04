namespace vfs.mgr
{
    partial class OptionDialog
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
            this.propUI = new System.Windows.Forms.PropertyGrid();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.descUI = new System.Windows.Forms.TextBox();
            this.descSplit = new System.Windows.Forms.Splitter();
            this.openDirBtn = new System.Windows.Forms.ToolStripButton();
            this.resetBtn = new System.Windows.Forms.ToolStripButton();
            this.evalPBKDF2Btn = new System.Windows.Forms.ToolStripButton();
            this.evalArgon2idBtn = new System.Windows.Forms.ToolStripButton();
            this.evalAeadFSBtn = new System.Windows.Forms.ToolStripButton();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // propUI
            // 
            this.propUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propUI.HelpVisible = false;
            this.propUI.LargeButtons = true;
            this.propUI.Location = new System.Drawing.Point(0, 80);
            this.propUI.Name = "propUI";
            this.propUI.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propUI.Size = new System.Drawing.Size(874, 467);
            this.propUI.TabIndex = 0;
            this.propUI.ToolbarVisible = false;
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDirBtn,
            this.resetBtn,
            this.evalPBKDF2Btn,
            this.evalArgon2idBtn,
            this.evalAeadFSBtn});
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Padding = new System.Windows.Forms.Padding(5);
            this.toolbar.Size = new System.Drawing.Size(874, 80);
            this.toolbar.TabIndex = 1;
            // 
            // descUI
            // 
            this.descUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descUI.Location = new System.Drawing.Point(0, 557);
            this.descUI.Multiline = true;
            this.descUI.Name = "descUI";
            this.descUI.Size = new System.Drawing.Size(874, 145);
            this.descUI.TabIndex = 2;
            // 
            // descSplit
            // 
            this.descSplit.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.descSplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descSplit.Location = new System.Drawing.Point(0, 547);
            this.descSplit.Name = "descSplit";
            this.descSplit.Size = new System.Drawing.Size(874, 10);
            this.descSplit.TabIndex = 3;
            this.descSplit.TabStop = false;
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
            // resetBtn
            // 
            this.resetBtn.Image = global::vfs.mgr.Properties.Resources.Reset;
            this.resetBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(82, 67);
            this.resetBtn.Text = "Reset";
            this.resetBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // evalPBKDF2Btn
            // 
            this.evalPBKDF2Btn.Image = global::vfs.mgr.Properties.Resources.EvalTime;
            this.evalPBKDF2Btn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.evalPBKDF2Btn.Name = "evalPBKDF2Btn";
            this.evalPBKDF2Btn.Size = new System.Drawing.Size(153, 67);
            this.evalPBKDF2Btn.Text = "EvalPBKDF2";
            this.evalPBKDF2Btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.evalPBKDF2Btn.Click += new System.EventHandler(this.evalPbkdf2Btn_Click);
            // 
            // evalArgon2idBtn
            // 
            this.evalArgon2idBtn.Image = global::vfs.mgr.Properties.Resources.EvalTime;
            this.evalArgon2idBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.evalArgon2idBtn.Name = "evalArgon2idBtn";
            this.evalArgon2idBtn.Size = new System.Drawing.Size(169, 67);
            this.evalArgon2idBtn.Text = "EvalArgon2id";
            this.evalArgon2idBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.evalArgon2idBtn.Click += new System.EventHandler(this.evalArgon2idBtn_Click);
            // 
            // evalAeadFSBtn
            // 
            this.evalAeadFSBtn.Image = global::vfs.mgr.Properties.Resources.EvalTime;
            this.evalAeadFSBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.evalAeadFSBtn.Name = "evalAeadFSBtn";
            this.evalAeadFSBtn.Size = new System.Drawing.Size(149, 67);
            this.evalAeadFSBtn.Text = "EvalAeadFS";
            this.evalAeadFSBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.evalAeadFSBtn.Click += new System.EventHandler(this.evalAeadFSBtn_Click);
            // 
            // OptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 702);
            this.Controls.Add(this.propUI);
            this.Controls.Add(this.descSplit);
            this.Controls.Add(this.descUI);
            this.Controls.Add(this.toolbar);
            this.Name = "OptionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Option";
            this.Load += new System.EventHandler(this.OptionDialog_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propUI;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.TextBox descUI;
        private System.Windows.Forms.Splitter descSplit;
        public System.Windows.Forms.ToolStripButton resetBtn;
        public System.Windows.Forms.ToolStripButton openDirBtn;
        public System.Windows.Forms.ToolStripButton evalPBKDF2Btn;
        public System.Windows.Forms.ToolStripButton evalArgon2idBtn;
        public System.Windows.Forms.ToolStripButton evalAeadFSBtn;
    }
}