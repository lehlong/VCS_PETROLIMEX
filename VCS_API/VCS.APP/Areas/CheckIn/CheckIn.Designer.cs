namespace VCS.APP.Areas.CheckIn
{
    partial class CheckIn
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
            mainPanel = new Panel();
            panel1 = new Panel();
            panel3 = new Panel();
            txtNumber = new TextBox();
            btnCheckNumber = new Button();
            panel2 = new Panel();
            txtStatus = new Label();
            label3 = new Label();
            label4 = new Label();
            button1 = new Button();
            cameraPanel = new Panel();
            videoView = new LibVLCSharp.WinForms.VideoView();
            infoPanel = new Panel();
            txtLicensePlate = new TextBox();
            label2 = new Label();
            label1 = new Label();
            lblLicensePlate = new Label();
            pictureBoxVehicle = new PictureBox();
            pictureBoxLicensePlate = new PictureBox();
            btnReset = new Button();
            btnDetect = new Button();
            mainPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            cameraPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)videoView).BeginInit();
            infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).BeginInit();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mainPanel.Controls.Add(panel1);
            mainPanel.Controls.Add(cameraPanel);
            mainPanel.Controls.Add(infoPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(20);
            mainPanel.Size = new Size(1406, 840);
            mainPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(545, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(846, 822);
            panel1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(txtNumber);
            panel3.Controls.Add(btnCheckNumber);
            panel3.Location = new Point(18, 150);
            panel3.Name = "panel3";
            panel3.Size = new Size(809, 40);
            panel3.TabIndex = 13;
            // 
            // txtNumber
            // 
            txtNumber.BackColor = Color.WhiteSmoke;
            txtNumber.BorderStyle = BorderStyle.None;
            txtNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumber.Location = new Point(12, 10);
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(695, 22);
            txtNumber.TabIndex = 10;
            txtNumber.TextChanged += textBox1_TextChanged;
            // 
            // btnCheckNumber
            // 
            btnCheckNumber.BackColor = SystemColors.ActiveBorder;
            btnCheckNumber.Cursor = Cursors.Hand;
            btnCheckNumber.FlatAppearance.BorderSize = 0;
            btnCheckNumber.FlatStyle = FlatStyle.Flat;
            btnCheckNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckNumber.Location = new Point(713, 0);
            btnCheckNumber.Name = "btnCheckNumber";
            btnCheckNumber.Size = new Size(96, 40);
            btnCheckNumber.TabIndex = 11;
            btnCheckNumber.Text = "Kiểm tra";
            btnCheckNumber.UseVisualStyleBackColor = false;
            btnCheckNumber.Click += btnCheckNumber_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(txtStatus);
            panel2.Location = new Point(18, 76);
            panel2.Name = "panel2";
            panel2.Size = new Size(809, 40);
            panel2.TabIndex = 12;
            panel2.Paint += panel2_Paint;
            // 
            // txtStatus
            // 
            txtStatus.AutoSize = true;
            txtStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStatus.ForeColor = Color.WhiteSmoke;
            txtStatus.Location = new Point(10, 8);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(84, 21);
            txtStatus.TabIndex = 0;
            txtStatus.Text = "Thông báo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(27, 51);
            label3.Name = "label3";
            label3.Size = new Size(153, 21);
            label3.TabIndex = 7;
            label3.Text = "Thông báo hệ thống:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(27, 125);
            label4.Name = "label4";
            label4.Size = new Size(98, 21);
            label4.TabIndex = 9;
            label4.Text = "Số lệnh xuất:";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(24, 144, 255);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(711, 14);
            button1.Name = "button1";
            button1.Size = new Size(116, 40);
            button1.TabIndex = 6;
            button1.Text = "Cho xe vào";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // cameraPanel
            // 
            cameraPanel.BackColor = Color.White;
            cameraPanel.Controls.Add(videoView);
            cameraPanel.Location = new Point(11, 9);
            cameraPanel.Name = "cameraPanel";
            cameraPanel.Size = new Size(525, 320);
            cameraPanel.TabIndex = 0;
            // 
            // videoView
            // 
            videoView.BackColor = Color.Black;
            videoView.Location = new Point(0, 0);
            videoView.Margin = new Padding(0);
            videoView.MediaPlayer = null;
            videoView.Name = "videoView";
            videoView.Size = new Size(525, 320);
            videoView.TabIndex = 0;
            // 
            // infoPanel
            // 
            infoPanel.BackColor = Color.White;
            infoPanel.Controls.Add(txtLicensePlate);
            infoPanel.Controls.Add(label2);
            infoPanel.Controls.Add(label1);
            infoPanel.Controls.Add(lblLicensePlate);
            infoPanel.Controls.Add(pictureBoxVehicle);
            infoPanel.Controls.Add(pictureBoxLicensePlate);
            infoPanel.Controls.Add(btnReset);
            infoPanel.Controls.Add(btnDetect);
            infoPanel.Location = new Point(12, 336);
            infoPanel.Name = "infoPanel";
            infoPanel.Padding = new Padding(10);
            infoPanel.Size = new Size(525, 495);
            infoPanel.TabIndex = 1;
            infoPanel.Paint += infoPanel_Paint;
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.BackColor = Color.WhiteSmoke;
            txtLicensePlate.BorderStyle = BorderStyle.None;
            txtLicensePlate.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLicensePlate.Location = new Point(10, 87);
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.Size = new Size(497, 47);
            txtLicensePlate.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(260, 148);
            label2.Name = "label2";
            label2.Size = new Size(114, 21);
            label2.TabIndex = 6;
            label2.Text = "Ảnh nhận diện:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(8, 148);
            label1.Name = "label1";
            label1.Size = new Size(79, 21);
            label1.TabIndex = 5;
            label1.Text = "Ảnh chụp:";
            // 
            // lblLicensePlate
            // 
            lblLicensePlate.AutoSize = true;
            lblLicensePlate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLicensePlate.Location = new Point(6, 60);
            lblLicensePlate.Name = "lblLicensePlate";
            lblLicensePlate.Size = new Size(82, 21);
            lblLicensePlate.TabIndex = 0;
            lblLicensePlate.Text = "Biển số xe:";
            // 
            // pictureBoxVehicle
            // 
            pictureBoxVehicle.BackColor = Color.WhiteSmoke;
            pictureBoxVehicle.Location = new Point(10, 170);
            pictureBoxVehicle.Name = "pictureBoxVehicle";
            pictureBoxVehicle.Size = new Size(247, 219);
            pictureBoxVehicle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxVehicle.TabIndex = 3;
            pictureBoxVehicle.TabStop = false;
            // 
            // pictureBoxLicensePlate
            // 
            pictureBoxLicensePlate.BackColor = Color.WhiteSmoke;
            pictureBoxLicensePlate.Location = new Point(263, 170);
            pictureBoxLicensePlate.Name = "pictureBoxLicensePlate";
            pictureBoxLicensePlate.Size = new Size(244, 219);
            pictureBoxLicensePlate.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLicensePlate.TabIndex = 4;
            pictureBoxLicensePlate.TabStop = false;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.FromArgb(24, 144, 255);
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(377, 13);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(130, 40);
            btnReset.TabIndex = 5;
            btnReset.Text = "Reset Camera";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnDetect
            // 
            btnDetect.BackColor = Color.FromArgb(24, 144, 255);
            btnDetect.Cursor = Cursors.Hand;
            btnDetect.FlatStyle = FlatStyle.Flat;
            btnDetect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetect.ForeColor = Color.White;
            btnDetect.Location = new Point(241, 13);
            btnDetect.Name = "btnDetect";
            btnDetect.Size = new Size(130, 40);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.UseVisualStyleBackColor = false;
            btnDetect.Click += btnDetect_Click;
            // 
            // CheckIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1406, 840);
            Controls.Add(mainPanel);
            Name = "CheckIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý cổng vào";
            Load += CheckIn_Load;
            mainPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            cameraPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)videoView).EndInit();
            infoPanel.ResumeLayout(false);
            infoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).EndInit();
            ResumeLayout(false);
        }
        #endregion
        private Panel mainPanel;
        private Panel cameraPanel;
        private LibVLCSharp.WinForms.VideoView videoView;
        private Label lblCameraTitle;
        private Panel infoPanel;
        private Label lblLicensePlate;
        private Button btnDetect;
        private PictureBox pictureBoxVehicle;
        private PictureBox pictureBoxLicensePlate;
        private Button btnReset;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Label label3;
        private Button button1;
        private Button btnCheckNumber;
        private TextBox txtNumber;
        private Label label4;
        private TextBox txtLicensePlate;
        private Panel panel2;
        private Label txtStatus;
        private Panel panel3;
    }
}