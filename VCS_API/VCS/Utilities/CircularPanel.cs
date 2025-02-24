using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VCS.APP.Utilities
{
    public class CircularPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, this.Width, this.Height);
                this.Region = new Region(path);
                using (Pen pen = new Pen(this.BackColor, 1))
                {
                    e.Graphics.DrawEllipse(pen, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }
    }
}

