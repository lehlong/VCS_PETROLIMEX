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
            
            // Panel chứa camera
            cameraPanel = new Panel();
            videoView = new LibVLCSharp.WinForms.VideoView();
            lblCameraTitle = new Label();

            // Panel chứa thông tin xe
            infoPanel = new Panel();
            lblLicensePlate = new Label();
            txtLicensePlate = new TextBox();
            btnDetect = new Button();
            pictureBoxVehicle = new PictureBox();
            pictureBoxLicensePlate = new PictureBox();
            btnReset = new Button();

            // Cấu hình Layout
            mainPanel.SuspendLayout();
            cameraPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)videoView).BeginInit();
            infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).BeginInit();
            SuspendLayout();

            // mainPanel
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            mainPanel.Controls.Add(cameraPanel);
            mainPanel.Controls.Add(infoPanel);

            // cameraPanel
            cameraPanel.Width = 800;
            cameraPanel.Height = 600;
            cameraPanel.BackColor = Color.White;
            cameraPanel.Padding = new Padding(10);
            cameraPanel.Controls.Add(lblCameraTitle);
            cameraPanel.Controls.Add(videoView);

            // lblCameraTitle
            lblCameraTitle.Text = "Camera Cổng Vào";
            lblCameraTitle.Dock = DockStyle.Top;
            lblCameraTitle.Height = 30;
            lblCameraTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCameraTitle.TextAlign = ContentAlignment.MiddleCenter;

            // videoView
            videoView.BackColor = Color.Black;
            videoView.Dock = DockStyle.Fill;
            videoView.Location = new Point(10, 40);
            videoView.MediaPlayer = null;
            videoView.TabIndex = 0;

            // infoPanel
            infoPanel.Width = 400;
            infoPanel.Dock = DockStyle.Right;
            infoPanel.BackColor = Color.White;
            infoPanel.Padding = new Padding(10);

            // lblLicensePlate
            lblLicensePlate.Text = "Biển số xe:";
            lblLicensePlate.AutoSize = true;
            lblLicensePlate.Location = new Point(10, 20);
            lblLicensePlate.Font = new Font("Segoe UI", 10F);

            // txtLicensePlate
            txtLicensePlate.Location = new Point(10, 45);
            txtLicensePlate.Width = 200;
            txtLicensePlate.Height = 30;
            txtLicensePlate.Font = new Font("Segoe UI", 12F);
            txtLicensePlate.ReadOnly = true;

            // btnDetect
            btnDetect.Text = "Nhận diện biển số";
            btnDetect.Location = new Point(220, 45);
            btnDetect.Width = 150;
            btnDetect.Height = 30;
            btnDetect.BackColor = Color.FromArgb(24, 144, 255);
            btnDetect.ForeColor = Color.White;
            btnDetect.FlatStyle = FlatStyle.Flat;
            btnDetect.Click += btnDetect_Click;

            // pictureBoxVehicle
            pictureBoxVehicle.Location = new Point(10, 90);
            pictureBoxVehicle.Width = 360;
            pictureBoxVehicle.Height = 240;
            pictureBoxVehicle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxVehicle.BorderStyle = BorderStyle.FixedSingle;

            // pictureBoxLicensePlate
            pictureBoxLicensePlate.Location = new Point(10, 340);
            pictureBoxLicensePlate.Width = 360;
            pictureBoxLicensePlate.Height = 120;
            pictureBoxLicensePlate.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLicensePlate.BorderStyle = BorderStyle.FixedSingle;

            // btnReset
            btnReset.Text = "Reset Camera";
            btnReset.Location = new Point(10, 470);
            btnReset.Width = 120;
            btnReset.Height = 35;
            btnReset.BackColor = Color.FromArgb(24, 144, 255);
            btnReset.ForeColor = Color.White;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Click += btnReset_Click;

            // Thêm controls vào infoPanel
            infoPanel.Controls.Add(lblLicensePlate);
            infoPanel.Controls.Add(txtLicensePlate);
            infoPanel.Controls.Add(btnDetect);
            infoPanel.Controls.Add(pictureBoxVehicle);
            infoPanel.Controls.Add(pictureBoxLicensePlate);
            infoPanel.Controls.Add(btnReset);

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(mainPanel);
            Name = "CheckIn";
            Text = "Quản lý cổng vào";
            StartPosition = FormStartPosition.CenterScreen;

            mainPanel.ResumeLayout(false);
            cameraPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)videoView).EndInit();
            infoPanel.ResumeLayout(false);
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
    }
}