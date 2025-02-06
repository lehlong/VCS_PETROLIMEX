namespace VCS.APP.Areas.CheckOut
{
    partial class CheckOut
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
            pictureBox1 = new PictureBox();
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
            txtNumber = new TextBox();
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
            btnCheck = new Button();
            btnCheckOut = new Button();
            label4 = new Label();
            txtStatus = new Label();
            label3 = new Label();
            panel2 = new Panel();
            panel5 = new Panel();
            mainPanel = new Panel();
            panel1 = new Panel();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            cameraPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)videoView).BeginInit();
            infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel5.SuspendLayout();
            mainPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.search;
            pictureBox1.Location = new Point(368, 11);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(18, 18);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 16;
            pictureBox1.TabStop = false;
            // 
            // cameraPanel
            // 
            cameraPanel.BackColor = Color.White;
            cameraPanel.Controls.Add(videoView);
            cameraPanel.Location = new Point(12, 110);
            cameraPanel.Name = "cameraPanel";
            cameraPanel.Size = new Size(525, 320);
            cameraPanel.TabIndex = 0;
            // 
            // videoView
            // 
            videoView.BackColor = Color.Black;
            videoView.Location = new Point(11, 11);
            videoView.Margin = new Padding(0);
            videoView.MediaPlayer = null;
            videoView.Name = "videoView";
            videoView.Size = new Size(502, 298);
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
            infoPanel.Location = new Point(12, 438);
            infoPanel.Name = "infoPanel";
            infoPanel.Padding = new Padding(10);
            infoPanel.Size = new Size(525, 393);
            infoPanel.TabIndex = 1;
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.BackColor = Color.WhiteSmoke;
            txtLicensePlate.BorderStyle = BorderStyle.None;
            txtLicensePlate.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLicensePlate.Location = new Point(13, 88);
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.Size = new Size(497, 47);
            txtLicensePlate.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(269, 149);
            label2.Name = "label2";
            label2.Size = new Size(114, 21);
            label2.TabIndex = 6;
            label2.Text = "Ảnh nhận diện:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 149);
            label1.Name = "label1";
            label1.Size = new Size(79, 21);
            label1.TabIndex = 5;
            label1.Text = "Ảnh chụp:";
            // 
            // lblLicensePlate
            // 
            lblLicensePlate.AutoSize = true;
            lblLicensePlate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLicensePlate.Location = new Point(16, 62);
            lblLicensePlate.Name = "lblLicensePlate";
            lblLicensePlate.Size = new Size(82, 21);
            lblLicensePlate.TabIndex = 0;
            lblLicensePlate.Text = "Biển số xe:";
            // 
            // pictureBoxVehicle
            // 
            pictureBoxVehicle.BackColor = Color.WhiteSmoke;
            pictureBoxVehicle.Location = new Point(13, 174);
            pictureBoxVehicle.Name = "pictureBoxVehicle";
            pictureBoxVehicle.Size = new Size(247, 198);
            pictureBoxVehicle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxVehicle.TabIndex = 3;
            pictureBoxVehicle.TabStop = false;
            // 
            // pictureBoxLicensePlate
            // 
            pictureBoxLicensePlate.BackColor = Color.WhiteSmoke;
            pictureBoxLicensePlate.Location = new Point(266, 174);
            pictureBoxLicensePlate.Name = "pictureBoxLicensePlate";
            pictureBoxLicensePlate.Size = new Size(244, 198);
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
            btnReset.Location = new Point(380, 17);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(130, 40);
            btnReset.TabIndex = 5;
            btnReset.Text = "Reset Camera";
            btnReset.UseVisualStyleBackColor = false;
            // 
            // btnDetect
            // 
            btnDetect.BackColor = Color.FromArgb(66, 66, 66);
            btnDetect.Cursor = Cursors.Hand;
            btnDetect.FlatStyle = FlatStyle.Flat;
            btnDetect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetect.ForeColor = Color.White;
            btnDetect.Location = new Point(244, 17);
            btnDetect.Name = "btnDetect";
            btnDetect.Size = new Size(130, 40);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.UseVisualStyleBackColor = false;
            // 
            // txtNumber
            // 
            txtNumber.BackColor = Color.WhiteSmoke;
            txtNumber.BorderStyle = BorderStyle.None;
            txtNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumber.Location = new Point(13, 9);
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(348, 22);
            txtNumber.TabIndex = 10;
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.refresh;
            pictureBox2.Location = new Point(823, 796);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(16, 16);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 25;
            pictureBox2.TabStop = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label9.Location = new Point(503, 796);
            label9.Name = "label9";
            label9.Size = new Size(87, 15);
            label9.TabIndex = 24;
            label9.Text = "Hệ thống TGBX";
            // 
            // panel8
            // 
            panel8.BackColor = Color.DarkGray;
            panel8.Location = new Point(490, 799);
            panel8.Name = "panel8";
            panel8.Size = new Size(10, 10);
            panel8.TabIndex = 23;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label8.Location = new Point(346, 796);
            label8.Name = "label8";
            label8.Size = new Size(123, 15);
            label8.TabIndex = 22;
            label8.Text = "Hệ thống tự động hoá";
            // 
            // panel7
            // 
            panel7.BackColor = Color.DarkGray;
            panel7.Location = new Point(332, 799);
            panel7.Name = "panel7";
            panel7.Size = new Size(10, 10);
            panel7.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label7.Location = new Point(624, 796);
            label7.Name = "label7";
            label7.Size = new Size(76, 15);
            label7.TabIndex = 20;
            label7.Text = "Cơ sở dữ liệu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label6.Location = new Point(732, 796);
            label6.Name = "label6";
            label6.Size = new Size(84, 15);
            label6.TabIndex = 19;
            label6.Text = "Hệ thống SMO";
            // 
            // statusDB
            // 
            statusDB.BackColor = Color.DarkGray;
            statusDB.Location = new Point(612, 799);
            statusDB.Name = "statusDB";
            statusDB.Size = new Size(10, 10);
            statusDB.TabIndex = 18;
            // 
            // statusSMO
            // 
            statusSMO.BackColor = Color.DarkGray;
            statusSMO.Location = new Point(718, 799);
            statusSMO.Name = "statusSMO";
            statusSMO.Size = new Size(10, 10);
            statusSMO.TabIndex = 17;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(430, 74);
            label5.Name = "label5";
            label5.Size = new Size(153, 21);
            label5.TabIndex = 16;
            label5.Text = "Phương tiện chưa ra:";
            label5.Click += label5_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(comboBox1);
            panel4.Location = new Point(430, 101);
            panel4.Name = "panel4";
            panel4.Size = new Size(396, 40);
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
            comboBox1.Size = new Size(396, 41);
            comboBox1.TabIndex = 16;
            // 
            // btnCheck
            // 
            btnCheck.BackColor = Color.FromArgb(66, 66, 66);
            btnCheck.Cursor = Cursors.Hand;
            btnCheck.FlatStyle = FlatStyle.Flat;
            btnCheck.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheck.ForeColor = Color.White;
            btnCheck.Location = new Point(578, 14);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(148, 40);
            btnCheck.TabIndex = 15;
            btnCheck.Text = "Kiểm tra hoá đơn";
            btnCheck.UseVisualStyleBackColor = false;
            // 
            // btnCheckOut
            // 
            btnCheckOut.BackColor = Color.FromArgb(66, 66, 66);
            btnCheckOut.Cursor = Cursors.Hand;
            btnCheckOut.FlatStyle = FlatStyle.Flat;
            btnCheckOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckOut.ForeColor = Color.White;
            btnCheckOut.Location = new Point(732, 14);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Size = new Size(95, 40);
            btnCheckOut.TabIndex = 14;
            btnCheckOut.Text = "Cho xe ra";
            btnCheckOut.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(22, 74);
            label4.Name = "label4";
            label4.Size = new Size(98, 21);
            label4.TabIndex = 9;
            label4.Text = "Số lệnh xuất:";
            // 
            // txtStatus
            // 
            txtStatus.AutoSize = true;
            txtStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStatus.ForeColor = Color.WhiteSmoke;
            txtStatus.Location = new Point(16, 14);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(84, 21);
            txtStatus.TabIndex = 0;
            txtStatus.Text = "Thông báo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(13, 8);
            label3.Name = "label3";
            label3.Size = new Size(153, 21);
            label3.TabIndex = 7;
            label3.Text = "Thông báo hệ thống:";
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(txtStatus);
            panel2.Location = new Point(11, 35);
            panel2.Name = "panel2";
            panel2.Size = new Size(502, 47);
            panel2.TabIndex = 12;
            // 
            // panel5
            // 
            panel5.BackColor = Color.White;
            panel5.Controls.Add(label3);
            panel5.Controls.Add(panel2);
            panel5.Location = new Point(12, 9);
            panel5.Name = "panel5";
            panel5.Size = new Size(525, 95);
            panel5.TabIndex = 3;
            // 
            // mainPanel
            // 
            mainPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mainPanel.Controls.Add(panel5);
            mainPanel.Controls.Add(panel1);
            mainPanel.Controls.Add(cameraPanel);
            mainPanel.Controls.Add(infoPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(20);
            mainPanel.Size = new Size(1404, 848);
            mainPanel.TabIndex = 1;
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
            panel1.Controls.Add(btnCheck);
            panel1.Controls.Add(btnCheckOut);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(545, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(846, 822);
            panel1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(txtNumber);
            panel3.Location = new Point(19, 101);
            panel3.Name = "panel3";
            panel3.Size = new Size(396, 40);
            panel3.TabIndex = 13;
            // 
            // CheckOut
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1404, 848);
            Controls.Add(mainPanel);
            Name = "CheckOut";
            Text = "Quản lý cổng ra";
            Load += CheckOut_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            cameraPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)videoView).EndInit();
            infoPanel.ResumeLayout(false);
            infoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            mainPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Panel cameraPanel;
        private LibVLCSharp.WinForms.VideoView videoView;
        private Panel infoPanel;
        private TextBox txtLicensePlate;
        private Label label2;
        private Label label1;
        private Label lblLicensePlate;
        private PictureBox pictureBoxVehicle;
        private PictureBox pictureBoxLicensePlate;
        private Button btnReset;
        private Button btnDetect;
        private TextBox txtNumber;
        private PictureBox pictureBox2;
        private Label label9;
        private Panel panel8;
        private Label label8;
        private Panel panel7;
        private Label label7;
        private Label label6;
        private Panel statusDB;
        private Panel statusSMO;
        private Label label5;
        private Panel panel4;
        private ComboBox comboBox1;
        private Button btnCheck;
        private Button btnCheckOut;
        private Label label4;
        private Label txtStatus;
        private Label label3;
        private Panel panel2;
        private Panel panel5;
        private Panel mainPanel;
        private Panel panel1;
        private Panel panel3;
    }
}