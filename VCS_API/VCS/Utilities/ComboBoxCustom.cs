
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VCS.APP.Utilities
{
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

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public ComboBoxItem(string text, string value)
        {
            Text = text; Value = value;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}

