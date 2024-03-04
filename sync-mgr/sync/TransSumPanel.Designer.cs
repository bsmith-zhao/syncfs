namespace sync.sync
{
    partial class TransSumPanel
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
            this.SuspendLayout();
            // 
            // treeUI
            // 
            this.treeUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeUI.Location = new System.Drawing.Point(0, 0);
            this.treeUI.Name = "treeUI";
            this.treeUI.Size = new System.Drawing.Size(494, 525);
            this.treeUI.TabIndex = 0;
            // 
            // TransSumPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeUI);
            this.Name = "TransSumPanel";
            this.Size = new System.Drawing.Size(494, 525);
            this.Load += new System.EventHandler(this.TransferDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeUI;
    }
}