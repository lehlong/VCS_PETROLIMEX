namespace VCS.APP.Areas.ViewAllCamera
{
    partial class AllCamera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllCamera));
            cameraPanel = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // cameraPanel
            // 
            cameraPanel.AutoScroll = true;
            cameraPanel.BackColor = Color.Transparent;
            cameraPanel.Dock = DockStyle.Fill;
            cameraPanel.ForeColor = SystemColors.ControlText;
            cameraPanel.Location = new Point(20, 20);
            cameraPanel.Margin = new Padding(4);
            cameraPanel.Name = "cameraPanel";
            cameraPanel.Padding = new Padding(10, 14, 13, 14);
            cameraPanel.Size = new Size(1288, 697);
            cameraPanel.TabIndex = 1;
            // 
            // AllCamera
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1328, 737);
            Controls.Add(cameraPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AllCamera";
            Padding = new Padding(20);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tất cả Camera";
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel cameraPanel;
    }
}