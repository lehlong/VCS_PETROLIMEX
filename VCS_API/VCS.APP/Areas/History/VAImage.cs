using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCS.APP.Areas.History
{
    public partial class VAImage : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        private List<string> imagePaths;
        public VAImage(List<string> imagePaths)
        {
            InitializeComponent();
            this.imagePaths = imagePaths;
            InitializeFlowLayoutPanel();
            AddImagesToPanel(imagePaths);
        }
        private void InitializeFlowLayoutPanel()
        {
            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.WrapContents = true;
            this.Controls.Add(flowLayoutPanel);
        }

        private void AddImagesToPanel(List<string> paths)
        {
            foreach (string path in paths)
            {
                if (System.IO.File.Exists(path))
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Width = 100;
                    pictureBox.Height = 100;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Image = Image.FromFile(path);
                    flowLayoutPanel.Controls.Add(pictureBox);
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
