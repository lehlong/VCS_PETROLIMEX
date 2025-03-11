
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VCS.APP.Utilities
{
    public class BorderlessDateTimePicker : DateTimePicker
    {
        private const int WM_NCCALCSIZE = 0x0083;

        public BorderlessDateTimePicker()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCCALCSIZE)
            {
                m.Result = IntPtr.Zero;
                return;
            }
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Ensure the background is transparent
            e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), this.ClientRectangle);

            // Calculate the text rectangle
            Rectangle textRect = new Rectangle(2, 2, this.Width - 20, this.Height - 4);
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, textRect, this.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            // Draw the calendar button
            Rectangle buttonRect = new Rectangle(this.Width - 18, 2, 16, this.Height - 4);
            ControlPaint.DrawComboButton(e.Graphics, buttonRect, ButtonState.Normal);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.SetStyle(ControlStyles.UserPaint, true);
        }
    }

    public class BorderlessComboBox : ComboBox
    {
        public BorderlessComboBox() { this.FlatStyle = FlatStyle.Flat; }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0xF)
            {
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                {
                    using (Pen p = new Pen(Color.WhiteSmoke, 1))
                    { g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1); }
                }
            }
        }
    }
}

