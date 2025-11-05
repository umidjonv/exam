namespace EX.Desktop.Forms
{
    partial class ProxyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxyForm));
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chbxDefault = new System.Windows.Forms.CheckBox();
            this.gpxOption = new System.Windows.Forms.GroupBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.tbxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gpxOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(204, 253);
            this.btnApply.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(100, 32);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(91, 253);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 32);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // chbxDefault
            // 
            this.chbxDefault.AutoSize = true;
            this.chbxDefault.Checked = true;
            this.chbxDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbxDefault.Location = new System.Drawing.Point(16, 24);
            this.chbxDefault.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.chbxDefault.Name = "chbxDefault";
            this.chbxDefault.Size = new System.Drawing.Size(147, 24);
            this.chbxDefault.TabIndex = 11;
            this.chbxDefault.Text = "По умолчанию?";
            this.chbxDefault.UseVisualStyleBackColor = true;
            this.chbxDefault.CheckedChanged += new System.EventHandler(this.ChbxDefault_CheckedChanged);
            // 
            // gpxOption
            // 
            this.gpxOption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpxOption.Controls.Add(this.numPort);
            this.gpxOption.Controls.Add(this.tbxPassword);
            this.gpxOption.Controls.Add(this.label4);
            this.gpxOption.Controls.Add(this.tbxUser);
            this.gpxOption.Controls.Add(this.label3);
            this.gpxOption.Controls.Add(this.label2);
            this.gpxOption.Controls.Add(this.tbxHost);
            this.gpxOption.Controls.Add(this.label1);
            this.gpxOption.Location = new System.Drawing.Point(16, 52);
            this.gpxOption.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.gpxOption.Name = "gpxOption";
            this.gpxOption.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.gpxOption.Size = new System.Drawing.Size(287, 182);
            this.gpxOption.TabIndex = 12;
            this.gpxOption.TabStop = false;
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(199, 34);
            this.numPort.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(80, 26);
            this.numPort.TabIndex = 18;
            this.numPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // tbxPassword
            // 
            this.tbxPassword.Location = new System.Drawing.Point(13, 146);
            this.tbxPassword.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.PasswordChar = '*';
            this.tbxPassword.Size = new System.Drawing.Size(268, 26);
            this.tbxPassword.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 125);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Пароль:";
            // 
            // tbxUser
            // 
            this.tbxUser.Location = new System.Drawing.Point(13, 88);
            this.tbxUser.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.Size = new System.Drawing.Size(268, 26);
            this.tbxUser.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 67);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Имя пользователя:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Порт:";
            // 
            // tbxHost
            // 
            this.tbxHost.Location = new System.Drawing.Point(13, 34);
            this.tbxHost.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tbxHost.Name = "tbxHost";
            this.tbxHost.Size = new System.Drawing.Size(145, 26);
            this.tbxHost.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Хост:";
            // 
            // ProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(327, 305);
            this.Controls.Add(this.gpxOption);
            this.Controls.Add(this.chbxDefault);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProxyForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Прокси";
            this.Load += new System.EventHandler(this.ProxyForm_Load);
            this.gpxOption.ResumeLayout(false);
            this.gpxOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chbxDefault;
        private System.Windows.Forms.GroupBox gpxOption;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.TextBox tbxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxHost;
        private System.Windows.Forms.Label label1;
    }
}