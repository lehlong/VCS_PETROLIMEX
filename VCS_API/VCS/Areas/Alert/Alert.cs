using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCS.Areas.Alert
{
    public partial class Alert : Form
    {
        public Alert()
        {
            InitializeComponent();
        }
        public enum enumAction
        {
            wait,
            start,
            close
        }
        public enum enumType
        {
            Success,
            Warning,
            Error,
            Info
        }
        private Alert.enumAction action;
        private int x, y;

        private static List<Alert> openAlerts = new List<Alert>();
        public void ShowAlert(string msg, enumType type)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int alertHeight = this.Height + 5; // Khoảng cách cố định 5px
            int alertWidth = this.Width;

            int startX = (screenWidth - alertWidth) / 2;

            // Thêm vào danh sách trước khi tính toán vị trí
            openAlerts.Add(this);

            // Cập nhật vị trí của tất cả các thông báo
            for (int i = 0; i < openAlerts.Count; i++)
            {
                int startY = 10 + (alertHeight * i); // Luôn bắt đầu từ 10px từ mép trên
                openAlerts[i].Location = new Point(startX, startY);
            }

            this.TopMost = true;

            // Gán kiểu cảnh báo
            switch (type)
            {
                case enumType.Success:
                    this.pictureBox2.Image = Properties.Resources.done;
                    this.BackColor = Color.FromArgb(40, 167, 69);
                    break;
                case enumType.Error:
                    this.pictureBox2.Image = Properties.Resources.error;
                    this.BackColor = Color.FromArgb(220, 53, 69);
                    break;
                case enumType.Warning:
                    this.pictureBox2.Image = Properties.Resources.error;
                    this.BackColor = Color.FromArgb(245, 199, 26);
                    break;
            }

            this.label1.Text = msg;
            this.Show();
            this.action = enumAction.start;
            this.timer1.Interval = 10;
            timer1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enumAction.close;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case enumAction.wait:
                    timer1.Interval = 3000; // Tự động đóng sau 3 giây
                    action = enumAction.close;
                    break;
                case enumAction.start:
                    timer1.Interval = 10; // Hiệu ứng nhanh hơn
                    this.Opacity += 0.2; // Tăng nhanh Opacity
                    this.Top += 8; // Trượt xuống nhanh hơn

                    if (this.Opacity >= 1.0)
                    {
                        this.Opacity = 1.0;
                        action = enumAction.wait;
                    }
                    break;
                case enumAction.close:
                    timer1.Interval = 10;
                    this.Opacity -= 0.2; // Fade-out nhanh hơn
                    this.Top -= 5; // Trượt lên khi đóng

                    if (this.Opacity <= 0.0)
                    {
                        this.Opacity = 0.0;
                        CloseAlert();
                    }
                    break;
            }
        }
        private void CloseAlert()
        {
            openAlerts.Remove(this);
            this.Close();

            // Cập nhật vị trí của tất cả thông báo còn lại
            for (int i = 0; i < openAlerts.Count; i++)
            {
                int newY = 10 + (this.Height + 5) * i; // Luôn bắt đầu từ 10px từ mép trên
                openAlerts[i].Location = new Point(openAlerts[i].Location.X, newY);
            }
        }
    }
}
