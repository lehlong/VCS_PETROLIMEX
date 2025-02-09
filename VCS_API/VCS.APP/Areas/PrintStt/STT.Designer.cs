namespace VCS.APP.Areas.PrintStt
{
    partial class STT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STT));
            panel1 = new Panel();
            label1 = new Label();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            labelWareHouse = new Label();
            lblVehicle = new Label();
            lblName = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblName);
            panel1.Controls.Add(lblVehicle);
            panel1.Controls.Add(labelWareHouse);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(20, 69);
            panel1.Name = "panel1";
            panel1.Size = new Size(248, 319);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 56.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(65, 173);
            label1.Name = "label1";
            label1.Size = new Size(128, 100);
            label1.TabIndex = 0;
            label1.Text = "01";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(23, 162, 184);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(23, 23);
            button1.Name = "button1";
            button1.Padding = new Padding(6, 0, 0, 0);
            button1.Size = new Size(99, 40);
            button1.TabIndex = 7;
            button1.Text = "   In vé";
            button1.TextAlign = ContentAlignment.MiddleRight;
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(28, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(193, 41);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // labelWareHouse
            // 
            labelWareHouse.AutoSize = true;
            labelWareHouse.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelWareHouse.Location = new Point(28, 67);
            labelWareHouse.Name = "labelWareHouse";
            labelWareHouse.Size = new Size(135, 25);
            labelWareHouse.TabIndex = 2;
            labelWareHouse.Text = "Kho Bến Thủy";
            labelWareHouse.Click += labelWareHouse_Click;
            // 
            // lblVehicle
            // 
            lblVehicle.AutoSize = true;
            lblVehicle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblVehicle.Location = new Point(28, 102);
            lblVehicle.Name = "lblVehicle";
            lblVehicle.Size = new Size(94, 21);
            lblVehicle.TabIndex = 3;
            lblVehicle.Text = "29S1.12345";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblName.Location = new Point(28, 136);
            lblName.Name = "lblName";
            lblName.Size = new Size(65, 21);
            lblName.TabIndex = 4;
            lblName.Text = "Võ Tòng";
            // 
            // STT
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(288, 408);
            Controls.Add(button1);
            Controls.Add(panel1);
            Name = "STT";
            Padding = new Padding(20);
            Text = "STT";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button button1;
        private PictureBox pictureBox1;
        private Label labelWareHouse;
        private Label lblName;
        private Label lblVehicle;
    }
}