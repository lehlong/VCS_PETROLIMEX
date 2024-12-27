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
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // cameraPanel
            // 
            cameraPanel.AutoScroll = true;
            cameraPanel.BackColor = SystemColors.Control;
            cameraPanel.ForeColor = SystemColors.ControlText;
            cameraPanel.Location = new Point(0, 64);
            cameraPanel.Margin = new Padding(3, 4, 3, 4);
            cameraPanel.Name = "cameraPanel";
            cameraPanel.Padding = new Padding(11, 13, 11, 13);
            cameraPanel.Size = new Size(1586, 960);
            cameraPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1586, 69);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(24, 144, 255);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.White;
            button1.Location = new Point(11, 19);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(122, 40);
            button1.TabIndex = 0;
            button1.Text = "Reset Camera";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1582, 1024);
            Controls.Add(panel1);
            Controls.Add(cameraPanel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Home";
            Text = "Camera Monitoring";
            Load += Home_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel cameraPanel;
        private Panel panel1;
        private Button button1;
    }
}