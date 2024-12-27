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
            cameraPanel = new FlowLayoutPanel();
            panel1 = new Panel();
            button1 = new Button();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // cameraPanel
            // 
            cameraPanel.AutoScroll = true;
            cameraPanel.BackColor = Color.Transparent;
            cameraPanel.ForeColor = SystemColors.ControlText;
            cameraPanel.Location = new Point(2, 73);
            cameraPanel.Margin = new Padding(4);
            cameraPanel.Name = "cameraPanel";
            cameraPanel.Padding = new Padding(13, 14, 13, 14);
            cameraPanel.Size = new Size(1785, 1008);
            cameraPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(button1);
            panel1.Location = new Point(2, 3);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1785, 73);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(24, 144, 255);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Location = new Point(13, 19);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(138, 42);
            button1.TabIndex = 0;
            button1.Text = "Reset Camera";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(cameraPanel);
            panel2.Location = new Point(-1, -2);
            panel2.Name = "panel2";
            panel2.Size = new Size(1785, 1065);
            panel2.TabIndex = 2;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1779, 1061);
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

        private System.Windows.Forms.FlowLayoutPanel cameraPanel;
        private Panel panel1;
        private Button button1;
        private Panel panel2;
    }
}