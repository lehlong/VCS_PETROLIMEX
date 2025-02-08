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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckOut));
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
            pictureBox2 = new PictureBox();
            label5 = new Label();
            panel4 = new Panel();
            comboBox1 = new ComboBox();
            btnCheckOut = new Button();
            txtStatus = new Label();
            label3 = new Label();
            panel2 = new Panel();
            panel5 = new Panel();
            mainPanel = new Panel();
            panel1 = new Panel();
            btnCheck = new Button();
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
            SuspendLayout();
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
            btnViewAll.TabIndex = 27;
            btnViewAll.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAll.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAll.UseVisualStyleBackColor = false;
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
            btnDetect.Location = new Point(279, 17);
            btnDetect.Name = "btnDetect";
            btnDetect.Padding = new Padding(6, 0, 0, 0);
            btnDetect.Size = new Size(135, 40);
            btnDetect.TabIndex = 2;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.TextAlign = ContentAlignment.MiddleLeft;
            btnDetect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDetect.UseVisualStyleBackColor = false;
            btnDetect.Click += btnDetect_Click;
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
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(11, 74);
            label5.Name = "label5";
            label5.Size = new Size(139, 21);
            label5.TabIndex = 16;
            label5.Text = "Chọn phương tiện:";
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(comboBox1);
            panel4.Location = new Point(11, 101);
            panel4.Name = "panel4";
            panel4.Size = new Size(816, 40);
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
            comboBox1.Size = new Size(816, 41);
            comboBox1.TabIndex = 16;
            comboBox1.DrawItem += comboBox1_DrawItem;
            // 
            // btnCheckOut
            // 
            btnCheckOut.BackColor = Color.FromArgb(0, 123, 255);
            btnCheckOut.Cursor = Cursors.Hand;
            btnCheckOut.FlatStyle = FlatStyle.Flat;
            btnCheckOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckOut.ForeColor = Color.White;
            btnCheckOut.Image = (Image)resources.GetObject("btnCheckOut.Image");
            btnCheckOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.Location = new Point(718, 14);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Padding = new Padding(6, 0, 0, 0);
            btnCheckOut.Size = new Size(109, 40);
            btnCheckOut.TabIndex = 14;
            btnCheckOut.Text = "Cho xe ra";
            btnCheckOut.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckOut.UseVisualStyleBackColor = false;
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
            mainPanel.Size = new Size(1473, 848);
            mainPanel.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnCheck);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(btnCheckOut);
            panel1.Location = new Point(545, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(846, 822);
            panel1.TabIndex = 2;
            // 
            // btnCheck
            // 
            btnCheck.BackColor = Color.FromArgb(52, 58, 64);
            btnCheck.Cursor = Cursors.Hand;
            btnCheck.FlatStyle = FlatStyle.Flat;
            btnCheck.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheck.ForeColor = Color.White;
            btnCheck.Image = (Image)resources.GetObject("btnCheck.Image");
            btnCheck.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheck.Location = new Point(541, 14);
            btnCheck.Name = "btnCheck";
            btnCheck.Padding = new Padding(6, 0, 0, 0);
            btnCheck.Size = new Size(169, 40);
            btnCheck.TabIndex = 26;
            btnCheck.Text = "Kiểm tra hoá đơn";
            btnCheck.TextAlign = ContentAlignment.MiddleLeft;
            btnCheck.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheck.UseVisualStyleBackColor = false;
            // 
            // CheckOut
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1473, 848);
            Controls.Add(mainPanel);
            Name = "CheckOut";
            Text = "Quản lý cổng ra";
            Load += CheckOut_Load;
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
            ResumeLayout(false);
        }

        #endregion
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
        private PictureBox pictureBox2;
        private Label label5;
        private Panel panel4;
        private ComboBox comboBox1;
        private Button btnCheckOut;
        private Label txtStatus;
        private Label label3;
        private Panel panel2;
        private Panel panel5;
        private Panel mainPanel;
        private Panel panel1;
        private Button btnCheck;
        private Button btnViewAll;
    }
}