using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VCS.APP.Utilities
{
    public class CustomTextBox : TextBox
    {
        private const int EM_SETRECT = 0x00B3;

        private Padding _textPadding = new Padding(5);

        public Padding TextPadding
        {
            get => _textPadding;
            set
            {
                _textPadding = value;
                UpdatePadding();
            }
        }
        


        private void UpdatePadding()
        {
            if (!IsHandleCreated) return;

            RECT rect = new RECT
            {
                Left = _textPadding.Left,
                Top = _textPadding.Top,
                Right = this.ClientSize.Width - _textPadding.Right,
                Bottom = this.ClientSize.Height - _textPadding.Bottom
            };
            SendMessage(this.Handle, EM_SETRECT, IntPtr.Zero, ref rect);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref RECT lParam);

        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}