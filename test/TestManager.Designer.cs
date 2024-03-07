namespace test
{
    partial class TestManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestManager));
            this.msgUI = new System.Windows.Forms.TextBox();
            this.statUI = new System.Windows.Forms.TextBox();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.runBtn = new System.Windows.Forms.ToolStripButton();
            this.stopBtn = new System.Windows.Forms.ToolStripButton();
            this.treeUI = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // logUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgUI.Location = new System.Drawing.Point(403, 38);
            this.msgUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.msgUI.Multiline = true;
            this.msgUI.Name = "logUI";
            this.msgUI.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.msgUI.Size = new System.Drawing.Size(898, 720);
            this.msgUI.TabIndex = 0;
            // 
            // statUI
            // 
            this.statUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statUI.Location = new System.Drawing.Point(403, 767);
            this.statUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.statUI.Name = "statUI";
            this.statUI.Size = new System.Drawing.Size(898, 39);
            this.statUI.TabIndex = 1;
            this.statUI.WordWrap = false;
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runBtn,
            this.stopBtn});
            this.toolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolbar.Location = new System.Drawing.Point(4, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(1297, 38);
            this.toolbar.TabIndex = 2;
            this.toolbar.Text = "toolStrip1";
            // 
            // runBtn
            // 
            this.runBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.runBtn.Image = ((System.Drawing.Image)(resources.GetObject("runBtn.Image")));
            this.runBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(64, 35);
            this.runBtn.Text = "Run";
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopBtn.Image = ((System.Drawing.Image)(resources.GetObject("stopBtn.Image")));
            this.stopBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(71, 35);
            this.stopBtn.Text = "Stop";
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // treeUI
            // 
            this.treeUI.CheckBoxes = true;
            this.treeUI.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeUI.Location = new System.Drawing.Point(4, 38);
            this.treeUI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeUI.Name = "treeUI";
            this.treeUI.Size = new System.Drawing.Size(389, 768);
            this.treeUI.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(393, 38);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 768);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(403, 758);
            this.splitter2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(898, 9);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // TestManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 811);
            this.Controls.Add(this.msgUI);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.statUI);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.treeUI);
            this.Controls.Add(this.toolbar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TestManager";
            this.Padding = new System.Windows.Forms.Padding(4, 0, 4, 5);
            this.Text = "TestManager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TestManager_Load);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.TextBox statUI;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripButton runBtn;
        private System.Windows.Forms.ToolStripButton stopBtn;
        private System.Windows.Forms.TreeView treeUI;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
    }
}