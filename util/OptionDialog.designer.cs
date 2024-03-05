namespace util
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
            this.openDirBtn = new System.Windows.Forms.ToolStripButton();
            this.resetBtn = new System.Windows.Forms.ToolStripButton();
            this.descUI = new System.Windows.Forms.TextBox();
            this.descSplit = new System.Windows.Forms.Splitter();
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
            this.resetBtn});
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Padding = new System.Windows.Forms.Padding(5);
            this.toolbar.Size = new System.Drawing.Size(874, 80);
            this.toolbar.TabIndex = 1;
            // 
            // openDirBtn
            // 
            this.openDirBtn.Image = global::util.Properties.Resources.OpenDir;
            this.openDirBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openDirBtn.Name = "openDirBtn";
            this.openDirBtn.Size = new System.Drawing.Size(115, 67);
            this.openDirBtn.Text = "OpenDir";
            this.openDirBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.openDirBtn.Click += new System.EventHandler(this.openDirBtn_Click);
            // 
            // resetBtn
            // 
            this.resetBtn.Image = global::util.Properties.Resources.Reset;
            this.resetBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(82, 67);
            this.resetBtn.Text = "Reset";
            this.resetBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
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
    }
}