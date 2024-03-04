namespace sync.ui
{
    partial class ViewBrowser
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
            this.repSplit = new System.Windows.Forms.Splitter();
            this.repPanel = new global::sync.ui.ViewPanel();
            this.viewPanel = new global::sync.ui.ViewPanel();
            this.SuspendLayout();
            // 
            // repSplit
            // 
            this.repSplit.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.repSplit.Dock = System.Windows.Forms.DockStyle.Right;
            this.repSplit.Location = new System.Drawing.Point(861, 0);
            this.repSplit.Name = "repSplit";
            this.repSplit.Size = new System.Drawing.Size(10, 913);
            this.repSplit.TabIndex = 2;
            this.repSplit.TabStop = false;
            this.repSplit.Visible = false;
            // 
            // repPanel
            // 
            this.repPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.repPanel.Location = new System.Drawing.Point(871, 0);
            this.repPanel.Name = "repPanel";
            this.repPanel.Size = new System.Drawing.Size(801, 913);
            this.repPanel.TabIndex = 1;
            this.repPanel.view = null;
            this.repPanel.Visible = false;
            // 
            // viewPanel
            // 
            this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanel.Location = new System.Drawing.Point(0, 0);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(861, 913);
            this.viewPanel.TabIndex = 3;
            this.viewPanel.view = null;
            // 
            // ViewBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1672, 913);
            this.Controls.Add(this.viewPanel);
            this.Controls.Add(this.repSplit);
            this.Controls.Add(this.repPanel);
            this.Name = "ViewBrowser";
            this.Text = "ViewExplorer";
            this.Load += new System.EventHandler(this.SyncRepManager_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Splitter repSplit;
        public ViewPanel repPanel;
        public ViewPanel viewPanel;
    }
}