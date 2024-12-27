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
            btnCheckNumber = new Button();
            txtNumber = new TextBox();
            label4 = new Label();
            button1 = new Button();
            btnReset = new Button();
            btnDetect = new Button();
            cameraPanel = new Panel();
            videoView = new LibVLCSharp.WinForms.VideoView();
            infoPanel = new Panel();
            txtStatus = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblLicensePlate = new Label();
            txtLicensePlate = new TextBox();
            pictureBoxVehicle = new PictureBox();
            pictureBoxLicensePlate = new PictureBox();
            mainPanel.SuspendLayout();
            panel1.SuspendLayout();
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
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnCheckNumber);
            panel1.Controls.Add(txtNumber);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnReset);
            panel1.Controls.Add(btnDetect);
            panel1.Location = new Point(545, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(846, 822);
            panel1.TabIndex = 2;
            // 
            // btnCheckNumber
            // 
            btnCheckNumber.Cursor = Cursors.Hand;
            btnCheckNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckNumber.Location = new Point(741, 86);
            btnCheckNumber.Name = "btnCheckNumber";
            btnCheckNumber.Size = new Size(96, 31);
            btnCheckNumber.TabIndex = 11;
            btnCheckNumber.Text = "Kiểm tra";
            btnCheckNumber.UseVisualStyleBackColor = true;
            btnCheckNumber.Click += btnCheckNumber_Click;
            // 
            // txtNumber
            // 
            txtNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumber.Location = new Point(17, 87);
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(787, 29);
            txtNumber.TabIndex = 10;
            txtNumber.TextChanged += textBox1_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(20, 65);
            label4.Name = "label4";
            label4.Size = new Size(87, 19);
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
            button1.Location = new Point(449, 9);
            button1.Name = "button1";
            button1.Size = new Size(116, 40);
            button1.TabIndex = 6;
            button1.Text = "Cho xe vào";
            button1.UseVisualStyleBackColor = false;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.FromArgb(24, 144, 255);
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(707, 9);
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
            btnDetect.Location = new Point(571, 9);
            btnDetect.Name = "btnDetect";
            btnDetect.Size = new Size(130, 40);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.UseVisualStyleBackColor = false;
            btnDetect.Click += btnDetect_Click;
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
            infoPanel.Controls.Add(txtStatus);
            infoPanel.Controls.Add(label3);
            infoPanel.Controls.Add(label2);
            infoPanel.Controls.Add(label1);
            infoPanel.Controls.Add(lblLicensePlate);
            infoPanel.Controls.Add(txtLicensePlate);
            infoPanel.Controls.Add(pictureBoxVehicle);
            infoPanel.Controls.Add(pictureBoxLicensePlate);
            infoPanel.Location = new Point(12, 336);
            infoPanel.Name = "infoPanel";
            infoPanel.Padding = new Padding(10);
            infoPanel.Size = new Size(525, 495);
            infoPanel.TabIndex = 1;
            infoPanel.Paint += infoPanel_Paint;
            // 
            // txtStatus
            // 
            txtStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStatus.Location = new Point(10, 30);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(500, 29);
            txtStatus.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(13, 8);
            label3.Name = "label3";
            label3.Size = new Size(82, 21);
            label3.TabIndex = 7;
            label3.Text = "Trạng thái:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(263, 156);
            label2.Name = "label2";
            label2.Size = new Size(114, 21);
            label2.TabIndex = 6;
            label2.Text = "Ảnh nhận diện:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(13, 156);
            label1.Name = "label1";
            label1.Size = new Size(79, 21);
            label1.TabIndex = 5;
            label1.Text = "Ảnh chụp:";
            // 
            // lblLicensePlate
            // 
            lblLicensePlate.AutoSize = true;
            lblLicensePlate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLicensePlate.Location = new Point(13, 68);
            lblLicensePlate.Name = "lblLicensePlate";
            lblLicensePlate.Size = new Size(82, 21);
            lblLicensePlate.TabIndex = 0;
            lblLicensePlate.Text = "Biển số xe:";
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLicensePlate.Location = new Point(10, 90);
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.ReadOnly = true;
            txtLicensePlate.Size = new Size(499, 57);
            txtLicensePlate.TabIndex = 1;
            // 
            // pictureBoxVehicle
            // 
            pictureBoxVehicle.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxVehicle.Location = new Point(10, 178);
            pictureBoxVehicle.Name = "pictureBoxVehicle";
            pictureBoxVehicle.Size = new Size(247, 219);
            pictureBoxVehicle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxVehicle.TabIndex = 3;
            pictureBoxVehicle.TabStop = false;
            // 
            // pictureBoxLicensePlate
            // 
            pictureBoxLicensePlate.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxLicensePlate.Location = new Point(263, 178);
            pictureBoxLicensePlate.Name = "pictureBoxLicensePlate";
            pictureBoxLicensePlate.Size = new Size(244, 219);
            pictureBoxLicensePlate.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLicensePlate.TabIndex = 4;
            pictureBoxLicensePlate.TabStop = false;
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
        private TextBox txtLicensePlate;
        private Button btnDetect;
        private PictureBox pictureBoxVehicle;
        private PictureBox pictureBoxLicensePlate;
        private Button btnReset;
        private Label label1;
        private Label label2;
        private TextBox txtStatus;
        private Label label3;
        private Panel panel1;
        private Button button1;
        private Button btnCheckNumber;
        private TextBox txtNumber;
        private Label label4;
    }
}