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
            cameraPanelIn = new FlowLayoutPanel();
            panel2 = new Panel();
            cameraPanelOut = new FlowLayoutPanel();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // cameraPanelIn
            // 
            cameraPanelIn.AutoScroll = true;
            cameraPanelIn.BackColor = Color.Transparent;
            cameraPanelIn.ForeColor = SystemColors.ControlText;
            cameraPanelIn.Location = new Point(7, 7);
            cameraPanelIn.Margin = new Padding(4);
            cameraPanelIn.Name = "cameraPanelIn";
            cameraPanelIn.Padding = new Padding(10, 14, 13, 14);
            cameraPanelIn.Size = new Size(687, 798);
            cameraPanelIn.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(cameraPanelOut);
            panel2.Controls.Add(cameraPanelIn);
            panel2.Location = new Point(6, 6);
            panel2.Margin = new Padding(6);
            panel2.Name = "panel2";
            panel2.Size = new Size(1396, 812);
            panel2.TabIndex = 3;
            panel2.Paint += panel2_Paint;
            // 
            // cameraPanelOut
            // 
            cameraPanelOut.AutoScroll = true;
            cameraPanelOut.BackColor = Color.Transparent;
            cameraPanelOut.ForeColor = SystemColors.ControlText;
            cameraPanelOut.Location = new Point(701, 7);
            cameraPanelOut.Margin = new Padding(4);
            cameraPanelOut.Name = "cameraPanelOut";
            cameraPanelOut.Padding = new Padding(10, 14, 13, 14);
            cameraPanelOut.Size = new Size(687, 798);
            cameraPanelOut.TabIndex = 1;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1408, 825);
            Controls.Add(panel2);
            Name = "Home";
            Text = "Home";
            Load += Home_Load;
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel cameraPanelIn;
        private Panel panel2;
        private FlowLayoutPanel cameraPanelOut;
    }
}