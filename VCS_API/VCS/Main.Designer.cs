﻿namespace VCS
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            txtWarehouse = new Label();
            txtUsername = new Label();
            Navigation = new Panel();
            btnLogOut = new Panel();
            pictureBox10 = new PictureBox();
            label9 = new Label();
            btnStatus = new Panel();
            p6 = new Panel();
            pictureBox3 = new PictureBox();
            label2 = new Label();
            btnSetting = new Panel();
            p5 = new Panel();
            pictureBox4 = new PictureBox();
            label3 = new Label();
            btnHistory = new Panel();
            p4 = new Panel();
            pictureBox5 = new PictureBox();
            label4 = new Label();
            btnCheckOut = new Panel();
            p3 = new Panel();
            pictureBox6 = new PictureBox();
            label5 = new Label();
            btnCheckIn = new Panel();
            p2 = new Panel();
            pictureBox7 = new PictureBox();
            label6 = new Label();
            pHome = new Panel();
            p1 = new Panel();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            pictureBox8 = new PictureBox();
            linkLabel1 = new LinkLabel();
            label10 = new Label();
            panelMain = new Panel();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            Navigation.SuspendLayout();
            btnLogOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            btnStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            btnSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            btnHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            btnCheckOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            btnCheckIn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            pHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // txtWarehouse
            // 
            txtWarehouse.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtWarehouse.AutoSize = true;
            txtWarehouse.BackColor = Color.Transparent;
            txtWarehouse.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtWarehouse.ForeColor = Color.FromArgb(13, 92, 171);
            txtWarehouse.ImageAlign = ContentAlignment.MiddleRight;
            txtWarehouse.Location = new Point(50, 42);
            txtWarehouse.Name = "txtWarehouse";
            txtWarehouse.Size = new Size(31, 17);
            txtWarehouse.TabIndex = 1;
            txtWarehouse.Text = "Kho";
            txtWarehouse.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUsername.AutoSize = true;
            txtUsername.BackColor = Color.Transparent;
            txtUsername.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUsername.ForeColor = Color.FromArgb(13, 92, 171);
            txtUsername.ImageAlign = ContentAlignment.MiddleRight;
            txtUsername.Location = new Point(48, 20);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(79, 21);
            txtUsername.TabIndex = 0;
            txtUsername.Text = "Hệ thống";
            txtUsername.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Navigation
            // 
            Navigation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Navigation.BackColor = Color.White;
            Navigation.BackgroundImageLayout = ImageLayout.None;
            Navigation.Controls.Add(btnLogOut);
            Navigation.Controls.Add(btnStatus);
            Navigation.Controls.Add(btnSetting);
            Navigation.Controls.Add(btnHistory);
            Navigation.Controls.Add(btnCheckOut);
            Navigation.Controls.Add(btnCheckIn);
            Navigation.Controls.Add(pHome);
            Navigation.Location = new Point(0, 83);
            Navigation.Name = "Navigation";
            Navigation.Size = new Size(228, 639);
            Navigation.TabIndex = 1;
            // 
            // btnLogOut
            // 
            btnLogOut.BackColor = Color.Transparent;
            btnLogOut.Controls.Add(pictureBox10);
            btnLogOut.Controls.Add(label9);
            btnLogOut.Cursor = Cursors.Hand;
            btnLogOut.Location = new Point(0, 336);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.Padding = new Padding(0, 12, 0, 12);
            btnLogOut.Size = new Size(228, 56);
            btnLogOut.TabIndex = 3;
            btnLogOut.Click += btnLogOut_Click;
            // 
            // pictureBox10
            // 
            pictureBox10.Image = Properties.Resources.icons8_lock_18__1_;
            pictureBox10.Location = new Point(16, 15);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(26, 26);
            pictureBox10.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox10.TabIndex = 1;
            pictureBox10.TabStop = false;
            pictureBox10.Click += pictureBox10_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(46, 18);
            label9.Name = "label9";
            label9.Size = new Size(80, 21);
            label9.TabIndex = 0;
            label9.Text = "Đăng xuất";
            label9.Click += label9_Click;
            // 
            // btnStatus
            // 
            btnStatus.BackColor = Color.Transparent;
            btnStatus.Controls.Add(p6);
            btnStatus.Controls.Add(pictureBox3);
            btnStatus.Controls.Add(label2);
            btnStatus.Cursor = Cursors.Hand;
            btnStatus.Location = new Point(0, 280);
            btnStatus.Name = "btnStatus";
            btnStatus.Padding = new Padding(0, 12, 0, 12);
            btnStatus.Size = new Size(228, 56);
            btnStatus.TabIndex = 2;
            btnStatus.Click += btnStatus_Click;
            // 
            // p6
            // 
            p6.BackColor = Color.Transparent;
            p6.Dock = DockStyle.Right;
            p6.Location = new Point(224, 12);
            p6.Name = "p6";
            p6.Size = new Size(4, 32);
            p6.TabIndex = 3;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.icons8_computer_18;
            pictureBox3.Location = new Point(16, 15);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(26, 26);
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.TabIndex = 1;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(46, 18);
            label2.Name = "label2";
            label2.Size = new Size(130, 21);
            label2.TabIndex = 0;
            label2.Text = "Trạng thái kết nối";
            label2.Click += label2_Click;
            // 
            // btnSetting
            // 
            btnSetting.BackColor = Color.Transparent;
            btnSetting.Controls.Add(p5);
            btnSetting.Controls.Add(pictureBox4);
            btnSetting.Controls.Add(label3);
            btnSetting.Cursor = Cursors.Hand;
            btnSetting.Location = new Point(0, 224);
            btnSetting.Name = "btnSetting";
            btnSetting.Padding = new Padding(0, 12, 0, 12);
            btnSetting.Size = new Size(228, 56);
            btnSetting.TabIndex = 2;
            btnSetting.Click += btnSetting_Click;
            // 
            // p5
            // 
            p5.BackColor = Color.Transparent;
            p5.Dock = DockStyle.Right;
            p5.Location = new Point(224, 12);
            p5.Name = "p5";
            p5.Size = new Size(4, 32);
            p5.TabIndex = 3;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.icons8_setting_18__1_;
            pictureBox4.Location = new Point(16, 15);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(26, 26);
            pictureBox4.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox4.TabIndex = 1;
            pictureBox4.TabStop = false;
            pictureBox4.Click += pictureBox4_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(46, 18);
            label3.Name = "label3";
            label3.Size = new Size(119, 21);
            label3.TabIndex = 0;
            label3.Text = "Cấu hình chung";
            label3.Click += label3_Click;
            // 
            // btnHistory
            // 
            btnHistory.BackColor = Color.Transparent;
            btnHistory.Controls.Add(p4);
            btnHistory.Controls.Add(pictureBox5);
            btnHistory.Controls.Add(label4);
            btnHistory.Cursor = Cursors.Hand;
            btnHistory.Location = new Point(0, 168);
            btnHistory.Name = "btnHistory";
            btnHistory.Padding = new Padding(0, 12, 0, 12);
            btnHistory.Size = new Size(228, 56);
            btnHistory.TabIndex = 2;
            btnHistory.Click += btnHistory_Click;
            // 
            // p4
            // 
            p4.BackColor = Color.Transparent;
            p4.Dock = DockStyle.Right;
            p4.Location = new Point(224, 12);
            p4.Name = "p4";
            p4.Size = new Size(4, 32);
            p4.TabIndex = 3;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.icons8_history_18__1_;
            pictureBox5.Location = new Point(16, 15);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(26, 26);
            pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox5.TabIndex = 1;
            pictureBox5.TabStop = false;
            pictureBox5.Click += pictureBox5_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(46, 18);
            label4.Name = "label4";
            label4.Size = new Size(105, 21);
            label4.TabIndex = 0;
            label4.Text = "Lịch sử vào ra";
            label4.Click += label4_Click;
            // 
            // btnCheckOut
            // 
            btnCheckOut.BackColor = Color.Transparent;
            btnCheckOut.Controls.Add(p3);
            btnCheckOut.Controls.Add(pictureBox6);
            btnCheckOut.Controls.Add(label5);
            btnCheckOut.Cursor = Cursors.Hand;
            btnCheckOut.Location = new Point(0, 112);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Padding = new Padding(0, 12, 0, 12);
            btnCheckOut.Size = new Size(228, 56);
            btnCheckOut.TabIndex = 2;
            btnCheckOut.Click += btnCheckOut_Click;
            // 
            // p3
            // 
            p3.BackColor = Color.Transparent;
            p3.Dock = DockStyle.Right;
            p3.Location = new Point(224, 12);
            p3.Name = "p3";
            p3.Size = new Size(4, 32);
            p3.TabIndex = 3;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = Properties.Resources.icons8_log_out_18__3_;
            pictureBox6.Location = new Point(16, 15);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(26, 26);
            pictureBox6.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox6.TabIndex = 1;
            pictureBox6.TabStop = false;
            pictureBox6.Click += pictureBox6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(46, 18);
            label5.Name = "label5";
            label5.Size = new Size(120, 21);
            label5.TabIndex = 0;
            label5.Text = "Quản lý cổng ra";
            label5.Click += label5_Click;
            // 
            // btnCheckIn
            // 
            btnCheckIn.BackColor = Color.Transparent;
            btnCheckIn.Controls.Add(p2);
            btnCheckIn.Controls.Add(pictureBox7);
            btnCheckIn.Controls.Add(label6);
            btnCheckIn.Cursor = Cursors.Hand;
            btnCheckIn.Location = new Point(0, 56);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Padding = new Padding(0, 12, 0, 12);
            btnCheckIn.Size = new Size(228, 56);
            btnCheckIn.TabIndex = 2;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // p2
            // 
            p2.BackColor = Color.Transparent;
            p2.Dock = DockStyle.Right;
            p2.Location = new Point(224, 12);
            p2.Name = "p2";
            p2.Size = new Size(4, 32);
            p2.TabIndex = 3;
            // 
            // pictureBox7
            // 
            pictureBox7.Image = Properties.Resources.icons8_log_in_18__3_;
            pictureBox7.Location = new Point(16, 15);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(26, 26);
            pictureBox7.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox7.TabIndex = 1;
            pictureBox7.TabStop = false;
            pictureBox7.Click += pictureBox7_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(46, 18);
            label6.Name = "label6";
            label6.Size = new Size(131, 21);
            label6.TabIndex = 0;
            label6.Text = "Quản lý cổng vào";
            label6.Click += label6_Click;
            // 
            // pHome
            // 
            pHome.BackColor = Color.Transparent;
            pHome.Controls.Add(p1);
            pHome.Controls.Add(pictureBox2);
            pHome.Controls.Add(label1);
            pHome.Cursor = Cursors.Hand;
            pHome.Location = new Point(0, 0);
            pHome.Name = "pHome";
            pHome.Padding = new Padding(0, 12, 0, 12);
            pHome.Size = new Size(228, 56);
            pHome.TabIndex = 2;
            pHome.Click += pHome_Click;
            // 
            // p1
            // 
            p1.BackColor = Color.FromArgb(13, 92, 171);
            p1.Dock = DockStyle.Right;
            p1.Location = new Point(224, 12);
            p1.Name = "p1";
            p1.Size = new Size(4, 32);
            p1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.icons8_home_18__2_;
            pictureBox2.Location = new Point(16, 15);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(26, 26);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(46, 18);
            label1.Name = "label1";
            label1.Size = new Size(78, 21);
            label1.TabIndex = 0;
            label1.Text = "Trang chủ";
            label1.Click += label1_Click;
            // 
            // pictureBox8
            // 
            pictureBox8.Image = Properties.Resources.user__2_;
            pictureBox8.Location = new Point(16, 26);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(26, 26);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 5;
            pictureBox8.TabStop = false;
            // 
            // linkLabel1
            // 
            linkLabel1.ActiveLinkColor = Color.DarkGray;
            linkLabel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            linkLabel1.AutoSize = true;
            linkLabel1.LinkColor = Color.DarkGray;
            linkLabel1.Location = new Point(1500, 4);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(68, 15);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "d2s.com.vn";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.ForeColor = Color.DarkGray;
            label10.Location = new Point(1403, 4);
            label10.Name = "label10";
            label10.Size = new Size(101, 15);
            label10.TabIndex = 5;
            label10.Text = "Copyright © 2025";
            // 
            // panelMain
            // 
            panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelMain.BackColor = Color.Transparent;
            panelMain.Location = new Point(229, 2);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1344, 776);
            panelMain.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.logo_menu2;
            pictureBox1.Location = new Point(24, 22);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(180, 38);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(0, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(228, 83);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(linkLabel1);
            panel2.Controls.Add(label10);
            panel2.Location = new Point(0, 777);
            panel2.Name = "panel2";
            panel2.Size = new Size(1579, 26);
            panel2.TabIndex = 4;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panel3.BackColor = Color.White;
            panel3.Controls.Add(txtWarehouse);
            panel3.Controls.Add(pictureBox8);
            panel3.Controls.Add(txtUsername);
            panel3.Location = new Point(0, 720);
            panel3.Name = "panel3";
            panel3.Size = new Size(228, 83);
            panel3.TabIndex = 5;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1574, 804);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(panelMain);
            Controls.Add(Navigation);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống VCS";
            Load += Main_Load;
            Navigation.ResumeLayout(false);
            btnLogOut.ResumeLayout(false);
            btnLogOut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            btnStatus.ResumeLayout(false);
            btnStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            btnSetting.ResumeLayout(false);
            btnSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            btnHistory.ResumeLayout(false);
            btnHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            btnCheckOut.ResumeLayout(false);
            btnCheckOut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            btnCheckIn.ResumeLayout(false);
            btnCheckIn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            pHome.ResumeLayout(false);
            pHome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel Navigation;
        private Panel btnStatus;
        private PictureBox pictureBox3;
        private Label label2;
        private Panel btnSetting;
        private PictureBox pictureBox4;
        private Label label3;
        private Panel btnHistory;
        private PictureBox pictureBox5;
        private Label label4;
        private Panel btnCheckOut;
        private PictureBox pictureBox6;
        private Label label5;
        private Panel btnCheckIn;
        private PictureBox pictureBox7;
        private Label label6;
        private Label txtWarehouse;
        private Label txtUsername;
        private Panel panelMain;
        private Panel pHome;
        private PictureBox pictureBox2;
        private Label label1;
        private Panel btnLogOut;
        private PictureBox pictureBox10;
        private Label label9;
        private LinkLabel linkLabel1;
        private Label label10;
        private Panel p6;
        private Panel p5;
        private Panel p4;
        private Panel p3;
        private Panel p2;
        private Panel p1;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox8;
        private Panel panel3;
    }
}