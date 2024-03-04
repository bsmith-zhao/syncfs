namespace util.prop
{
    partial class PropDialog
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
            this.descUI = new System.Windows.Forms.TextBox();
            this.splitUI = new System.Windows.Forms.Splitter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propUI
            // 
            this.propUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propUI.HelpVisible = false;
            this.propUI.LargeButtons = true;
            this.propUI.Location = new System.Drawing.Point(0, 0);
            this.propUI.Name = "propUI";
            this.propUI.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propUI.Size = new System.Drawing.Size(732, 411);
            this.propUI.TabIndex = 0;
            this.propUI.ToolbarVisible = false;
            // 
            // descUI
            // 
            this.descUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.descUI.Location = new System.Drawing.Point(0, 421);
            this.descUI.Multiline = true;
            this.descUI.Name = "descUI";
            this.descUI.Size = new System.Drawing.Size(732, 88);
            this.descUI.TabIndex = 2;
            // 
            // splitUI
            // 
            this.splitUI.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.splitUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitUI.Location = new System.Drawing.Point(0, 411);
            this.splitUI.Name = "splitUI";
            this.splitUI.Size = new System.Drawing.Size(732, 10);
            this.splitUI.TabIndex = 3;
            this.splitUI.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.okBtn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cancelBtn, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 509);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(732, 65);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(173, 12);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(120, 40);
            this.okBtn.TabIndex = 3;
            this.okBtn.Text = "确定";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(439, 12);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(120, 40);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // PropDialog
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(732, 574);
            this.Controls.Add(this.propUI);
            this.Controls.Add(this.splitUI);
            this.Controls.Add(this.descUI);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PropDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Property";
            this.Load += new System.EventHandler(this.SetupDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propUI;
        private System.Windows.Forms.TextBox descUI;
        private System.Windows.Forms.Splitter splitUI;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Button okBtn;
        public System.Windows.Forms.Button cancelBtn;
    }
}