using VCS.APP.Utilities;

namespace VCS.APP
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
            panelMenu = new Panel();
            btnHistory = new Button();
            btnConfig = new Button();
            pictureBox2 = new PictureBox();
            btnLogOut = new Button();
            btnCheckOut = new Button();
            btnCheckIn = new Button();
            btnHome = new Button();
            panelLogo = new Panel();
            panelMain = new Panel();
            panelTitle = new Panel();
            btnUser = new Button();
            label1 = new Label();
            labelTitle = new Label();
            pictureBox1 = new PictureBox();
            panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.AllowDrop = true;
            panelMenu.BackColor = Color.FromArgb(66, 66, 66);
            panelMenu.Controls.Add(btnHistory);
            panelMenu.Controls.Add(btnConfig);
            panelMenu.Controls.Add(pictureBox2);
            panelMenu.Controls.Add(btnLogOut);
            panelMenu.Controls.Add(btnCheckOut);
            panelMenu.Controls.Add(btnCheckIn);
            panelMenu.Controls.Add(btnHome);
            panelMenu.Controls.Add(panelLogo);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(200, 900);
            panelMenu.TabIndex = 0;
            panelMenu.Paint += panelMenu_Paint;
            // 
            // btnHistory
            // 
            btnHistory.Cursor = Cursors.Hand;
            btnHistory.Dock = DockStyle.Top;
            btnHistory.FlatAppearance.BorderSize = 0;
            btnHistory.FlatStyle = FlatStyle.Flat;
            btnHistory.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHistory.ForeColor = Color.White;
            btnHistory.Image = (Image)resources.GetObject("btnHistory.Image");
            btnHistory.ImageAlign = ContentAlignment.MiddleLeft;
            btnHistory.Location = new Point(0, 310);
            btnHistory.Name = "btnHistory";
            btnHistory.Padding = new Padding(12, 0, 0, 0);
            btnHistory.Size = new Size(200, 50);
            btnHistory.TabIndex = 7;
            btnHistory.Text = "Lịch sử ra vào";
            btnHistory.TextAlign = ContentAlignment.MiddleLeft;
            btnHistory.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHistory.UseVisualStyleBackColor = true;
            btnHistory.Click += button2_Click;
            // 
            // btnConfig
            // 
            btnConfig.Cursor = Cursors.Hand;
            btnConfig.Dock = DockStyle.Top;
            btnConfig.FlatAppearance.BorderSize = 0;
            btnConfig.FlatStyle = FlatStyle.Flat;
            btnConfig.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnConfig.ForeColor = Color.White;
            btnConfig.Image = (Image)resources.GetObject("btnConfig.Image");
            btnConfig.ImageAlign = ContentAlignment.MiddleLeft;
            btnConfig.Location = new Point(0, 260);
            btnConfig.Name = "btnConfig";
            btnConfig.Padding = new Padding(12, 0, 0, 0);
            btnConfig.Size = new Size(200, 50);
            btnConfig.TabIndex = 6;
            btnConfig.Text = "Cấu hình chung";
            btnConfig.TextAlign = ContentAlignment.MiddleLeft;
            btnConfig.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnConfig.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.logo_d2s;
            pictureBox2.Location = new Point(39, 855);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(119, 27);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            // 
            // btnLogOut
            // 
            btnLogOut.Cursor = Cursors.Hand;
            btnLogOut.Dock = DockStyle.Top;
            btnLogOut.FlatAppearance.BorderSize = 0;
            btnLogOut.FlatStyle = FlatStyle.Flat;
            btnLogOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogOut.ForeColor = Color.White;
            btnLogOut.Image = Properties.Resources.padlock_menu;
            btnLogOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogOut.Location = new Point(0, 210);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.Padding = new Padding(12, 0, 0, 0);
            btnLogOut.Size = new Size(200, 50);
            btnLogOut.TabIndex = 4;
            btnLogOut.Text = "Đăng xuất";
            btnLogOut.TextAlign = ContentAlignment.MiddleLeft;
            btnLogOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLogOut.UseVisualStyleBackColor = true;
            btnLogOut.Click += btnLogOut_Click;
            // 
            // btnCheckOut
            // 
            btnCheckOut.Cursor = Cursors.Hand;
            btnCheckOut.Dock = DockStyle.Top;
            btnCheckOut.FlatAppearance.BorderSize = 0;
            btnCheckOut.FlatStyle = FlatStyle.Flat;
            btnCheckOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckOut.ForeColor = Color.White;
            btnCheckOut.Image = Properties.Resources.log_out;
            btnCheckOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.Location = new Point(0, 160);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Padding = new Padding(12, 0, 0, 0);
            btnCheckOut.Size = new Size(200, 50);
            btnCheckOut.TabIndex = 3;
            btnCheckOut.Text = "Quản lý cổng ra";
            btnCheckOut.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckOut.UseVisualStyleBackColor = true;
            btnCheckOut.Click += btnCheckOut_Click;
            // 
            // btnCheckIn
            // 
            btnCheckIn.Cursor = Cursors.Hand;
            btnCheckIn.Dock = DockStyle.Top;
            btnCheckIn.FlatAppearance.BorderSize = 0;
            btnCheckIn.FlatStyle = FlatStyle.Flat;
            btnCheckIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.Image = Properties.Resources.log_in;
            btnCheckIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.Location = new Point(0, 110);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Padding = new Padding(12, 0, 0, 0);
            btnCheckIn.Size = new Size(200, 50);
            btnCheckIn.TabIndex = 2;
            btnCheckIn.Text = "Quản lý cổng vào";
            btnCheckIn.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckIn.UseVisualStyleBackColor = true;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.FromArgb(66, 66, 66);
            btnHome.Cursor = Cursors.Hand;
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHome.ForeColor = Color.White;
            btnHome.Image = Properties.Resources.home_icon2;
            btnHome.ImageAlign = ContentAlignment.MiddleLeft;
            btnHome.Location = new Point(0, 60);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(12, 0, 0, 0);
            btnHome.Size = new Size(200, 50);
            btnHome.TabIndex = 1;
            btnHome.Text = "Trang chủ";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // panelLogo
            // 
            panelLogo.BackColor = Color.FromArgb(66, 66, 66);
            panelLogo.Controls.Add(pictureBox1);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelLogo.Location = new Point(0, 0);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(200, 60);
            panelLogo.TabIndex = 0;
            // 
            // panelMain
            // 
            panelMain.BackgroundImageLayout = ImageLayout.None;
            panelMain.Controls.Add(panelTitle);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelMain.Location = new Point(200, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1400, 900);
            panelMain.TabIndex = 1;
            panelMain.Paint += panelMain_Paint;
            // 
            // panelTitle
            // 
            panelTitle.BackColor = Color.White;
            panelTitle.BackgroundImageLayout = ImageLayout.None;
            panelTitle.Controls.Add(btnUser);
            panelTitle.Controls.Add(label1);
            panelTitle.Controls.Add(labelTitle);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(1400, 60);
            panelTitle.TabIndex = 0;
            panelTitle.Paint += panelTitle_Paint;
            // 
            // btnUser
            // 
            btnUser.BackColor = Color.Transparent;
            btnUser.BackgroundImageLayout = ImageLayout.None;
            btnUser.Dock = DockStyle.Right;
            btnUser.FlatAppearance.BorderColor = Color.White;
            btnUser.FlatAppearance.BorderSize = 0;
            btnUser.FlatStyle = FlatStyle.Flat;
            btnUser.Image = (Image)resources.GetObject("btnUser.Image");
            btnUser.ImageAlign = ContentAlignment.MiddleRight;
            btnUser.Location = new Point(1117, 0);
            btnUser.Name = "btnUser";
            btnUser.Padding = new Padding(0, 0, 12, 0);
            btnUser.Size = new Size(283, 60);
            btnUser.TabIndex = 3;
            btnUser.Text = "Người dùng";
            btnUser.TextAlign = ContentAlignment.MiddleRight;
            btnUser.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnUser.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(25, 20);
            label1.Name = "label1";
            label1.Size = new Size(231, 21);
            label1.TabIndex = 2;
            label1.Text = "HỆ THỐNG XẾP TÀI TỰ ĐỘNG";
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(251, 19);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(88, 21);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "/ Trang chủ";
            labelTitle.Click += labelTitle_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.VCS_3;
            pictureBox1.Location = new Point(56, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(80, 35);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1600, 900);
            Controls.Add(panelMain);
            Controls.Add(panelMenu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống VCS";
            panelMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panelLogo.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            panelTitle.ResumeLayout(false);
            panelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMenu;
        private Panel panelLogo;
        private Button btnHome;
        private Button btnLogOut;
        private Button btnCheckOut;
        private Button btnCheckIn;
        private Panel panelMain;
        private Panel panelTitle;
        private Label labelTitle;
        private Label label1;
        private PictureBox pictureBox2;
        private Button btnUser;
        private Button btnHistory;
        private Button btnConfig;
        private PictureBox pictureBox1;
    }
}