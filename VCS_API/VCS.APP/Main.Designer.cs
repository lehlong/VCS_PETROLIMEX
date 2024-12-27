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
            pictureBox2 = new PictureBox();
            btnLogOut = new Button();
            btnCheckOut = new Button();
            btnCheckIn = new Button();
            btnHome = new Button();
            panelLogo = new Panel();
            logoText = new Label();
            panelMain = new Panel();
            panelTitle = new Panel();
            btnUser = new Button();
            label1 = new Label();
            labelTitle = new Label();
            panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelTitle.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(66, 66, 66);
            panelMenu.Controls.Add(pictureBox2);
            panelMenu.Controls.Add(btnLogOut);
            panelMenu.Controls.Add(btnCheckOut);
            panelMenu.Controls.Add(btnCheckIn);
            panelMenu.Controls.Add(btnHome);
            panelMenu.Controls.Add(panelLogo);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(200, 900);
            panelMenu.TabIndex = 0;
            panelMenu.Paint += panelMenu_Paint;
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
            btnLogOut.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogOut.ForeColor = Color.White;
            btnLogOut.Image = Properties.Resources.padlock_menu;
            btnLogOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogOut.Location = new Point(0, 164);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.Padding = new Padding(12, 0, 0, 0);
            btnLogOut.Size = new Size(200, 40);
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
            btnCheckOut.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckOut.ForeColor = Color.White;
            btnCheckOut.Image = Properties.Resources.log_out;
            btnCheckOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.Location = new Point(0, 124);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Padding = new Padding(12, 0, 0, 0);
            btnCheckOut.Size = new Size(200, 40);
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
            btnCheckIn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.Image = Properties.Resources.log_in;
            btnCheckIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.Location = new Point(0, 84);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Padding = new Padding(12, 0, 0, 0);
            btnCheckIn.Size = new Size(200, 40);
            btnCheckIn.TabIndex = 2;
            btnCheckIn.Text = "Quản lý cổng vào";
            btnCheckIn.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckIn.UseVisualStyleBackColor = true;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // btnHome
            // 
            btnHome.Cursor = Cursors.Hand;
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHome.ForeColor = Color.White;
            btnHome.Image = Properties.Resources.home_icon2;
            btnHome.ImageAlign = ContentAlignment.MiddleLeft;
            btnHome.Location = new Point(0, 44);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(12, 0, 0, 0);
            btnHome.Size = new Size(200, 40);
            btnHome.TabIndex = 1;
            btnHome.Text = "Trang chủ";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHome.UseVisualStyleBackColor = true;
            btnHome.Click += btnHome_Click;
            // 
            // panelLogo
            // 
            panelLogo.Controls.Add(logoText);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(200, 44);
            panelLogo.TabIndex = 0;
            // 
            // logoText
            // 
            logoText.AutoSize = true;
            logoText.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            logoText.ForeColor = Color.White;
            logoText.Location = new Point(17, 12);
            logoText.Name = "logoText";
            logoText.Size = new Size(40, 21);
            logoText.TabIndex = 0;
            logoText.Text = "VSC";
            // 
            // panelMain
            // 
            panelMain.BackgroundImageLayout = ImageLayout.None;
            panelMain.Controls.Add(panelTitle);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(200, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1400, 900);
            panelMain.TabIndex = 1;
            // 
            // panelTitle
            // 
            panelTitle.BackColor = Color.White;
            panelTitle.BackgroundImageLayout = ImageLayout.None;
            panelTitle.Controls.Add(btnUser);
            panelTitle.Controls.Add(label1);
            panelTitle.Controls.Add(labelTitle);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(1400, 44);
            panelTitle.TabIndex = 0;
            panelTitle.Paint += panelTitle_Paint;
            // 
            // btnUser
            // 
            btnUser.BackColor = Color.Transparent;
            btnUser.BackgroundImageLayout = ImageLayout.None;
            btnUser.Cursor = Cursors.Hand;
            btnUser.Dock = DockStyle.Right;
            btnUser.FlatAppearance.BorderColor = Color.White;
            btnUser.FlatAppearance.BorderSize = 0;
            btnUser.FlatStyle = FlatStyle.Flat;
            btnUser.Image = (Image)resources.GetObject("btnUser.Image");
            btnUser.ImageAlign = ContentAlignment.MiddleRight;
            btnUser.Location = new Point(1222, 0);
            btnUser.Name = "btnUser";
            btnUser.Padding = new Padding(0, 0, 12, 0);
            btnUser.Size = new Size(178, 44);
            btnUser.TabIndex = 3;
            btnUser.Text = "Người dùng";
            btnUser.TextAlign = ContentAlignment.MiddleRight;
            btnUser.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnUser.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(25, 14);
            label1.Name = "label1";
            label1.Size = new Size(171, 15);
            label1.TabIndex = 2;
            label1.Text = "HỆ THỐNG XẾP TÀI TỰ ĐỘNG";
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(196, 13);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(67, 15);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "/ Trang chủ";
            labelTitle.Click += labelTitle_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1600, 900);
            Controls.Add(panelMain);
            Controls.Add(panelMenu);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            panelMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelTitle.ResumeLayout(false);
            panelTitle.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMenu;
        private Panel panelLogo;
        private Label logoText;
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
    }
}