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
            pictureBox2 = new PictureBox();
            label9 = new Label();
            panel8 = new Panel();
            label8 = new Label();
            panel7 = new Panel();
            label7 = new Label();
            label6 = new Label();
            statusDB = new Panel();
            statusSMO = new Panel();
            label5 = new Label();
            panel4 = new Panel();
            comboBox1 = new ComboBox();
            button3 = new Button();
            btnCheckIn = new Button();
            panel3 = new Panel();
            pictureBox1 = new PictureBox();
            txtNumber = new TextBox();
            label4 = new Label();
            button1 = new Button();
            cameraPanel = new Panel();
            videoView = new LibVLCSharp.WinForms.VideoView();
            infoPanel = new Panel();
            txtLicensePlate = new TextBox();
            panel2 = new Panel();
            txtStatus = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblLicensePlate = new Label();
            pictureBoxVehicle = new PictureBox();
            pictureBoxLicensePlate = new PictureBox();
            btnReset = new Button();
            btnDetect = new Button();
            mainPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            cameraPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)videoView).BeginInit();
            infoPanel.SuspendLayout();
            panel2.SuspendLayout();
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
            mainPanel.Margin = new Padding(3, 4, 3, 4);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(23, 27, 23, 27);
            mainPanel.Size = new Size(1607, 1055);
            mainPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(panel8);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(panel7);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(statusDB);
            panel1.Controls.Add(statusSMO);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(btnCheckIn);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(623, 12);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(967, 1096);
            panel1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.refresh;
            pictureBox2.Location = new Point(941, 1061);
            pictureBox2.Margin = new Padding(3, 4, 3, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(18, 21);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 25;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label9.Location = new Point(575, 1061);
            label9.Name = "label9";
            label9.Size = new Size(108, 20);
            label9.TabIndex = 24;
            label9.Text = "Hệ thống TGBX";
            // 
            // panel8
            // 
            panel8.BackColor = Color.DarkGray;
            panel8.Location = new Point(560, 1065);
            panel8.Margin = new Padding(3, 4, 3, 4);
            panel8.Name = "panel8";
            panel8.Size = new Size(11, 13);
            panel8.TabIndex = 23;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label8.Location = new Point(395, 1061);
            label8.Name = "label8";
            label8.Size = new Size(150, 20);
            label8.TabIndex = 22;
            label8.Text = "Hệ thống tự động hoá";
            // 
            // panel7
            // 
            panel7.BackColor = Color.DarkGray;
            panel7.Location = new Point(379, 1065);
            panel7.Margin = new Padding(3, 4, 3, 4);
            panel7.Name = "panel7";
            panel7.Size = new Size(11, 13);
            panel7.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label7.Location = new Point(713, 1061);
            label7.Name = "label7";
            label7.Size = new Size(93, 20);
            label7.TabIndex = 20;
            label7.Text = "Cơ sở dữ liệu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label6.Location = new Point(837, 1061);
            label6.Name = "label6";
            label6.Size = new Size(102, 20);
            label6.TabIndex = 19;
            label6.Text = "Hệ thống SMO";
            // 
            // statusDB
            // 
            statusDB.BackColor = Color.DarkGray;
            statusDB.Location = new Point(699, 1065);
            statusDB.Margin = new Padding(3, 4, 3, 4);
            statusDB.Name = "statusDB";
            statusDB.Size = new Size(11, 13);
            statusDB.TabIndex = 18;
            // 
            // statusSMO
            // 
            statusSMO.BackColor = Color.DarkGray;
            statusSMO.Location = new Point(821, 1065);
            statusSMO.Margin = new Padding(3, 4, 3, 4);
            statusSMO.Name = "statusSMO";
            statusSMO.Size = new Size(11, 13);
            statusSMO.TabIndex = 17;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(491, 99);
            label5.Name = "label5";
            label5.Size = new Size(207, 28);
            label5.TabIndex = 16;
            label5.Text = "Cập nhật phương tiện:";
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(comboBox1);
            panel4.Location = new Point(491, 135);
            panel4.Margin = new Padding(3, 4, 3, 4);
            panel4.Name = "panel4";
            panel4.Size = new Size(453, 53);
            panel4.TabIndex = 14;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.WhiteSmoke;
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.ItemHeight = 35;
            comboBox1.Location = new Point(0, 0);
            comboBox1.Margin = new Padding(0);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(453, 41);
            comboBox1.TabIndex = 16;
            comboBox1.DrawItem += comboBox1_DrawItem;
            comboBox1.SelectedValueChanged += comboBox1_SelectedValueChanged;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(66, 66, 66);
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.White;
            button3.Location = new Point(429, 19);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(110, 53);
            button3.TabIndex = 15;
            button3.Text = "Cập nhật";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // btnCheckIn
            // 
            btnCheckIn.BackColor = Color.FromArgb(66, 66, 66);
            btnCheckIn.Cursor = Cursors.Hand;
            btnCheckIn.FlatStyle = FlatStyle.Flat;
            btnCheckIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.Location = new Point(738, 19);
            btnCheckIn.Margin = new Padding(3, 4, 3, 4);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Size = new Size(207, 53);
            btnCheckIn.TabIndex = 14;
            btnCheckIn.Text = "Cho vào kho - Cấp số";
            btnCheckIn.UseVisualStyleBackColor = false;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(txtNumber);
            panel3.Location = new Point(22, 135);
            panel3.Margin = new Padding(3, 4, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(453, 53);
            panel3.TabIndex = 13;
            // 
            // pictureBox1
            // 
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.search;
            pictureBox1.Location = new Point(421, 15);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(21, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 16;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // txtNumber
            // 
            txtNumber.BackColor = Color.WhiteSmoke;
            txtNumber.BorderStyle = BorderStyle.None;
            txtNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumber.Location = new Point(15, 12);
            txtNumber.Margin = new Padding(3, 4, 3, 4);
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(398, 27);
            txtNumber.TabIndex = 10;
            txtNumber.TextChanged += textBox1_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(25, 99);
            label4.Name = "label4";
            label4.Size = new Size(123, 28);
            label4.TabIndex = 9;
            label4.Text = "Số lệnh xuất:";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(66, 66, 66);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(545, 19);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(186, 53);
            button1.TabIndex = 6;
            button1.Text = "Cho vào hàng chờ";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // cameraPanel
            // 
            cameraPanel.BackColor = Color.White;
            cameraPanel.Controls.Add(videoView);
            cameraPanel.Location = new Point(13, 12);
            cameraPanel.Margin = new Padding(3, 4, 3, 4);
            cameraPanel.Name = "cameraPanel";
            cameraPanel.Size = new Size(600, 427);
            cameraPanel.TabIndex = 0;
            // 
            // videoView
            // 
            videoView.BackColor = Color.Black;
            videoView.Location = new Point(0, 0);
            videoView.Margin = new Padding(0);
            videoView.MediaPlayer = null;
            videoView.Name = "videoView";
            videoView.Size = new Size(600, 427);
            videoView.TabIndex = 0;
            // 
            // infoPanel
            // 
            infoPanel.BackColor = Color.White;
            infoPanel.Controls.Add(txtLicensePlate);
            infoPanel.Controls.Add(panel2);
            infoPanel.Controls.Add(label3);
            infoPanel.Controls.Add(label2);
            infoPanel.Controls.Add(label1);
            infoPanel.Controls.Add(lblLicensePlate);
            infoPanel.Controls.Add(pictureBoxVehicle);
            infoPanel.Controls.Add(pictureBoxLicensePlate);
            infoPanel.Controls.Add(btnReset);
            infoPanel.Controls.Add(btnDetect);
            infoPanel.Location = new Point(14, 448);
            infoPanel.Margin = new Padding(3, 4, 3, 4);
            infoPanel.Name = "infoPanel";
            infoPanel.Padding = new Padding(11, 13, 11, 13);
            infoPanel.Size = new Size(600, 660);
            infoPanel.TabIndex = 1;
            infoPanel.Paint += infoPanel_Paint;
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.BackColor = Color.WhiteSmoke;
            txtLicensePlate.BorderStyle = BorderStyle.None;
            txtLicensePlate.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLicensePlate.Location = new Point(11, 128);
            txtLicensePlate.Margin = new Padding(3, 4, 3, 4);
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.Size = new Size(568, 59);
            txtLicensePlate.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(txtStatus);
            panel2.Location = new Point(13, 557);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(567, 63);
            panel2.TabIndex = 12;
            panel2.Paint += panel2_Paint;
            // 
            // txtStatus
            // 
            txtStatus.AutoSize = true;
            txtStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStatus.ForeColor = Color.WhiteSmoke;
            txtStatus.Location = new Point(14, 16);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(107, 28);
            txtStatus.TabIndex = 0;
            txtStatus.Text = "Thông báo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(16, 521);
            label3.Name = "label3";
            label3.Size = new Size(195, 28);
            label3.TabIndex = 7;
            label3.Text = "Thông báo hệ thống:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(304, 207);
            label2.Name = "label2";
            label2.Size = new Size(142, 28);
            label2.TabIndex = 6;
            label2.Text = "Ảnh nhận diện:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(15, 207);
            label1.Name = "label1";
            label1.Size = new Size(99, 28);
            label1.TabIndex = 5;
            label1.Text = "Ảnh chụp:";
            // 
            // lblLicensePlate
            // 
            lblLicensePlate.AutoSize = true;
            lblLicensePlate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLicensePlate.Location = new Point(15, 91);
            lblLicensePlate.Name = "lblLicensePlate";
            lblLicensePlate.Size = new Size(102, 28);
            lblLicensePlate.TabIndex = 0;
            lblLicensePlate.Text = "Biển số xe:";
            // 
            // pictureBoxVehicle
            // 
            pictureBoxVehicle.BackColor = Color.WhiteSmoke;
            pictureBoxVehicle.Location = new Point(11, 243);
            pictureBoxVehicle.Margin = new Padding(3, 4, 3, 4);
            pictureBoxVehicle.Name = "pictureBoxVehicle";
            pictureBoxVehicle.Size = new Size(282, 264);
            pictureBoxVehicle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxVehicle.TabIndex = 3;
            pictureBoxVehicle.TabStop = false;
            // 
            // pictureBoxLicensePlate
            // 
            pictureBoxLicensePlate.BackColor = Color.WhiteSmoke;
            pictureBoxLicensePlate.Location = new Point(301, 243);
            pictureBoxLicensePlate.Margin = new Padding(3, 4, 3, 4);
            pictureBoxLicensePlate.Name = "pictureBoxLicensePlate";
            pictureBoxLicensePlate.Size = new Size(279, 264);
            pictureBoxLicensePlate.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLicensePlate.TabIndex = 4;
            pictureBoxLicensePlate.TabStop = false;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.FromArgb(66, 66, 66);
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(431, 24);
            btnReset.Margin = new Padding(3, 4, 3, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(149, 53);
            btnReset.TabIndex = 5;
            btnReset.Text = "Reset Camera";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnDetect
            // 
            btnDetect.BackColor = Color.FromArgb(66, 66, 66);
            btnDetect.Cursor = Cursors.Hand;
            btnDetect.FlatStyle = FlatStyle.Flat;
            btnDetect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetect.ForeColor = Color.White;
            btnDetect.Location = new Point(275, 24);
            btnDetect.Margin = new Padding(3, 4, 3, 4);
            btnDetect.Name = "btnDetect";
            btnDetect.Size = new Size(149, 53);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.UseVisualStyleBackColor = false;
            btnDetect.Click += btnDetect_Click;
            // 
            // CheckIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1607, 1055);
            Controls.Add(mainPanel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "CheckIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý cổng vào";
            Load += CheckIn_Load;
            mainPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            cameraPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)videoView).EndInit();
            infoPanel.ResumeLayout(false);
            infoPanel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private TextBox txtNumber;
        private Label label4;
        private TextBox txtLicensePlate;
        private Panel panel2;
        private Label txtStatus;
        private Panel panel3;
        private Button button3;
        private Button button2;
        private Panel panel4;
        private PictureBox pictureBox1;
        private ComboBox comboBox1;
        private Label label5;
        private Panel statusSMO;
        private Label label9;
        private Panel panel8;
        private Label label8;
        private Panel panel7;
        private Label label7;
        private Label label6;
        private Panel statusDB;
        private PictureBox pictureBox2;
        private Button btnCheckIn;
    }
}