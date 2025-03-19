namespace VCS.APP.Areas.ConfigApp
{
    partial class ConfigApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigApp));
            panel1 = new Panel();
            isStarup = new CheckBox();
            panel8 = new Panel();
            txtTimeService = new TextBox();
            Lable7 = new Label();
            panel7 = new Panel();
            txtCropHeight = new TextBox();
            label6 = new Label();
            panel6 = new Panel();
            txtPathSaveFile = new TextBox();
            panel5 = new Panel();
            txtCropWidth = new TextBox();
            panel4 = new Panel();
            txtSmoApiPassword = new TextBox();
            panel2 = new Panel();
            txtSmoApiUsername = new TextBox();
            panel3 = new Panel();
            txtSmoApiUrl = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            panel1.SuspendLayout();
            panel8.SuspendLayout();
            panel7.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(isStarup);
            panel1.Controls.Add(panel8);
            panel1.Controls.Add(Lable7);
            panel1.Controls.Add(panel7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(6, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(1334, 770);
            panel1.TabIndex = 0;
            // 
            // isStarup
            // 
            isStarup.AutoSize = true;
            isStarup.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            isStarup.Location = new Point(466, 361);
            isStarup.Name = "isStarup";
            isStarup.Size = new Size(201, 25);
            isStarup.TabIndex = 47;
            isStarup.Text = "Khởi động cùng Window";
            isStarup.UseVisualStyleBackColor = true;
            isStarup.CheckedChanged += isStarup_CheckedChanged;
            // 
            // panel8
            // 
            panel8.BackColor = Color.WhiteSmoke;
            panel8.Controls.Add(txtTimeService);
            panel8.Location = new Point(23, 352);
            panel8.Name = "panel8";
            panel8.Size = new Size(420, 40);
            panel8.TabIndex = 46;
            // 
            // txtTimeService
            // 
            txtTimeService.BackColor = Color.WhiteSmoke;
            txtTimeService.BorderStyle = BorderStyle.None;
            txtTimeService.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTimeService.Location = new Point(13, 9);
            txtTimeService.Name = "txtTimeService";
            txtTimeService.Size = new Size(390, 22);
            txtTimeService.TabIndex = 10;
            // 
            // Lable7
            // 
            Lable7.AutoSize = true;
            Lable7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Lable7.Location = new Point(26, 329);
            Lable7.Name = "Lable7";
            Lable7.Size = new Size(123, 21);
            Lable7.TabIndex = 45;
            Lable7.Text = "Đồng bộ  (phút):";
            // 
            // panel7
            // 
            panel7.BackColor = Color.WhiteSmoke;
            panel7.Controls.Add(txtCropHeight);
            panel7.Location = new Point(466, 186);
            panel7.Name = "panel7";
            panel7.Size = new Size(420, 40);
            panel7.TabIndex = 44;
            // 
            // txtCropHeight
            // 
            txtCropHeight.BackColor = Color.WhiteSmoke;
            txtCropHeight.BorderStyle = BorderStyle.None;
            txtCropHeight.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCropHeight.Location = new Point(13, 9);
            txtCropHeight.Name = "txtCropHeight";
            txtCropHeight.Size = new Size(390, 22);
            txtCropHeight.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(469, 163);
            label6.Name = "label6";
            label6.Size = new Size(111, 21);
            label6.TabIndex = 43;
            label6.Text = "Chiều cao ảnh:";
            // 
            // panel6
            // 
            panel6.BackColor = Color.WhiteSmoke;
            panel6.Controls.Add(txtPathSaveFile);
            panel6.Location = new Point(466, 269);
            panel6.Name = "panel6";
            panel6.Size = new Size(420, 40);
            panel6.TabIndex = 42;
            // 
            // txtPathSaveFile
            // 
            txtPathSaveFile.BackColor = Color.WhiteSmoke;
            txtPathSaveFile.BorderStyle = BorderStyle.None;
            txtPathSaveFile.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPathSaveFile.Location = new Point(13, 9);
            txtPathSaveFile.Name = "txtPathSaveFile";
            txtPathSaveFile.Size = new Size(390, 22);
            txtPathSaveFile.TabIndex = 10;
            // 
            // panel5
            // 
            panel5.BackColor = Color.WhiteSmoke;
            panel5.Controls.Add(txtCropWidth);
            panel5.Location = new Point(466, 103);
            panel5.Name = "panel5";
            panel5.Size = new Size(420, 40);
            panel5.TabIndex = 42;
            // 
            // txtCropWidth
            // 
            txtCropWidth.BackColor = Color.WhiteSmoke;
            txtCropWidth.BorderStyle = BorderStyle.None;
            txtCropWidth.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCropWidth.Location = new Point(13, 9);
            txtCropWidth.Name = "txtCropWidth";
            txtCropWidth.Size = new Size(390, 22);
            txtCropWidth.TabIndex = 10;
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(txtSmoApiPassword);
            panel4.Location = new Point(23, 269);
            panel4.Name = "panel4";
            panel4.Size = new Size(420, 40);
            panel4.TabIndex = 42;
            // 
            // txtSmoApiPassword
            // 
            txtSmoApiPassword.BackColor = Color.WhiteSmoke;
            txtSmoApiPassword.BorderStyle = BorderStyle.None;
            txtSmoApiPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSmoApiPassword.Location = new Point(13, 9);
            txtSmoApiPassword.Name = "txtSmoApiPassword";
            txtSmoApiPassword.Size = new Size(390, 22);
            txtSmoApiPassword.TabIndex = 10;
            txtSmoApiPassword.UseSystemPasswordChar = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(txtSmoApiUsername);
            panel2.Location = new Point(23, 186);
            panel2.Name = "panel2";
            panel2.Size = new Size(420, 40);
            panel2.TabIndex = 42;
            // 
            // txtSmoApiUsername
            // 
            txtSmoApiUsername.BackColor = Color.WhiteSmoke;
            txtSmoApiUsername.BorderStyle = BorderStyle.None;
            txtSmoApiUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSmoApiUsername.Location = new Point(13, 9);
            txtSmoApiUsername.Name = "txtSmoApiUsername";
            txtSmoApiUsername.Size = new Size(390, 22);
            txtSmoApiUsername.TabIndex = 10;
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(txtSmoApiUrl);
            panel3.Location = new Point(23, 103);
            panel3.Name = "panel3";
            panel3.Size = new Size(420, 40);
            panel3.TabIndex = 41;
            // 
            // txtSmoApiUrl
            // 
            txtSmoApiUrl.BackColor = Color.WhiteSmoke;
            txtSmoApiUrl.BorderStyle = BorderStyle.None;
            txtSmoApiUrl.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSmoApiUrl.Location = new Point(13, 9);
            txtSmoApiUrl.Name = "txtSmoApiUrl";
            txtSmoApiUrl.Size = new Size(390, 22);
            txtSmoApiUrl.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(469, 246);
            label5.Name = "label5";
            label5.Size = new Size(147, 21);
            label5.TabIndex = 39;
            label5.Text = "Đường dẫn lưu ảnh:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(469, 80);
            label4.Name = "label4";
            label4.Size = new Size(120, 21);
            label4.TabIndex = 38;
            label4.Text = "Chiều rộng ảnh:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(26, 246);
            label3.Name = "label3";
            label3.Size = new Size(145, 21);
            label3.TabIndex = 37;
            label3.Text = "SMO API Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(26, 163);
            label2.Name = "label2";
            label2.Size = new Size(150, 21);
            label2.TabIndex = 36;
            label2.Text = "SMO API Username:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(26, 80);
            label1.Name = "label1";
            label1.Size = new Size(100, 21);
            label1.TabIndex = 35;
            label1.Text = "SMO API Url:";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(40, 167, 69);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(22, 22);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Padding = new Padding(6, 0, 0, 0);
            button1.Size = new Size(172, 40);
            button1.TabIndex = 34;
            button1.Text = "Cập nhật thông tin";
            button1.TextAlign = ContentAlignment.MiddleLeft;
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // ConfigApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1344, 781);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ConfigApp";
            Text = "ConfigApp";
            Load += ConfigApp_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Panel panel3;
        private TextBox txtSmoApiUrl;
        private Panel panel6;
        private TextBox txtPathSaveFile;
        private Panel panel5;
        private TextBox txtCropWidth;
        private Panel panel4;
        private TextBox txtSmoApiPassword;
        private Panel panel2;
        private TextBox txtSmoApiUsername;
        private Panel panel7;
        private TextBox txtCropHeight;
        private Label label6;
        private Panel panel8;
        private TextBox txtTimeService;
        private Label Lable7;
        private CheckBox isStarup;
    }
}