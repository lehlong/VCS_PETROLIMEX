namespace VCS.Areas.Alert
{
    public partial class Alert : Form
    {
        public Alert() => InitializeComponent();
        public enum enumAction { wait, start, close }
        public enum enumType { Success, Warning, Error, Info }
        private enumAction action;
        private static List<Alert> openAlerts = new List<Alert>();
        private int waitTime;
        public void ShowAlert(string msg, enumType type)
        {
            Opacity = 0.0;
            StartPosition = FormStartPosition.Manual;
            using (Graphics g = CreateGraphics())
            {
                int textWidth = (int)g.MeasureString(msg, label1.Font).Width + 40;
                Width = Math.Max(Width, textWidth);
            }
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int alertHeight = Height + 5, alertWidth = Width, startX = (screenWidth - alertWidth) / 2;
            openAlerts.Add(this);
            for (int i = 0; i < openAlerts.Count; i++)
                openAlerts[i].Location = new Point(startX, 10 + alertHeight * i);
            TopMost = true;
            (pictureBox2.Image, BackColor, waitTime) = type switch
            {
                enumType.Success => (Properties.Resources.done, Color.FromArgb(40, 167, 69), 3000),
                enumType.Error => (Properties.Resources.error, Color.FromArgb(220, 53, 69), 10000),
                enumType.Warning => (Properties.Resources.error, Color.FromArgb(245, 199, 26), 5000),
                _ => (Properties.Resources.error, Color.FromArgb(23, 162, 184), 5000)
            };
            label1.Text = msg;
            Show();
            action = enumAction.start;
            timer1.Interval = 10;
            timer1.Start();
            Task.Delay(waitTime).ContinueWith(t => action = enumAction.close);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enumAction.close;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (action)
            {
                case enumAction.wait:
                    timer1.Interval = waitTime;
                    action = enumAction.close;
                    break;
                case enumAction.start:
                    timer1.Interval = 10;
                    Opacity = Math.Min(Opacity + 0.2, 1.0);
                    Top += 8;
                    if (Opacity >= 1.0) action = enumAction.wait;
                    break;
                case enumAction.close:
                    timer1.Interval = 10;
                    Opacity -= 0.2;
                    Top -= 5;
                    if (Opacity <= 0.0) CloseAlert();
                    break;
            }
        }
        private void CloseAlert()
        {
            openAlerts.Remove(this);
            Close();
            for (int i = 0; i < openAlerts.Count; i++)
                openAlerts[i].Location = new Point(openAlerts[i].Location.X, 10 + (Height + 5) * i);
        }
    }
}
