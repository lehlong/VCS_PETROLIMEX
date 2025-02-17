namespace VCS.APP.Areas.Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            cameraPanelIn = new FlowLayoutPanel();
            panel1 = new Panel();
            button1 = new Button();
            panel2 = new Panel();
            cameraPanelOut = new FlowLayoutPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // cameraPanelIn
            // 
            cameraPanelIn.AutoScroll = true;
            cameraPanelIn.BackColor = Color.Transparent;
            cameraPanelIn.ForeColor = SystemColors.ControlText;
            cameraPanelIn.Location = new Point(2, 61);
            cameraPanelIn.Margin = new Padding(4);
            cameraPanelIn.Name = "cameraPanelIn";
            cameraPanelIn.Padding = new Padding(10, 14, 13, 14);
            cameraPanelIn.Size = new Size(687, 745);
            cameraPanelIn.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(button1);
            panel1.Location = new Point(4, 4);
            panel1.Margin = new Padding(10, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 0, 0);
            panel1.Size = new Size(1370, 53);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(220, 53, 69);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(18, 10);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Padding = new Padding(6, 0, 0, 0);
            button1.Size = new Size(138, 40);
            button1.TabIndex = 0;
            button1.Text = "Reset Camera";
            button1.TextAlign = ContentAlignment.MiddleLeft;
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(cameraPanelOut);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(cameraPanelIn);
            panel2.Location = new Point(10, 9);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(1382, 824);
            panel2.TabIndex = 2;
            // 
            // cameraPanelOut
            // 
            cameraPanelOut.AutoScroll = true;
            cameraPanelOut.BackColor = Color.Transparent;
            cameraPanelOut.ForeColor = SystemColors.ControlText;
            cameraPanelOut.Location = new Point(691, 61);
            cameraPanelOut.Margin = new Padding(4);
            cameraPanelOut.Name = "cameraPanelOut";
            cameraPanelOut.Padding = new Padding(10, 14, 13, 14);
            cameraPanelOut.Size = new Size(687, 745);
            cameraPanelOut.TabIndex = 1;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1890, 1061);
            Controls.Add(panel2);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "Home";
            Text = "Camera Monitoring";
            Load += Home_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel cameraPanelIn;
        private Panel panel1;
        private Button button1;
        private Panel panel2;
        private FlowLayoutPanel cameraPanelOut;
    }
}