namespace sync.ui
{
    partial class RepManager
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
            this.repPanel = new global::sync.ui.ViewPanel();
            this.SuspendLayout();
            // 
            // repPanel
            // 
            this.repPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repPanel.Location = new System.Drawing.Point(0, 0);
            this.repPanel.Name = "repPanel";
            this.repPanel.Size = new System.Drawing.Size(1287, 776);
            this.repPanel.TabIndex = 0;
            // 
            // RepManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 776);
            this.Controls.Add(this.repPanel);
            this.Name = "RepManager";
            this.Text = "RepManager";
            this.Load += new System.EventHandler(this.RepManager_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public ViewPanel repPanel;
    }
}