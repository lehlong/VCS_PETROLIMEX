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
            label2 = new Label();
            label1 = new Label();
            panel1 = new Panel();
            viewStreamOut = new LibVLCSharp.WinForms.VideoView();
            vCFullscreenOut = new Button();
            panel4 = new Panel();
            viewStreamIn = new LibVLCSharp.WinForms.VideoView();
            btnViewAllOut = new Button();
            vCFullscreenIn = new Button();
            button3 = new Button();
            btnViewAllIn = new Button();
            btnDetect = new Button();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewStreamOut).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)viewStreamIn).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(vCFullscreenOut);
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(btnViewAllOut);
            panel2.Controls.Add(vCFullscreenIn);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(btnViewAllIn);
            panel2.Controls.Add(btnDetect);
            panel2.Location = new Point(6, 6);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(1332, 770);
            panel2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(964, 22);
            label2.Name = "label2";
            label2.Size = new Size(110, 15);
            label2.TabIndex = 34;
            label2.Text = "CAMERA CỔNG RA";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(276, 22);
            label1.Name = "label1";
            label1.Size = new Size(118, 15);
            label1.TabIndex = 33;
            label1.Text = "CAMERA CỔNG VÀO";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(viewStreamOut);
            panel1.Location = new Point(702, 55);
            panel1.Name = "panel1";
            panel1.Size = new Size(581, 369);
            panel1.TabIndex = 32;
            // 
            // viewStreamOut
            // 
            viewStreamOut.BackColor = Color.Black;
            viewStreamOut.Location = new Point(5, 4);
            viewStreamOut.MediaPlayer = null;
            viewStreamOut.Name = "viewStreamOut";
            viewStreamOut.Size = new Size(572, 360);
            viewStreamOut.TabIndex = 0;
            viewStreamOut.Text = "videoView1";
            // 
            // vCFullscreenOut
            // 
            vCFullscreenOut.BackColor = Color.FromArgb(52, 58, 64);
            vCFullscreenOut.Cursor = Cursors.Hand;
            vCFullscreenOut.FlatStyle = FlatStyle.Flat;
            vCFullscreenOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            vCFullscreenOut.ForeColor = Color.White;
            vCFullscreenOut.Image = Properties.Resources.icons8_fullscreen_18;
            vCFullscreenOut.ImageAlign = ContentAlignment.MiddleLeft;
            vCFullscreenOut.Location = new Point(1241, 440);
            vCFullscreenOut.Name = "vCFullscreenOut";
            vCFullscreenOut.Padding = new Padding(6, 0, 0, 0);
            vCFullscreenOut.Size = new Size(42, 40);
            vCFullscreenOut.TabIndex = 31;
            vCFullscreenOut.TextAlign = ContentAlignment.MiddleLeft;
            vCFullscreenOut.TextImageRelation = TextImageRelation.ImageBeforeText;
            vCFullscreenOut.UseVisualStyleBackColor = false;
            vCFullscreenOut.Click += vCFullscreenOut_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(viewStreamIn);
            panel4.Location = new Point(54, 55);
            panel4.Name = "panel4";
            panel4.Size = new Size(581, 369);
            panel4.TabIndex = 28;
            // 
            // viewStreamIn
            // 
            viewStreamIn.BackColor = Color.Black;
            viewStreamIn.Location = new Point(5, 4);
            viewStreamIn.MediaPlayer = null;
            viewStreamIn.Name = "viewStreamIn";
            viewStreamIn.Size = new Size(572, 360);
            viewStreamIn.TabIndex = 0;
            viewStreamIn.Text = "videoView1";
            // 
            // btnViewAllOut
            // 
            btnViewAllOut.BackColor = Color.Gold;
            btnViewAllOut.Cursor = Cursors.Hand;
            btnViewAllOut.FlatStyle = FlatStyle.Flat;
            btnViewAllOut.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewAllOut.ForeColor = Color.White;
            btnViewAllOut.Image = Properties.Resources.icons8_application_18;
            btnViewAllOut.ImageAlign = ContentAlignment.MiddleLeft;
            btnViewAllOut.Location = new Point(1193, 440);
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
            vCFullscreenIn.BackColor = Color.FromArgb(52, 58, 64);
            vCFullscreenIn.Cursor = Cursors.Hand;
            vCFullscreenIn.FlatStyle = FlatStyle.Flat;
            vCFullscreenIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            vCFullscreenIn.ForeColor = Color.White;
            vCFullscreenIn.Image = Properties.Resources.icons8_fullscreen_18;
            vCFullscreenIn.ImageAlign = ContentAlignment.MiddleLeft;
            vCFullscreenIn.Location = new Point(595, 440);
            vCFullscreenIn.Name = "vCFullscreenIn";
            vCFullscreenIn.Padding = new Padding(6, 0, 0, 0);
            vCFullscreenIn.Size = new Size(42, 40);
            vCFullscreenIn.TabIndex = 24;
            vCFullscreenIn.TextAlign = ContentAlignment.MiddleLeft;
            vCFullscreenIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            vCFullscreenIn.UseVisualStyleBackColor = false;
            vCFullscreenIn.Click += vCFullscreenIn_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(13, 92, 171);
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.White;
            button3.Image = Properties.Resources.icons8_camera_18__1_;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(1051, 440);
            button3.Name = "button3";
            button3.Padding = new Padding(6, 0, 0, 0);
            button3.Size = new Size(136, 40);
            button3.TabIndex = 29;
            button3.Text = "Nhận diện xe";
            button3.TextAlign = ContentAlignment.MiddleLeft;
            button3.TextImageRelation = TextImageRelation.ImageBeforeText;
            button3.UseVisualStyleBackColor = false;
            // 
            // btnViewAllIn
            // 
            btnViewAllIn.BackColor = Color.Gold;
            btnViewAllIn.Cursor = Cursors.Hand;
            btnViewAllIn.FlatStyle = FlatStyle.Flat;
            btnViewAllIn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewAllIn.ForeColor = Color.White;
            btnViewAllIn.Image = Properties.Resources.icons8_application_18;
            btnViewAllIn.ImageAlign = ContentAlignment.MiddleLeft;
            btnViewAllIn.Location = new Point(547, 440);
            btnViewAllIn.Name = "btnViewAllIn";
            btnViewAllIn.Padding = new Padding(6, 0, 0, 0);
            btnViewAllIn.Size = new Size(42, 40);
            btnViewAllIn.TabIndex = 23;
            btnViewAllIn.TextAlign = ContentAlignment.MiddleLeft;
            btnViewAllIn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnViewAllIn.UseVisualStyleBackColor = false;
            btnViewAllIn.Click += btnViewAllIn_Click;
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
            btnDetect.Location = new Point(405, 440);
            btnDetect.Name = "btnDetect";
            btnDetect.Padding = new Padding(6, 0, 0, 0);
            btnDetect.Size = new Size(136, 40);
            btnDetect.TabIndex = 22;
            btnDetect.Text = "Nhận diện xe";
            btnDetect.TextAlign = ContentAlignment.MiddleLeft;
            btnDetect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDetect.UseVisualStyleBackColor = false;
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
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)viewStreamOut).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)viewStreamIn).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel2;
        private Button vCFullscreenIn;
        private Button btnViewAllIn;
        private Button btnDetect;
        private Panel panel1;
        private LibVLCSharp.WinForms.VideoView viewStreamOut;
        private Button vCFullscreenOut;
        private Panel panel4;
        private LibVLCSharp.WinForms.VideoView viewStreamIn;
        private Button btnViewAllOut;
        private Button button3;
        private Label label2;
        private Label label1;
    }
}