namespace VCS.Areas.CheckIn
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
            panel2 = new Panel();
            panelDODetail = new Panel();
            panel12 = new Panel();
            txtNoteIn = new TextBox();
            panel11 = new Panel();
            selectVehicle = new VCS.APP.Utilities.BorderlessComboBox();
            panel10 = new Panel();
            btnCheckDetailDO = new PictureBox();
            txtNumberDO = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            btnDeleteQueue = new Button();
            btnResetForm = new Button();
            btnUpdateQueue = new Button();
            btnCheckIn = new Button();
            btnQueue = new Button();
            panel4 = new Panel();
            viewStream = new LibVLCSharp.WinForms.VideoView();
            panel5 = new Panel();
            viewCameraFullscreen = new Button();
            panel1 = new Panel();
            txtVehicleName = new TextBox();
            label1 = new Label();
            panel6 = new Panel();
            pictureBoxLicensePlate = new PictureBox();
            label2 = new Label();
            panel8 = new Panel();
            pictureBoxVehicle = new PictureBox();
            label3 = new Label();
            panel7 = new Panel();
            txtLicensePlate = new TextBox();
            label4 = new Label();
            btnViewAll = new Button();
            btnDetect = new Button();
            panel2.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnCheckDetailDO).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewStream).BeginInit();
            panel5.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).BeginInit();
            panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).BeginInit();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.White;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(panelDODetail);
            panel2.Controls.Add(panel12);
            panel2.Controls.Add(panel11);
            panel2.Controls.Add(panel10);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(btnDeleteQueue);
            panel2.Controls.Add(btnResetForm);
            panel2.Controls.Add(btnUpdateQueue);
            panel2.Controls.Add(btnCheckIn);
            panel2.Controls.Add(btnQueue);
            panel2.Location = new Point(524, 6);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(814, 769);
            panel2.TabIndex = 4;
            // 
            // panelDODetail
            // 
            panelDODetail.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelDODetail.Location = new Point(6, 210);
            panelDODetail.Name = "panelDODetail";
            panelDODetail.Size = new Size(801, 553);
            panelDODetail.TabIndex = 38;
            // 
            // panel12
            // 
            panel12.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel12.BackColor = Color.WhiteSmoke;
            panel12.Controls.Add(txtNoteIn);
            panel12.Location = new Point(6, 159);
            panel12.Name = "panel12";
            panel12.Size = new Size(802, 40);
            panel12.TabIndex = 37;
            // 
            // txtNoteIn
            // 
            txtNoteIn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNoteIn.BackColor = Color.WhiteSmoke;
            txtNoteIn.BorderStyle = BorderStyle.None;
            txtNoteIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNoteIn.Location = new Point(10, 9);
            txtNoteIn.Name = "txtNoteIn";
            txtNoteIn.Size = new Size(782, 22);
            txtNoteIn.TabIndex = 40;
            // 
            // panel11
            // 
            panel11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel11.BackColor = Color.WhiteSmoke;
            panel11.Controls.Add(selectVehicle);
            panel11.Location = new Point(409, 85);
            panel11.Name = "panel11";
            panel11.Size = new Size(399, 40);
            panel11.TabIndex = 37;
            // 
            // selectVehicle
            // 
            selectVehicle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectVehicle.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            selectVehicle.AutoCompleteSource = AutoCompleteSource.ListItems;
            selectVehicle.BackColor = Color.WhiteSmoke;
            selectVehicle.FlatStyle = FlatStyle.Flat;
            selectVehicle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            selectVehicle.FormattingEnabled = true;
            selectVehicle.Location = new Point(5, 5);
            selectVehicle.Name = "selectVehicle";
            selectVehicle.Size = new Size(389, 29);
            selectVehicle.TabIndex = 1;
            selectVehicle.SelectedValueChanged += selectVehicle_SelectedValueChanged;
            // 
            // panel10
            // 
            panel10.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel10.BackColor = Color.WhiteSmoke;
            panel10.Controls.Add(btnCheckDetailDO);
            panel10.Controls.Add(txtNumberDO);
            panel10.Location = new Point(6, 85);
            panel10.Name = "panel10";
            panel10.Size = new Size(397, 40);
            panel10.TabIndex = 36;
            // 
            // btnCheckDetailDO
            // 
            btnCheckDetailDO.Anchor = AnchorStyles.Right;
            btnCheckDetailDO.Cursor = Cursors.Hand;
            btnCheckDetailDO.Image = Properties.Resources.search;
            btnCheckDetailDO.Location = new Point(368, 10);
            btnCheckDetailDO.Name = "btnCheckDetailDO";
            btnCheckDetailDO.Size = new Size(20, 20);
            btnCheckDetailDO.SizeMode = PictureBoxSizeMode.StretchImage;
            btnCheckDetailDO.TabIndex = 39;
            btnCheckDetailDO.TabStop = false;
            btnCheckDetailDO.Click += btnCheckDetailDO_Click;
            // 
            // txtNumberDO
            // 
            txtNumberDO.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNumberDO.BackColor = Color.WhiteSmoke;
            txtNumberDO.BorderStyle = BorderStyle.None;
            txtNumberDO.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumberDO.Location = new Point(10, 9);
            txtNumberDO.MaxLength = 1000;
            txtNumberDO.Name = "txtNumberDO";
            txtNumberDO.Size = new Size(344, 22);
            txtNumberDO.TabIndex = 38;
            txtNumberDO.TextChanged += txtNumberDO_TextChanged;
            txtNumberDO.KeyDown += txtNumberDO_KeyDown;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(9, 136);
            label6.Name = "label6";
            label6.Size = new Size(66, 21);
            label6.TabIndex = 35;
            label6.Text = "Ghi chú:";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(412, 62);
            label7.Name = "label7";
            label7.Size = new Size(209, 21);
            label7.TabIndex = 34;
            label7.Text = "Phương tiện trong hàng chờ:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(9, 62);
            label8.Name = "label8";
            label8.Size = new Size(98, 21);
            label8.TabIndex = 33;
            label8.Text = "Số lệnh xuất:";
            // 
            // btnDeleteQueue
            // 
            btnDeleteQueue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeleteQueue.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteQueue.Cursor = Cursors.Hand;
            btnDeleteQueue.FlatStyle = FlatStyle.Flat;
            btnDeleteQueue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDeleteQueue.ForeColor = Color.White;
            btnDeleteQueue.Image = Properties.Resources.icons8_delete_18;
            btnDeleteQueue.ImageAlign = ContentAlignment.MiddleLeft;
            btnDeleteQueue.Location = new Point(86, 6);
            btnDeleteQueue.Name = "btnDeleteQueue";
            btnDeleteQueue.Padding = new Padding(6, 0, 6, 0);
            btnDeleteQueue.Size = new Size(182, 40);
            btnDeleteQueue.TabIndex = 32;
            btnDeleteQueue.Text = "Xoá khỏi hàng chờ";
            btnDeleteQueue.TextAlign = ContentAlignment.MiddleLeft;
            btnDeleteQueue.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDeleteQueue.UseVisualStyleBackColor = false;
            btnDeleteQueue.Visible = false;
            btnDeleteQueue.Click += btnDeleteQueue_Click;
            // 
            // btnResetForm
            // 
            btnResetForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResetForm.BackColor = Color.FromArgb(220, 53, 69);
            btnResetForm.Cursor = Cursors.Hand;
            btnResetForm.FlatStyle = FlatStyle.Flat;
            btnResetForm.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResetForm.ForeColor = Color.White;
            btnResetForm.Image = Properties.Resources.icons8_sync_181;
            btnResetForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnResetForm.Location = new Point(768, 6);
            btnResetForm.Name = "btnResetForm";
            btnResetForm.Padding = new Padding(6, 0, 6, 0);
            btnResetForm.Size = new Size(40, 40);
            btnResetForm.TabIndex = 31;
            btnResetForm.TextAlign = ContentAlignment.MiddleLeft;
            btnResetForm.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnResetForm.UseVisualStyleBackColor = false;
            btnResetForm.Click += btnResetForm_Click;
            // 
            // btnUpdateQueue
            // 
            btnUpdateQueue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateQueue.BackColor = Color.FromArgb(40, 167, 69);
            btnUpdateQueue.Cursor = Cursors.Hand;
            btnUpdateQueue.FlatStyle = FlatStyle.Flat;
            btnUpdateQueue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUpdateQueue.ForeColor = Color.White;
            btnUpdateQueue.Image = Properties.Resources.icons8_check_18;
            btnUpdateQueue.ImageAlign = ContentAlignment.MiddleLeft;
            btnUpdateQueue.Location = new Point(273, 6);
            btnUpdateQueue.Name = "btnUpdateQueue";
            btnUpdateQueue.Padding = new Padding(6, 0, 6, 0);
            btnUpdateQueue.Size = new Size(114, 40);
            btnUpdateQueue.TabIndex = 30;
            btnUpdateQueue.Text = "Cập nhật";
            btnUpdateQueue.TextAlign = ContentAlignment.MiddleLeft;
            btnUpdateQueue.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUpdateQueue.UseVisualStyleBackColor = false;
            btnUpdateQueue.Visible = false;
            btnUpdateQueue.Click += btnUpdateQueue_Click;
            // 
            // btnCheckIn
            // 
            btnCheckIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCheckIn.BackColor = Color.FromArgb(13, 92, 171);
            btnCheckIn.Cursor = Cursors.Hand;
            btnCheckIn.FlatStyle = FlatStyle.Flat;
            btnCheckIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckIn.ForeColor = Color.White;
            btnCheckIn.Image = Properties.Resources.icons8_log_in_18__2_;
            btnCheckIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.Location = new Point(571, 6);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Padding = new Padding(6, 0, 0, 0);
            btnCheckIn.Size = new Size(192, 40);
            btnCheckIn.TabIndex = 29;
            btnCheckIn.Text = "Cho vào kho - Cấp số";
            btnCheckIn.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckIn.UseVisualStyleBackColor = false;
            btnCheckIn.Click += btnCheckIn_Click;
            // 
            // btnQueue
            // 
            btnQueue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnQueue.BackColor = Color.FromArgb(52, 58, 64);
            btnQueue.Cursor = Cursors.Hand;
            btnQueue.FlatStyle = FlatStyle.Flat;
            btnQueue.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnQueue.ForeColor = Color.White;
            btnQueue.Image = Properties.Resources.icons8_truck_18;
            btnQueue.ImageAlign = ContentAlignment.MiddleLeft;
            btnQueue.Location = new Point(392, 6);
            btnQueue.Name = "btnQueue";
            btnQueue.Padding = new Padding(6, 0, 0, 0);
            btnQueue.Size = new Size(174, 40);
            btnQueue.TabIndex = 28;
            btnQueue.Text = "Cho vào hàng chờ";
            btnQueue.TextAlign = ContentAlignment.MiddleLeft;
            btnQueue.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnQueue.UseVisualStyleBackColor = false;
            btnQueue.Click += btnQueue_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(viewStream);
            panel4.Location = new Point(6, 6);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(6);
            panel4.Size = new Size(514, 295);
            panel4.TabIndex = 6;
            // 
            // viewStream
            // 
            viewStream.BackColor = Color.Black;
            viewStream.Dock = DockStyle.Fill;
            viewStream.Location = new Point(6, 6);
            viewStream.Margin = new Padding(0);
            viewStream.MediaPlayer = null;
            viewStream.Name = "viewStream";
            viewStream.Size = new Size(502, 283);
            viewStream.TabIndex = 0;
            viewStream.Text = "videoView1";
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel5.BackColor = Color.White;
            panel5.Controls.Add(viewCameraFullscreen);
            panel5.Controls.Add(panel1);
            panel5.Controls.Add(label1);
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(label2);
            panel5.Controls.Add(panel8);
            panel5.Controls.Add(label3);
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(label4);
            panel5.Controls.Add(btnViewAll);
            panel5.Controls.Add(btnDetect);
            panel5.Location = new Point(6, 306);
            panel5.Name = "panel5";
            panel5.Size = new Size(514, 469);
            panel5.TabIndex = 7;
            // 
            // viewCameraFullscreen
            // 
            viewCameraFullscreen.BackColor = Color.FromArgb(52, 58, 64);
            viewCameraFullscreen.Cursor = Cursors.Hand;
            viewCameraFullscreen.FlatStyle = FlatStyle.Flat;
            viewCameraFullscreen.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            viewCameraFullscreen.ForeColor = Color.White;
            viewCameraFullscreen.Image = Properties.Resources.icons8_fullscreen_18;
            viewCameraFullscreen.ImageAlign = ContentAlignment.MiddleLeft;
            viewCameraFullscreen.Location = new Point(465, 8);
            viewCameraFullscreen.Name = "viewCameraFullscreen";
            viewCameraFullscreen.Padding = new Padding(6, 0, 0, 0);
            viewCameraFullscreen.Size = new Size(42, 40);
            viewCameraFullscreen.TabIndex = 21;
            viewCameraFullscreen.TextAlign = ContentAlignment.MiddleLeft;
            viewCameraFullscreen.TextImageRelation = TextImageRelation.ImageBeforeText;
            viewCameraFullscreen.UseVisualStyleBackColor = false;
            viewCameraFullscreen.Click += viewCameraFullscreen_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(txtVehicleName);
            panel1.Location = new Point(5, 177);
            panel1.Name = "panel1";
            panel1.Size = new Size(504, 44);
            panel1.TabIndex = 17;
            // 
            // txtVehicleName
            // 
            txtVehicleName.BackColor = Color.WhiteSmoke;
            txtVehicleName.BorderStyle = BorderStyle.None;
            txtVehicleName.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtVehicleName.Location = new Point(7, 6);
            txtVehicleName.MaxLength = 200;
            txtVehicleName.Name = "txtVehicleName";
            txtVehicleName.Size = new Size(487, 32);
            txtVehicleName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(8, 154);
            label1.Name = "label1";
            label1.Size = new Size(50, 21);
            label1.TabIndex = 16;
            label1.Text = "Tài xế:";
            // 
            // panel6
            // 
            panel6.BackColor = Color.WhiteSmoke;
            panel6.Controls.Add(pictureBoxLicensePlate);
            panel6.Location = new Point(265, 259);
            panel6.Name = "panel6";
            panel6.Size = new Size(244, 178);
            panel6.TabIndex = 20;
            // 
            // pictureBoxLicensePlate
            // 
            pictureBoxLicensePlate.Cursor = Cursors.Hand;
            pictureBoxLicensePlate.Location = new Point(5, 4);
            pictureBoxLicensePlate.Name = "pictureBoxLicensePlate";
            pictureBoxLicensePlate.Size = new Size(235, 170);
            pictureBoxLicensePlate.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLicensePlate.TabIndex = 0;
            pictureBoxLicensePlate.TabStop = false;
            pictureBoxLicensePlate.Click += pictureBoxLicensePlate_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(268, 236);
            label2.Name = "label2";
            label2.Size = new Size(114, 21);
            label2.TabIndex = 19;
            label2.Text = "Ảnh nhận diện:";
            // 
            // panel8
            // 
            panel8.BackColor = Color.WhiteSmoke;
            panel8.Controls.Add(pictureBoxVehicle);
            panel8.Location = new Point(6, 259);
            panel8.Name = "panel8";
            panel8.Size = new Size(244, 178);
            panel8.TabIndex = 18;
            // 
            // pictureBoxVehicle
            // 
            pictureBoxVehicle.Cursor = Cursors.Hand;
            pictureBoxVehicle.Location = new Point(5, 4);
            pictureBoxVehicle.Name = "pictureBoxVehicle";
            pictureBoxVehicle.Size = new Size(235, 170);
            pictureBoxVehicle.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxVehicle.TabIndex = 0;
            pictureBoxVehicle.TabStop = false;
            pictureBoxVehicle.Click += pictureBoxVehicle_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(9, 236);
            label3.Name = "label3";
            label3.Size = new Size(79, 21);
            label3.TabIndex = 16;
            label3.Text = "Ảnh chụp:";
            // 
            // panel7
            // 
            panel7.BackColor = Color.WhiteSmoke;
            panel7.Controls.Add(txtLicensePlate);
            panel7.Location = new Point(5, 86);
            panel7.Name = "panel7";
            panel7.Size = new Size(504, 60);
            panel7.TabIndex = 15;
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.BackColor = Color.WhiteSmoke;
            txtLicensePlate.BorderStyle = BorderStyle.None;
            txtLicensePlate.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLicensePlate.Location = new Point(7, 7);
            txtLicensePlate.MaxLength = 8;
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.Size = new Size(487, 47);
            txtLicensePlate.TabIndex = 0;
            txtLicensePlate.TextChanged += txtLicensePlate_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(8, 63);
            label4.Name = "label4";
            label4.Size = new Size(152, 21);
            label4.TabIndex = 14;
            label4.Text = "Biển số phương tiện:";
            // 
            // btnViewAll
            // 
            btnViewAll.BackColor = Color.Gold;
            btnViewAll.Cursor = Cursors.Hand;
            btnViewAll.FlatStyle = FlatStyle.Flat;
            btnViewAll.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewAll.ForeColor = Color.White;
            btnViewAll.Image = Properties.Resources.icons8_application_18;
            btnViewAll.ImageAlign = ContentAlignment.MiddleLeft;
            btnViewAll.Location = new Point(417, 8);
            btnViewAll.Name = "btnViewAll";
            btnViewAll.Padding = new Padding(6, 0, 0, 0);
            btnViewAll.Size = new Size(42, 40);
            btnViewAll.TabIndex = 12;
            btnViewAll.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAll.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAll.UseVisualStyleBackColor = false;
            btnViewAll.Click += btnViewAll_Click;
            // 
            // btnDetect
            // 
            btnDetect.BackColor = Color.FromArgb(13, 92, 171);
            btnDetect.Cursor = Cursors.Hand;
            btnDetect.FlatStyle = FlatStyle.Flat;
            btnDetect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetect.ForeColor = Color.White;
            btnDetect.Image = Properties.Resources.icons8_camera_18__1_;
            btnDetect.ImageAlign = ContentAlignment.MiddleLeft;
            btnDetect.Location = new Point(275, 8);
            btnDetect.Name = "btnDetect";
            btnDetect.Padding = new Padding(6, 0, 0, 0);
            btnDetect.Size = new Size(136, 40);
            btnDetect.TabIndex = 11;
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
            ClientSize = new Size(1344, 781);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CheckIn";
            Text = "Quản lý cổng vào";
            Load += CheckIn_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btnCheckDetailDO).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)viewStream).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).EndInit();
            panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).EndInit();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel2;
        private Panel panel4;
        private Panel panel5;
        private Button btnViewAll;
        private Button btnDetect;
        private Panel panel7;
        private TextBox txtLicensePlate;
        private Label label4;
        private Panel panel8;
        private Label label3;
        private Button btnDeleteQueue;
        private Button btnResetForm;
        private Button btnUpdateQueue;
        private Button btnCheckIn;
        private Button btnQueue;
        private Label label6;
        private Label label7;
        private Label label8;
        private Panel panel11;
        private Panel panel10;
        private Panel panel12;
        private TextBox txtNumberDO;
        private PictureBox btnCheckDetailDO;
        private TextBox txtNoteIn;
        private LibVLCSharp.WinForms.VideoView viewStream;
        private Panel panelDODetail;
        private PictureBox pictureBoxVehicle;
        private Panel panel6;
        private PictureBox pictureBoxLicensePlate;
        private Label label2;
        private Panel panel1;
        private TextBox txtVehicleName;
        private Label label1;
        private Button viewCameraFullscreen;
        private APP.Utilities.BorderlessComboBox selectVehicle;
    }
}