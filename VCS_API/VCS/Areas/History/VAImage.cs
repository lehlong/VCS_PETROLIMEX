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
                    pictureBox.Width = 200;
                    pictureBox.Height = 200;
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
