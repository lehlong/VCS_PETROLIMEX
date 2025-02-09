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
            txtDateTime = new Label();
            lblSTT = new Label();
            label3 = new Label();
            label2 = new Label();
            lblName = new Label();
            lblVehicle = new Label();
            txtWareHouse = new Label();
            pictureBox1 = new PictureBox();
            button2 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(txtDateTime);
            panel1.Controls.Add(lblSTT);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(lblName);
            panel1.Controls.Add(lblVehicle);
            panel1.Controls.Add(txtWareHouse);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(8, 56);
            panel1.Name = "panel1";
            panel1.Size = new Size(213, 215);
            panel1.TabIndex = 0;
            // 
            // txtDateTime
            // 
            txtDateTime.AutoSize = true;
            txtDateTime.Location = new Point(48, 184);
            txtDateTime.Name = "txtDateTime";
            txtDateTime.Size = new Size(0, 15);
            txtDateTime.TabIndex = 8;
            txtDateTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSTT
            // 
            lblSTT.AutoSize = true;
            lblSTT.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSTT.Location = new Point(65, 119);
            lblSTT.Name = "lblSTT";
            lblSTT.Size = new Size(0, 65);
            lblSTT.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(6, 98);
            label3.Name = "label3";
            label3.Size = new Size(53, 21);
            label3.TabIndex = 6;
            label3.Text = "Tài xế:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(6, 75);
            label2.Name = "label2";
            label2.Size = new Size(97, 21);
            label2.TabIndex = 5;
            label2.Text = "Phương tiện:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblName.Location = new Point(56, 98);
            lblName.Name = "lblName";
            lblName.Size = new Size(0, 21);
            lblName.TabIndex = 4;
            // 
            // lblVehicle
            // 
            lblVehicle.AutoSize = true;
            lblVehicle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblVehicle.Location = new Point(100, 75);
            lblVehicle.Name = "lblVehicle";
            lblVehicle.Size = new Size(0, 21);
            lblVehicle.TabIndex = 3;
            lblVehicle.Click += lblVehicle_Click;
            // 
            // txtWareHouse
            // 
            txtWareHouse.AutoSize = true;
            txtWareHouse.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtWareHouse.Location = new Point(6, 51);
            txtWareHouse.Name = "txtWareHouse";
            txtWareHouse.Size = new Size(0, 25);
            txtWareHouse.TabIndex = 2;
            txtWareHouse.Click += labelWareHouse_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(10, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(193, 38);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(0, 123, 255);
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(8, 8);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Padding = new Padding(6, 0, 0, 0);
            button2.Size = new Size(84, 40);
            button2.TabIndex = 8;
            button2.Text = "In STT";
            button2.TextAlign = ContentAlignment.MiddleLeft;
            button2.TextImageRelation = TextImageRelation.ImageBeforeText;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click_1;
            // 
            // STT
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(229, 279);
            Controls.Add(button2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "STT";
            Padding = new Padding(8);
            Text = "In số thự tự";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label txtWareHouse;
        private Label lblName;
        private Label lblVehicle;
        private Label label3;
        private Label label2;
        private Label txtDateTime;
        private Label lblSTT;
        private Button button2;
    }
}