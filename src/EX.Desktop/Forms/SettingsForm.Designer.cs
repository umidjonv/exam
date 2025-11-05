using EX.Desktop.Helpers;

namespace EX.Desktop.Forms
{
    partial class SettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.cbxAudioDevices = new System.Windows.Forms.ComboBox();
            this.btnTestSound = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxCameraDevices = new System.Windows.Forms.ComboBox();
            this.btnTestCamera = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblClock = new System.Windows.Forms.Label();
            this.lblLogout = new System.Windows.Forms.LinkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnInternet = new System.Windows.Forms.Button();
            this.lblSpeedLan = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxAudioDevices
            // 
            this.cbxAudioDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxAudioDevices.BackColor = System.Drawing.SystemColors.Window;
            this.cbxAudioDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAudioDevices.FormattingEnabled = true;
            this.cbxAudioDevices.Location = new System.Drawing.Point(17, 32);
            this.cbxAudioDevices.Margin = new System.Windows.Forms.Padding(2);
            this.cbxAudioDevices.Name = "cbxAudioDevices";
            this.cbxAudioDevices.Size = new System.Drawing.Size(411, 29);
            this.cbxAudioDevices.TabIndex = 1;
            // 
            // btnTestSound
            // 
            this.btnTestSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestSound.Enabled = false;
            this.btnTestSound.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTestSound.Location = new System.Drawing.Point(440, 31);
            this.btnTestSound.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestSound.Name = "btnTestSound";
            this.btnTestSound.Size = new System.Drawing.Size(80, 30);
            this.btnTestSound.TabIndex = 2;
            this.btnTestSound.Text = "Тест";
            this.btnTestSound.UseVisualStyleBackColor = true;
            this.btnTestSound.Click += new System.EventHandler(this.BtnTestSound_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxAudioDevices);
            this.groupBox1.Controls.Add(this.btnTestSound);
            this.groupBox1.Location = new System.Drawing.Point(25, 110);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(533, 75);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Звук";
            // 
            // cbxCameraDevices
            // 
            this.cbxCameraDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxCameraDevices.BackColor = System.Drawing.SystemColors.Window;
            this.cbxCameraDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCameraDevices.FormattingEnabled = true;
            this.cbxCameraDevices.Location = new System.Drawing.Point(17, 31);
            this.cbxCameraDevices.Margin = new System.Windows.Forms.Padding(2);
            this.cbxCameraDevices.Name = "cbxCameraDevices";
            this.cbxCameraDevices.Size = new System.Drawing.Size(411, 29);
            this.cbxCameraDevices.TabIndex = 1;
            // 
            // btnTestCamera
            // 
            this.btnTestCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestCamera.Enabled = false;
            this.btnTestCamera.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTestCamera.Location = new System.Drawing.Point(440, 31);
            this.btnTestCamera.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestCamera.Name = "btnTestCamera";
            this.btnTestCamera.Size = new System.Drawing.Size(80, 30);
            this.btnTestCamera.TabIndex = 2;
            this.btnTestCamera.Text = "Тест";
            this.btnTestCamera.UseVisualStyleBackColor = true;
            this.btnTestCamera.Click += new System.EventHandler(this.BtnTestCamera_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxCameraDevices);
            this.groupBox2.Controls.Add(this.btnTestCamera);
            this.groupBox2.Location = new System.Drawing.Point(25, 196);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(533, 75);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Камера";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.Location = new System.Drawing.Point(250, 283);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 40);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Начать";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.lblUserName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 371);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.statusStrip1.Size = new System.Drawing.Size(587, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(112, 21);
            this.toolStripStatusLabel3.Text = "Мой профиль:";
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUserName.IsLink = true;
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(54, 21);
            this.lblUserName.Text = "admin";
            this.lblUserName.Click += new System.EventHandler(this.LblUserName_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(50, 17);
            this.toolStripStatusLabel2.Text = "Logged:";
            // 
            // lblClock
            // 
            this.lblClock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblClock.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblClock.ForeColor = System.Drawing.Color.Black;
            this.lblClock.Location = new System.Drawing.Point(25, 334);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(533, 31);
            this.lblClock.TabIndex = 11;
            this.lblClock.Text = "Статус экзамена обновляется ...";
            this.lblClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLogout
            // 
            this.lblLogout.AutoSize = true;
            this.lblLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLogout.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLogout.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lblLogout.LinkColor = System.Drawing.Color.Red;
            this.lblLogout.Location = new System.Drawing.Point(518, 4);
            this.lblLogout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLogout.Name = "lblLogout";
            this.lblLogout.Size = new System.Drawing.Size(60, 21);
            this.lblLogout.TabIndex = 12;
            this.lblLogout.TabStop = true;
            this.lblLogout.Text = "Выйти";
            this.lblLogout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblLogout_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnInternet);
            this.groupBox3.Controls.Add(this.lblSpeedLan);
            this.groupBox3.Location = new System.Drawing.Point(25, 31);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(533, 75);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Интернет";
            // 
            // btnInternet
            // 
            this.btnInternet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInternet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnInternet.Location = new System.Drawing.Point(440, 31);
            this.btnInternet.Margin = new System.Windows.Forms.Padding(2);
            this.btnInternet.Name = "btnInternet";
            this.btnInternet.Size = new System.Drawing.Size(80, 30);
            this.btnInternet.TabIndex = 3;
            this.btnInternet.Text = "Тест";
            this.btnInternet.UseVisualStyleBackColor = true;
            this.btnInternet.Click += new System.EventHandler(this.BtnInternet_Click);
            // 
            // lblSpeedLan
            // 
            this.lblSpeedLan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSpeedLan.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpeedLan.Location = new System.Drawing.Point(17, 34);
            this.lblSpeedLan.Name = "lblSpeedLan";
            this.lblSpeedLan.Size = new System.Drawing.Size(411, 25);
            this.lblSpeedLan.TabIndex = 0;
            this.lblSpeedLan.Text = "Проверьте скорость вашего интернета ...";
            this.lblSpeedLan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(587, 397);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblLogout);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки устройства";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbxAudioDevices;
        private System.Windows.Forms.Button btnTestSound;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxCameraDevices;
        private System.Windows.Forms.Button btnTestCamera;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblUserName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.LinkLabel lblLogout;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblSpeedLan;
        private System.Windows.Forms.Button btnInternet;
    }
}

