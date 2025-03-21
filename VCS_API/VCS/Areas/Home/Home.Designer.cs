namespace VCS.Areas.Home
{
    partial class Home
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
            viewStreamOut = new LibVLCSharp.WinForms.VideoView();
            viewStreamIn = new LibVLCSharp.WinForms.VideoView();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            vCFullscreenOut = new Button();
            btnViewAllOut = new Button();
            vCFullscreenIn = new Button();
            btnDetectOut = new Button();
            btnViewAllIn = new Button();
            btnDetectIn = new Button();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewStreamOut).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewStreamIn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.White;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(viewStreamOut);
            panel2.Controls.Add(viewStreamIn);
            panel2.Controls.Add(pictureBox2);
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(vCFullscreenOut);
            panel2.Controls.Add(btnViewAllOut);
            panel2.Controls.Add(vCFullscreenIn);
            panel2.Controls.Add(btnDetectOut);
            panel2.Controls.Add(btnViewAllIn);
            panel2.Controls.Add(btnDetectIn);
            panel2.Location = new Point(6, 6);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(1332, 770);
            panel2.TabIndex = 3;
            // 
            // viewStreamOut
            // 
            viewStreamOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            viewStreamOut.BackColor = Color.Black;
            viewStreamOut.Location = new Point(670, 66);
            viewStreamOut.MediaPlayer = null;
            viewStreamOut.Name = "viewStreamOut";
            viewStreamOut.Size = new Size(644, 378);
            viewStreamOut.TabIndex = 0;
            viewStreamOut.Text = "videoView1";
            // 
            // viewStreamIn
            // 
            viewStreamIn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            viewStreamIn.BackColor = Color.Black;
            viewStreamIn.Location = new Point(17, 66);
            viewStreamIn.MediaPlayer = null;
            viewStreamIn.Name = "viewStreamIn";
            viewStreamIn.Size = new Size(644, 378);
            viewStreamIn.TabIndex = 0;
            viewStreamIn.Text = "videoView1";
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Image = Properties.Resources.icons8_wall_mount_camera_30;
            pictureBox2.Location = new Point(670, 27);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 30);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 36;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.icons8_wall_mount_camera_30;
            pictureBox1.Location = new Point(18, 29);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(30, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 35;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.Location = new Point(706, 32);
            label2.Name = "label2";
            label2.Size = new Size(151, 21);
            label2.TabIndex = 34;
            label2.Text = "CAMERA CỔNG RA";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(54, 34);
            label1.Name = "label1";
            label1.Size = new Size(163, 21);
            label1.TabIndex = 33;
            label1.Text = "CAMERA CỔNG VÀO";
            // 
            // vCFullscreenOut
            // 
            vCFullscreenOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            vCFullscreenOut.BackColor = Color.FromArgb(52, 58, 64);
            vCFullscreenOut.Cursor = Cursors.Hand;
            vCFullscreenOut.FlatStyle = FlatStyle.Flat;
            vCFullscreenOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            vCFullscreenOut.ForeColor = Color.White;
            vCFullscreenOut.Image = Properties.Resources.icons8_fullscreen_18;
            vCFullscreenOut.ImageAlign = ContentAlignment.MiddleLeft;
            vCFullscreenOut.Location = new Point(1272, 453);
            vCFullscreenOut.Name = "vCFullscreenOut";
            vCFullscreenOut.Padding = new Padding(6, 0, 0, 0);
            vCFullscreenOut.Size = new Size(42, 40);
            vCFullscreenOut.TabIndex = 31;
            vCFullscreenOut.TextAlign = ContentAlignment.MiddleLeft;
            vCFullscreenOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            vCFullscreenOut.UseVisualStyleBackColor = false;
            vCFullscreenOut.Click += vCFullscreenOut_Click;
            // 
            // btnViewAllOut
            // 
            btnViewAllOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewAllOut.BackColor = Color.Gold;
            btnViewAllOut.Cursor = Cursors.Hand;
            btnViewAllOut.FlatStyle = FlatStyle.Flat;
            btnViewAllOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewAllOut.ForeColor = Color.White;
            btnViewAllOut.Image = Properties.Resources.icons8_application_18;
            btnViewAllOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnViewAllOut.Location = new Point(1224, 453);
            btnViewAllOut.Name = "btnViewAllOut";
            btnViewAllOut.Padding = new Padding(6, 0, 0, 0);
            btnViewAllOut.Size = new Size(42, 40);
            btnViewAllOut.TabIndex = 30;
            btnViewAllOut.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAllOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAllOut.UseVisualStyleBackColor = false;
            btnViewAllOut.Click += btnViewAllOut_Click;
            // 
            // vCFullscreenIn
            // 
            vCFullscreenIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            vCFullscreenIn.BackColor = Color.FromArgb(52, 58, 64);
            vCFullscreenIn.Cursor = Cursors.Hand;
            vCFullscreenIn.FlatStyle = FlatStyle.Flat;
            vCFullscreenIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            vCFullscreenIn.ForeColor = Color.White;
            vCFullscreenIn.Image = Properties.Resources.icons8_fullscreen_18;
            vCFullscreenIn.ImageAlign = ContentAlignment.MiddleLeft;
            vCFullscreenIn.Location = new Point(620, 453);
            vCFullscreenIn.Name = "vCFullscreenIn";
            vCFullscreenIn.Padding = new Padding(6, 0, 0, 0);
            vCFullscreenIn.Size = new Size(42, 40);
            vCFullscreenIn.TabIndex = 24;
            vCFullscreenIn.TextAlign = ContentAlignment.MiddleLeft;
            vCFullscreenIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            vCFullscreenIn.UseVisualStyleBackColor = false;
            vCFullscreenIn.Click += vCFullscreenIn_Click;
            // 
            // btnDetectOut
            // 
            btnDetectOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDetectOut.BackColor = Color.FromArgb(13, 92, 171);
            btnDetectOut.Cursor = Cursors.Hand;
            btnDetectOut.FlatStyle = FlatStyle.Flat;
            btnDetectOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetectOut.ForeColor = Color.White;
            btnDetectOut.Image = Properties.Resources.icons8_camera_18__1_;
            btnDetectOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnDetectOut.Location = new Point(1081, 453);
            btnDetectOut.Name = "btnDetectOut";
            btnDetectOut.Padding = new Padding(6, 0, 0, 0);
            btnDetectOut.Size = new Size(137, 40);
            btnDetectOut.TabIndex = 29;
            btnDetectOut.Text = "Nhận diện xe";
            btnDetectOut.TextAlign = ContentAlignment.MiddleLeft;
            btnDetectOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDetectOut.UseVisualStyleBackColor = false;
            btnDetectOut.Click += btnDetectOut_Click;
            // 
            // btnViewAllIn
            // 
            btnViewAllIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewAllIn.BackColor = Color.Gold;
            btnViewAllIn.Cursor = Cursors.Hand;
            btnViewAllIn.FlatStyle = FlatStyle.Flat;
            btnViewAllIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewAllIn.ForeColor = Color.White;
            btnViewAllIn.Image = Properties.Resources.icons8_application_18;
            btnViewAllIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnViewAllIn.Location = new Point(572, 453);
            btnViewAllIn.Name = "btnViewAllIn";
            btnViewAllIn.Padding = new Padding(6, 0, 0, 0);
            btnViewAllIn.Size = new Size(42, 40);
            btnViewAllIn.TabIndex = 23;
            btnViewAllIn.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAllIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAllIn.UseVisualStyleBackColor = false;
            btnViewAllIn.Click += btnViewAllIn_Click;
            // 
            // btnDetectIn
            // 
            btnDetectIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDetectIn.BackColor = Color.FromArgb(13, 92, 171);
            btnDetectIn.Cursor = Cursors.Hand;
            btnDetectIn.FlatStyle = FlatStyle.Flat;
            btnDetectIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDetectIn.ForeColor = Color.White;
            btnDetectIn.Image = Properties.Resources.icons8_camera_18__1_;
            btnDetectIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnDetectIn.Location = new Point(430, 453);
            btnDetectIn.Name = "btnDetectIn";
            btnDetectIn.Padding = new Padding(6, 0, 0, 0);
            btnDetectIn.Size = new Size(136, 40);
            btnDetectIn.TabIndex = 22;
            btnDetectIn.Text = "Nhận diện xe";
            btnDetectIn.TextAlign = ContentAlignment.MiddleLeft;
            btnDetectIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDetectIn.UseVisualStyleBackColor = false;
            btnDetectIn.Click += btnDetectIn_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1344, 781);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Home";
            Text = "Home";
            Load += Home_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)viewStreamOut).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewStreamIn).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel2;
        private Button vCFullscreenIn;
        private Button btnViewAllIn;
        private Button btnDetectIn;
        private LibVLCSharp.WinForms.VideoView viewStreamOut;
        private Button vCFullscreenOut;
        private LibVLCSharp.WinForms.VideoView viewStreamIn;
        private Button btnViewAllOut;
        private Button btnDetectOut;
        private Label label2;
        private Label label1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
    }
}