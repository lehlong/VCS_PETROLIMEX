namespace VCS.Areas.Login
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            btnLogin = new Button();
            password = new TextBox();
            username = new TextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            label10 = new Label();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(13, 92, 171);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(391, 277);
            btnLogin.Margin = new Padding(4);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(237, 40);
            btnLogin.TabIndex = 11;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // password
            // 
            password.BackColor = Color.WhiteSmoke;
            password.BorderStyle = BorderStyle.None;
            password.Location = new Point(36, 7);
            password.Margin = new Padding(4);
            password.Name = "password";
            password.Size = new Size(196, 22);
            password.TabIndex = 10;
            password.UseSystemPasswordChar = true;
            // 
            // username
            // 
            username.BackColor = Color.WhiteSmoke;
            username.BorderStyle = BorderStyle.None;
            username.Location = new Point(36, 7);
            username.Margin = new Padding(4);
            username.Name = "username";
            username.Size = new Size(196, 22);
            username.TabIndex = 9;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.logo_login;
            pictureBox1.Location = new Point(430, 28);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(159, 117);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.Logo_Petrolimex_PLX;
            pictureBox2.Location = new Point(109, 162);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(127, 113);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 17;
            pictureBox2.TabStop = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.DarkGray;
            label10.Location = new Point(388, 429);
            label10.Name = "label10";
            label10.Size = new Size(252, 15);
            label10.TabIndex = 18;
            label10.Text = "Copyright © 2025 Dynamic Software Solutions";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.icons8_user_18;
            pictureBox3.Location = new Point(9, 10);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(18, 18);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 19;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.icons8_key_18;
            pictureBox4.Location = new Point(9, 9);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(18, 18);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 20;
            pictureBox4.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(pictureBox3);
            panel1.Controls.Add(username);
            panel1.Location = new Point(391, 173);
            panel1.Name = "panel1";
            panel1.Size = new Size(237, 36);
            panel1.TabIndex = 21;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(pictureBox4);
            panel2.Controls.Add(password);
            panel2.Location = new Point(391, 221);
            panel2.Name = "panel2";
            panel2.Size = new Size(237, 36);
            panel2.TabIndex = 22;
            // 
            // panel3
            // 
            panel3.BackColor = Color.AliceBlue;
            panel3.BackgroundImage = Properties.Resources.bg_auth;
            panel3.BackgroundImageLayout = ImageLayout.Stretch;
            panel3.Controls.Add(pictureBox2);
            panel3.Location = new Point(-1, -1);
            panel3.Name = "panel3";
            panel3.Size = new Size(340, 465);
            panel3.TabIndex = 23;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(675, 461);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label10);
            Controls.Add(btnLogin);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống VCS";
            FormClosed += Login_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnLogin;
        private TextBox password;
        private TextBox username;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label label10;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
    }
}