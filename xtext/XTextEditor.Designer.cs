namespace xtext
{
    partial class XTextEditor
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
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.optionBtn = new System.Windows.Forms.ToolStripButton();
            this.newBtn = new System.Windows.Forms.ToolStripButton();
            this.openBtn = new System.Windows.Forms.ToolStripButton();
            this.saveBtn = new System.Windows.Forms.ToolStripButton();
            this.saveAsBtn = new System.Windows.Forms.ToolStripButton();
            this.setPwdBtn = new System.Windows.Forms.ToolStripButton();
            this.textUI = new System.Windows.Forms.TextBox();
            this.msgUI = new System.Windows.Forms.TextBox();
            this.splitUI = new System.Windows.Forms.Splitter();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionBtn,
            this.newBtn,
            this.openBtn,
            this.saveBtn,
            this.saveAsBtn,
            this.setPwdBtn});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(1126, 70);
            this.toolbar.TabIndex = 8;
            this.toolbar.Text = "toolStrip1";
            // 
            // optionBtn
            // 
            this.optionBtn.Image = global::xtext.Properties.Resources.Option;
            this.optionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optionBtn.Name = "optionBtn";
            this.optionBtn.Size = new System.Drawing.Size(98, 67);
            this.optionBtn.Text = "Option";
            this.optionBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.optionBtn.Click += new System.EventHandler(this.optionBtn_Click);
            // 
            // newBtn
            // 
            this.newBtn.Image = global::xtext.Properties.Resources.New;
            this.newBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newBtn.Name = "newBtn";
            this.newBtn.Size = new System.Drawing.Size(71, 67);
            this.newBtn.Text = "New";
            this.newBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.newBtn.Click += new System.EventHandler(this.newBtn_Click);
            // 
            // openBtn
            // 
            this.openBtn.Image = global::xtext.Properties.Resources.OpenDir;
            this.openBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openBtn.Name = "openBtn";
            this.openBtn.Size = new System.Drawing.Size(82, 67);
            this.openBtn.Text = "Open";
            this.openBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.openBtn.Click += new System.EventHandler(this.openBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Image = global::xtext.Properties.Resources.Save;
            this.saveBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(72, 67);
            this.saveBtn.Text = "Save";
            this.saveBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // saveAsBtn
            // 
            this.saveAsBtn.Image = global::xtext.Properties.Resources.SaveAs;
            this.saveAsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAsBtn.Name = "saveAsBtn";
            this.saveAsBtn.Size = new System.Drawing.Size(100, 67);
            this.saveAsBtn.Text = "SaveAs";
            this.saveAsBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.saveAsBtn.Click += new System.EventHandler(this.saveAsBtn_Click);
            // 
            // setPwdBtn
            // 
            this.setPwdBtn.Image = global::xtext.Properties.Resources.SetPwd;
            this.setPwdBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setPwdBtn.Name = "setPwdBtn";
            this.setPwdBtn.Size = new System.Drawing.Size(104, 67);
            this.setPwdBtn.Text = "SetPwd";
            this.setPwdBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.setPwdBtn.Click += new System.EventHandler(this.setPwdBtn_Click);
            // 
            // textUI
            // 
            this.textUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textUI.Location = new System.Drawing.Point(0, 70);
            this.textUI.Multiline = true;
            this.textUI.Name = "textUI";
            this.textUI.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textUI.Size = new System.Drawing.Size(824, 551);
            this.textUI.TabIndex = 9;
            this.textUI.WordWrap = false;
            // 
            // msgUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Right;
            this.msgUI.Location = new System.Drawing.Point(834, 70);
            this.msgUI.Multiline = true;
            this.msgUI.Name = "msgUI";
            this.msgUI.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.msgUI.Size = new System.Drawing.Size(292, 551);
            this.msgUI.TabIndex = 10;
            this.msgUI.WordWrap = false;
            // 
            // splitUI
            // 
            this.splitUI.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitUI.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitUI.Location = new System.Drawing.Point(824, 70);
            this.splitUI.Name = "splitUI";
            this.splitUI.Size = new System.Drawing.Size(10, 551);
            this.splitUI.TabIndex = 11;
            this.splitUI.TabStop = false;
            // 
            // XTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 621);
            this.Controls.Add(this.textUI);
            this.Controls.Add(this.splitUI);
            this.Controls.Add(this.msgUI);
            this.Controls.Add(this.toolbar);
            this.Name = "XTextEditor";
            this.Text = "XTextEditor";
            this.Load += new System.EventHandler(this.NoteForm_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbar;
        public System.Windows.Forms.ToolStripButton optionBtn;
        private System.Windows.Forms.TextBox textUI;
        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.Splitter splitUI;
        public System.Windows.Forms.ToolStripButton openBtn;
        private System.Windows.Forms.ToolStripButton saveBtn;
        private System.Windows.Forms.ToolStripButton saveAsBtn;
        private System.Windows.Forms.ToolStripButton newBtn;
        private System.Windows.Forms.ToolStripButton setPwdBtn;
    }
}

