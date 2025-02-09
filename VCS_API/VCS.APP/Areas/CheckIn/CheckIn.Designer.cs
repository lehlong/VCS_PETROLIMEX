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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckIn));
            mainPanel = new Panel();
            panel5 = new Panel();
            label3 = new Label();
            panel2 = new Panel();
            txtStatus = new Label();
            panel1 = new Panel();
            button2 = new Button();
            pictureBox2 = new PictureBox();
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
            btnViewAll = new Button();
            txtLicensePlate = new TextBox();
            label2 = new Label();
            label1 = new Label();
            lblLicensePlate = new Label();
            pictureBoxVehicle = new PictureBox();
            pictureBoxLicensePlate = new PictureBox();
            btnReset = new Button();
            btnDetect = new Button();
            mainPanel.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            mainPanel.Controls.Add(panel5);
            mainPanel.Controls.Add(panel1);
            mainPanel.Controls.Add(cameraPanel);
            mainPanel.Controls.Add(infoPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(20);
            mainPanel.Size = new Size(1410, 862);
            mainPanel.TabIndex = 0;
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
            panel2.Location = new Point(11, 32);
            panel2.Name = "panel2";
            panel2.Size = new Size(502, 47);
            panel2.TabIndex = 12;
            panel2.Paint += panel2_Paint;
            // 
            // txtStatus
            // 
            txtStatus.AutoSize = true;
            txtStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStatus.ForeColor = Color.WhiteSmoke;
            txtStatus.Location = new Point(11, 12);
            txtStatus.Name = "txtStatus";
            txtStatus.Size = new Size(84, 21);
            txtStatus.TabIndex = 0;
            txtStatus.Text = "Thông báo";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(button2);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(btnCheckIn);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(545, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(846, 822);
            panel1.TabIndex = 2;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(23, 162, 184);
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(227, 14);
            button2.Name = "button2";
            button2.Padding = new Padding(6, 0, 0, 0);
            button2.Size = new Size(99, 40);
            button2.TabIndex = 26;
            button2.Text = "   Print";
            button2.TextAlign = ContentAlignment.MiddleRight;
            button2.TextImageRelation = TextImageRelation.ImageBeforeText;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
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
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(430, 74);
            label5.Name = "label5";
            label5.Size = new Size(164, 21);
            label5.TabIndex = 16;
            label5.Text = "Cập nhật phương tiện:";
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
            comboBox1.DrawItem += comboBox1_DrawItem;
            comboBox1.SelectedValueChanged += comboBox1_SelectedValueChanged;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(40, 167, 69);
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.White;
            button3.Image = (Image)resources.GetObject("button3.Image");
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(332, 14);
            button3.Name = "button3";
            button3.Padding = new Padding(6, 0, 6, 0);
            button3.Size = new Size(114, 40);
            button3.TabIndex = 15;
            button3.Text = "Cập nhật";
            button3.TextAlign = ContentAlignment.MiddleLeft;
            button3.TextImageRelation = TextImageRelation.ImageBeforeText;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // btnCheckIn
            // 
            btnCheckIn.BackColor = Color.FromArgb(0, 123, 255);
            btnCheckIn.Cursor = Cursors.Hand;
            btnCheckIn.FlatStyle = FlatStyle.Flat;
            btnCheckIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.Image = (Image)resources.GetObject("btnCheckIn.Image");
            btnCheckIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.Location = new Point(635, 14);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Padding = new Padding(6, 0, 0, 0);
            btnCheckIn.Size = new Size(192, 40);
            btnCheckIn.TabIndex = 14;
            btnCheckIn.Text = "Cho vào kho - Cấp số";
            btnCheckIn.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckIn.UseVisualStyleBackColor = false;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(txtNumber);
            panel3.Location = new Point(22, 101);
            panel3.Name = "panel3";
            panel3.Size = new Size(396, 40);
            panel3.TabIndex = 13;
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
            pictureBox1.Click += pictureBox1_Click;
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
            txtNumber.TextChanged += textBox1_TextChanged;
            txtNumber.KeyPress += txtNumber_KeyPress;
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
            // button1
            // 
            button1.BackColor = Color.FromArgb(52, 58, 64);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(453, 14);
            button1.Name = "button1";
            button1.Padding = new Padding(6, 0, 0, 0);
            button1.Size = new Size(174, 40);
            button1.TabIndex = 6;
            button1.Text = "Cho vào hàng chờ";
            button1.TextAlign = ContentAlignment.MiddleLeft;
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
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
            videoView.Click += videoView_Click;
            // 
            // infoPanel
            // 
            infoPanel.BackColor = Color.White;
            infoPanel.Controls.Add(btnViewAll);
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
            infoPanel.Paint += infoPanel_Paint;
            // 
            // btnViewAll
            // 
            btnViewAll.BackColor = Color.Gold;
            btnViewAll.Cursor = Cursors.Hand;
            btnViewAll.FlatStyle = FlatStyle.Flat;
            btnViewAll.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewAll.ForeColor = Color.White;
            btnViewAll.Image = (Image)resources.GetObject("btnViewAll.Image");
            btnViewAll.ImageAlign = ContentAlignment.MiddleLeft;
            btnViewAll.Location = new Point(420, 17);
            btnViewAll.Name = "btnViewAll";
            btnViewAll.Padding = new Padding(6, 0, 0, 0);
            btnViewAll.Size = new Size(42, 40);
            btnViewAll.TabIndex = 10;
            btnViewAll.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAll.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAll.UseVisualStyleBackColor = false;
            btnViewAll.Click += btnViewAll_Click;
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
            label2.Location = new Point(269, 147);
            label2.Name = "label2";
            label2.Size = new Size(114, 21);
            label2.TabIndex = 6;
            label2.Text = "Ảnh nhận diện:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 147);
            label1.Name = "label1";
            label1.Size = new Size(79, 21);
            label1.TabIndex = 5;
            label1.Text = "Ảnh chụp:";
            // 
            // lblLicensePlate
            // 
            lblLicensePlate.AutoSize = true;
            lblLicensePlate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLicensePlate.Location = new Point(16, 60);
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
            btnReset.BackColor = Color.FromArgb(220, 53, 69);
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Image = (Image)resources.GetObject("btnReset.Image");
            btnReset.ImageAlign = ContentAlignment.MiddleLeft;
            btnReset.Location = new Point(468, 17);
            btnReset.Name = "btnReset";
            btnReset.Padding = new Padding(6, 0, 0, 0);
            btnReset.Size = new Size(42, 40);
            btnReset.TabIndex = 5;
            btnReset.TextAlign = ContentAlignment.MiddleLeft;
            btnReset.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnDetect
            // 
            btnDetect.BackColor = Color.FromArgb(0, 123, 255);
            btnDetect.Cursor = Cursors.Hand;
            btnDetect.FlatStyle = FlatStyle.Flat;
            btnDetect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetect.ForeColor = Color.White;
            btnDetect.Image = (Image)resources.GetObject("btnDetect.Image");
            btnDetect.ImageAlign = ContentAlignment.MiddleLeft;
            btnDetect.Location = new Point(278, 17);
            btnDetect.Name = "btnDetect";
            btnDetect.Padding = new Padding(6, 0, 0, 0);
            btnDetect.Size = new Size(136, 40);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.TextAlign = ContentAlignment.MiddleLeft;
            btnDetect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDetect.UseVisualStyleBackColor = false;
            btnDetect.Click += btnDetect_Click;
            // 
            // CheckIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1410, 862);
            Controls.Add(mainPanel);
            Name = "CheckIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý cổng vào";
            Load += CheckIn_Load;
            mainPanel.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private Button btnViewAll;
        private Panel panel4;
        private PictureBox pictureBox1;
        private ComboBox comboBox1;
        private Label label5;
        private PictureBox pictureBox2;
        private Button btnCheckIn;
        private Panel panel5;
        private Button button2;
    }
}