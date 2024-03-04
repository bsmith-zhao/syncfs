namespace test
{
    partial class TestForm
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
            this.richTextUI = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextUI
            // 
            this.richTextUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextUI.EnableAutoDragDrop = true;
            this.richTextUI.Location = new System.Drawing.Point(32, 126);
            this.richTextUI.Name = "richTextUI";
            this.richTextUI.ShowSelectionMargin = true;
            this.richTextUI.Size = new System.Drawing.Size(1153, 649);
            this.richTextUI.TabIndex = 0;
            this.richTextUI.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(360, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 61);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 808);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextUI);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextUI;
        private System.Windows.Forms.Button button1;
    }
}