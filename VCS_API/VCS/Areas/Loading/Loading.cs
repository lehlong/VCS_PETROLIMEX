using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCS.Areas.Loading
{
    public partial class Loading : Form
    {
        private Form parentForm;
        public Loading(Form parentForm)
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Loading_Paint);
            this.Shown += new EventHandler(Loading_Shown);
            this.FormClosing += new FormClosingEventHandler(Loading_FormClosing);
            this.parentForm = parentForm;
            UpdateSizeAndPosition();
            // Đặt kích thước và vị trí của form loading theo form cha
            this.Size = parentForm.Size;
            this.Location = parentForm.Location;

            // Đảm bảo Form Loading luôn ở trên
            this.TopMost = true;
            this.ShowInTaskbar = false;

            // Gán sự kiện để cập nhật vị trí khi form cha di chuyển
            parentForm.LocationChanged += (s, e) => UpdateSizeAndPosition();
            parentForm.SizeChanged += (s, e) => UpdateSizeAndPosition();
            UpdateRegion();
        }
        private void UpdateSizeAndPosition()
        {
            // Lấy kích thước viền và thanh tiêu đề
            int borderWidth = SystemInformation.FixedFrameBorderSize.Width;
            int titleHeight = SystemInformation.CaptionHeight;

            // Nếu Form cha có viền & tiêu đề thì trừ đi kích thước đó
            int adjustedWidth = parentForm.Width - (2 * borderWidth);
            int adjustedHeight = parentForm.Height - (parentForm.FormBorderStyle != FormBorderStyle.None ? titleHeight : 0);

            // Cập nhật kích thước & vị trí của Loading Form
            this.Size = new Size(adjustedWidth, adjustedHeight);
            this.Location = new Point(parentForm.Left + borderWidth, parentForm.Top + (parentForm.FormBorderStyle != FormBorderStyle.None ? titleHeight : 0));

            // Cập nhật vùng hiển thị để tránh tràn
            UpdateRegion();
        }
        private void UpdateRegion()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(0, 0, parentForm.Width, parentForm.Height));

            this.Region = new Region(path);
        }
        private void Loading_Shown(object sender, EventArgs e)
        {
            animationTimer.Start();
        }

        private void Loading_FormClosing(object sender, FormClosingEventArgs e)
        {
            animationTimer.Stop();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            currentAngle += 3;
            if (currentAngle >= 360)
                currentAngle = 0;

            this.Invalidate(); 
        }
        private void Loading_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
    

            // Vị trí và kích thước của progress bar
            int barWidth = 160;
            int barHeight = 10;
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            int barX = centerX - barWidth / 2;
            int barY = centerY - barHeight / 2;

            // Vẽ background của progress bar
            using (SolidBrush bgBrush = new SolidBrush(Color.WhiteSmoke))
            {
                g.FillRectangle(bgBrush, barX, barY, barWidth, barHeight);
            }

            int fillWidth = (int)((currentAngle % 120) / 120.0 * barWidth);

            // Đảm bảo fillWidth luôn lớn hơn 0 để tránh lỗi
            fillWidth = Math.Max(1, fillWidth);

            if (fillWidth > 0)
            {
                using (SolidBrush fillBrush = new SolidBrush(ColorTranslator.FromHtml("#0d5cab")))
                {
                    g.FillRectangle(fillBrush, barX, barY, fillWidth, barHeight);
                }
            }
        }

    }

}

