using VCS.APP.Utilities;

namespace VCS.Areas.CheckOut
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
            btnViewAll = new Button();
            panel4 = new Panel();
            viewStream = new LibVLCSharp.WinForms.VideoView();
            panel5 = new Panel();
            panel1 = new Panel();
            txtVehicleName = new TextBox();
            label1 = new Label();
            viewCameraFullscreen = new Button();
            panel6 = new Panel();
            pictureBoxLicensePlate = new PictureBox();
            label2 = new Label();
            panel8 = new Panel();
            pictureBoxVehicle = new PictureBox();
            label3 = new Label();
            panel7 = new Panel();
            txtLicensePlate = new TextBox();
            label4 = new Label();
            btnDetect = new Button();
            panel2 = new Panel();
            btnCheck = new Button();
            btnCheckOut = new Button();
            panel12 = new Panel();
            txtNoteOut = new TextBox();
            panel11 = new Panel();
            selectVehicle = new BorderlessComboBox();
            label6 = new Label();
            label7 = new Label();
            btnResetForm = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            panelCheckIn = new Panel();
            panelCheckOut = new Panel();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewStream).BeginInit();
            panel5.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLicensePlate).BeginInit();
            panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVehicle).BeginInit();
            panel7.SuspendLayout();
            panel2.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
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
            btnViewAll.Location = new Point(416, 9);
            btnViewAll.Name = "btnViewAll";
            btnViewAll.Padding = new Padding(6, 0, 0, 0);
            btnViewAll.Size = new Size(42, 40);
            btnViewAll.TabIndex = 12;
            btnViewAll.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAll.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAll.UseVisualStyleBackColor = false;
            btnViewAll.Click += btnViewAll_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(viewStream);
            panel4.Location = new Point(6, 6);
            panel4.Name = "panel4";
            panel4.Size = new Size(514, 319);
            panel4.TabIndex = 10;
            // 
            // viewStream
            // 
            viewStream.BackColor = Color.Black;
            viewStream.Location = new Point(6, 6);
            viewStream.MediaPlayer = null;
            viewStream.Name = "viewStream";
            viewStream.Size = new Size(503, 307);
            viewStream.TabIndex = 0;
            viewStream.Text = "videoView1";
            // 
            // panel5
            // 
            panel5.BackColor = Color.White;
            panel5.Controls.Add(panel1);
            panel5.Controls.Add(label1);
            panel5.Controls.Add(viewCameraFullscreen);
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(label2);
            panel5.Controls.Add(panel8);
            panel5.Controls.Add(label3);
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(label4);
            panel5.Controls.Add(btnViewAll);
            panel5.Controls.Add(btnDetect);
            panel5.Location = new Point(6, 331);
            panel5.Name = "panel5";
            panel5.Size = new Size(514, 443);
            panel5.TabIndex = 11;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(txtVehicleName);
            panel1.Location = new Point(5, 170);
            panel1.Name = "panel1";
            panel1.Size = new Size(504, 44);
            panel1.TabIndex = 26;
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
            label1.Location = new Point(8, 147);
            label1.Name = "label1";
            label1.Size = new Size(50, 21);
            label1.TabIndex = 25;
            label1.Text = "Tài xế:";
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
            viewCameraFullscreen.Location = new Point(464, 9);
            viewCameraFullscreen.Name = "viewCameraFullscreen";
            viewCameraFullscreen.Padding = new Padding(6, 0, 0, 0);
            viewCameraFullscreen.Size = new Size(42, 40);
            viewCameraFullscreen.TabIndex = 22;
            viewCameraFullscreen.TextAlign = ContentAlignment.MiddleLeft;
            viewCameraFullscreen.TextImageRelation = TextImageRelation.ImageBeforeText;
            viewCameraFullscreen.UseVisualStyleBackColor = false;
            viewCameraFullscreen.Click += viewCameraFullscreen_Click;
            // 
            // panel6
            // 
            panel6.BackColor = Color.WhiteSmoke;
            panel6.Controls.Add(pictureBoxLicensePlate);
            panel6.Location = new Point(264, 257);
            panel6.Name = "panel6";
            panel6.Size = new Size(244, 178);
            panel6.TabIndex = 24;
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
            label2.Location = new Point(267, 234);
            label2.Name = "label2";
            label2.Size = new Size(114, 21);
            label2.TabIndex = 23;
            label2.Text = "Ảnh nhận diện:";
            // 
            // panel8
            // 
            panel8.BackColor = Color.WhiteSmoke;
            panel8.Controls.Add(pictureBoxVehicle);
            panel8.Location = new Point(5, 257);
            panel8.Name = "panel8";
            panel8.Size = new Size(244, 178);
            panel8.TabIndex = 22;
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
            label3.Location = new Point(8, 234);
            label3.Name = "label3";
            label3.Size = new Size(79, 21);
            label3.TabIndex = 21;
            label3.Text = "Ảnh chụp:";
            // 
            // panel7
            // 
            panel7.BackColor = Color.WhiteSmoke;
            panel7.Controls.Add(txtLicensePlate);
            panel7.Location = new Point(5, 79);
            panel7.Name = "panel7";
            panel7.Size = new Size(504, 60);
            panel7.TabIndex = 15;
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.BackColor = Color.WhiteSmoke;
            txtLicensePlate.BorderStyle = BorderStyle.None;
            txtLicensePlate.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLicensePlate.Location = new Point(7, 5);
            txtLicensePlate.MaxLength = 8;
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.Size = new Size(487, 47);
            txtLicensePlate.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(8, 56);
            label4.Name = "label4";
            label4.Size = new Size(152, 21);
            label4.TabIndex = 14;
            label4.Text = "Biển số phương tiện:";
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
            btnDetect.Location = new Point(274, 9);
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
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(btnCheck);
            panel2.Controls.Add(btnCheckOut);
            panel2.Controls.Add(panel12);
            panel2.Controls.Add(panel11);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(btnResetForm);
            panel2.Location = new Point(525, 6);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(814, 209);
            panel2.TabIndex = 8;
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
            btnCheck.Location = new Point(480, 6);
            btnCheck.Name = "btnCheck";
            btnCheck.Padding = new Padding(6, 0, 0, 0);
            btnCheck.Size = new Size(169, 40);
            btnCheck.TabIndex = 40;
            btnCheck.Text = "Kiểm tra hoá đơn";
            btnCheck.TextAlign = ContentAlignment.MiddleLeft;
            btnCheck.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheck.UseVisualStyleBackColor = false;
            btnCheck.Click += btnCheck_Click;
            // 
            // btnCheckOut
            // 
            btnCheckOut.BackColor = Color.FromArgb(13, 92, 171);
            btnCheckOut.Cursor = Cursors.Hand;
            btnCheckOut.FlatStyle = FlatStyle.Flat;
            btnCheckOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckOut.ForeColor = Color.White;
            btnCheckOut.Image = (Image)resources.GetObject("btnCheckOut.Image");
            btnCheckOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.Location = new Point(654, 6);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Padding = new Padding(6, 0, 0, 0);
            btnCheckOut.Size = new Size(109, 40);
            btnCheckOut.TabIndex = 39;
            btnCheckOut.Text = "Cho xe ra";
            btnCheckOut.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCheckOut.UseVisualStyleBackColor = false;
            btnCheckOut.Click += btnCheckOut_Click;
            // 
            // panel12
            // 
            panel12.BackColor = Color.WhiteSmoke;
            panel12.Controls.Add(txtNoteOut);
            panel12.Location = new Point(6, 159);
            panel12.Name = "panel12";
            panel12.Size = new Size(802, 40);
            panel12.TabIndex = 37;
            // 
            // txtNoteOut
            // 
            txtNoteOut.BackColor = Color.WhiteSmoke;
            txtNoteOut.BorderStyle = BorderStyle.None;
            txtNoteOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNoteOut.Location = new Point(10, 9);
            txtNoteOut.Name = "txtNoteOut";
            txtNoteOut.Size = new Size(782, 22);
            txtNoteOut.TabIndex = 40;
            // 
            // panel11
            // 
            panel11.BackColor = Color.WhiteSmoke;
            panel11.Controls.Add(selectVehicle);
            panel11.Location = new Point(6, 85);
            panel11.Name = "panel11";
            panel11.Size = new Size(802, 40);
            panel11.TabIndex = 37;
            // 
            // selectVehicle
            // 
            selectVehicle.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            selectVehicle.AutoCompleteSource = AutoCompleteSource.ListItems;
            selectVehicle.BackColor = Color.WhiteSmoke;
            selectVehicle.FlatStyle = FlatStyle.Flat;
            selectVehicle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            selectVehicle.FormattingEnabled = true;
            selectVehicle.Location = new Point(8, 5);
            selectVehicle.Name = "selectVehicle";
            selectVehicle.Size = new Size(784, 29);
            selectVehicle.TabIndex = 0;
            selectVehicle.SelectedValueChanged += selectVehicle_SelectedValueChanged;
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
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(9, 62);
            label7.Name = "label7";
            label7.Size = new Size(170, 21);
            label7.TabIndex = 34;
            label7.Text = "Phương tiện trong kho:";
            // 
            // btnResetForm
            // 
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
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.ItemSize = new Size(200, 30);
            tabControl1.Location = new Point(525, 220);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(818, 556);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panelCheckOut);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(810, 518);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Lệnh xuất CheckOut";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(panelCheckIn);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(810, 518);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Lệnh xuất CheckIn";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelCheckIn
            // 
            panelCheckIn.Dock = DockStyle.Fill;
            panelCheckIn.Location = new Point(3, 3);
            panelCheckIn.Name = "panelCheckIn";
            panelCheckIn.Size = new Size(804, 512);
            panelCheckIn.TabIndex = 0;
            // 
            // panelCheckOut
            // 
            panelCheckOut.Dock = DockStyle.Fill;
            panelCheckOut.Location = new Point(3, 3);
            panelCheckOut.Name = "panelCheckOut";
            panelCheckOut.Size = new Size(804, 512);
            panelCheckOut.TabIndex = 0;
            // 
            // CheckOut
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1344, 781);
            Controls.Add(tabControl1);
            Controls.Add(panel4);
            Controls.Add(panel5);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CheckOut";
            Text = "CheckOut";
            Load += CheckOut_Load;
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
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel11.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnViewAll;
        private Panel panel4;
        private LibVLCSharp.WinForms.VideoView viewStream;
        private Panel panel5;
        private Panel panel7;
        private TextBox txtLicensePlate;
        private Label label4;
        private Button btnDetect;
        private Panel panel2;
        private Panel panel12;
        private TextBox txtNoteOut;
        private Panel panel11;
        private Label label6;
        private Label label7;
        private Button btnCheck;
        private Button btnCheckOut;
        private Button btnResetForm;
        private Panel panel6;
        private PictureBox pictureBoxLicensePlate;
        private Label label2;
        private Panel panel8;
        private PictureBox pictureBoxVehicle;
        private Label label3;
        private Button viewCameraFullscreen;
        private Panel panel1;
        private TextBox txtVehicleName;
        private Label label1;
        private BorderlessComboBox selectVehicle;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel panelCheckOut;
        private Panel panelCheckIn;
    }
}