namespace sync
{
    partial class WorkRunner
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.msgUI = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.parseBtn = new System.Windows.Forms.Button();
            this.workBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.actionUI = new System.Windows.Forms.TextBox();
            this.statusUI = new System.Windows.Forms.TextBox();
            this.actPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.transPanel = new System.Windows.Forms.Panel();
            this.resSplit = new System.Windows.Forms.Splitter();
            this.tableLayoutPanel2.SuspendLayout();
            this.actPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgUI
            // 
            this.msgUI.Dock = System.Windows.Forms.DockStyle.Right;
            this.msgUI.Location = new System.Drawing.Point(480, 0);
            this.msgUI.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.msgUI.MaxLength = 256;
            this.msgUI.Multiline = true;
            this.msgUI.Name = "msgUI";
            this.msgUI.Size = new System.Drawing.Size(478, 442);
            this.msgUI.TabIndex = 111;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.parseBtn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.workBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.stopBtn, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 474);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(958, 65);
            this.tableLayoutPanel2.TabIndex = 112;
            // 
            // parseBtn
            // 
            this.parseBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.parseBtn.Location = new System.Drawing.Point(219, 15);
            this.parseBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.parseBtn.Name = "parseBtn";
            this.parseBtn.Size = new System.Drawing.Size(120, 40);
            this.parseBtn.TabIndex = 1;
            this.parseBtn.Text = "Parse";
            this.parseBtn.UseVisualStyleBackColor = true;
            this.parseBtn.Click += new System.EventHandler(this.parseBtn_Click);
            // 
            // workBtn
            // 
            this.workBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.workBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.workBtn.Location = new System.Drawing.Point(419, 15);
            this.workBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.workBtn.Name = "workBtn";
            this.workBtn.Size = new System.Drawing.Size(120, 40);
            this.workBtn.TabIndex = 2;
            this.workBtn.Text = "Run";
            this.workBtn.UseVisualStyleBackColor = true;
            this.workBtn.Click += new System.EventHandler(this.workBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(619, 15);
            this.stopBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(120, 40);
            this.stopBtn.TabIndex = 3;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // actionUI
            // 
            this.actionUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.actionUI.Dock = System.Windows.Forms.DockStyle.Left;
            this.actionUI.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.actionUI.Location = new System.Drawing.Point(0, 0);
            this.actionUI.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.actionUI.Name = "actionUI";
            this.actionUI.ReadOnly = true;
            this.actionUI.Size = new System.Drawing.Size(174, 28);
            this.actionUI.TabIndex = 1;
            // 
            // statusUI
            // 
            this.statusUI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusUI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusUI.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusUI.Location = new System.Drawing.Point(180, 0);
            this.statusUI.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.statusUI.Name = "statusUI";
            this.statusUI.ReadOnly = true;
            this.statusUI.Size = new System.Drawing.Size(778, 28);
            this.statusUI.TabIndex = 2;
            this.statusUI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // actPanel
            // 
            this.actPanel.Controls.Add(this.statusUI);
            this.actPanel.Controls.Add(this.splitter1);
            this.actPanel.Controls.Add(this.actionUI);
            this.actPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actPanel.Location = new System.Drawing.Point(0, 442);
            this.actPanel.Margin = new System.Windows.Forms.Padding(2);
            this.actPanel.Name = "actPanel";
            this.actPanel.Size = new System.Drawing.Size(958, 32);
            this.actPanel.TabIndex = 116;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.splitter1.Location = new System.Drawing.Point(174, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 32);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // transPanel
            // 
            this.transPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transPanel.Location = new System.Drawing.Point(0, 0);
            this.transPanel.Name = "transPanel";
            this.transPanel.Size = new System.Drawing.Size(472, 442);
            this.transPanel.TabIndex = 117;
            // 
            // resSplit
            // 
            this.resSplit.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.resSplit.Dock = System.Windows.Forms.DockStyle.Right;
            this.resSplit.Location = new System.Drawing.Point(472, 0);
            this.resSplit.Name = "resSplit";
            this.resSplit.Size = new System.Drawing.Size(8, 442);
            this.resSplit.TabIndex = 118;
            this.resSplit.TabStop = false;
            // 
            // WorkRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 539);
            this.Controls.Add(this.transPanel);
            this.Controls.Add(this.resSplit);
            this.Controls.Add(this.msgUI);
            this.Controls.Add(this.actPanel);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "WorkRunner";
            this.Text = "WorkRunner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkRunner_FormClosing);
            this.Load += new System.EventHandler(this.WorkRunner_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.actPanel.ResumeLayout(false);
            this.actPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox msgUI;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button parseBtn;
        public System.Windows.Forms.Button workBtn;
        public System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.TextBox actionUI;
        private System.Windows.Forms.TextBox statusUI;
        private System.Windows.Forms.Panel actPanel;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel transPanel;
        private System.Windows.Forms.Splitter resSplit;
    }
}
