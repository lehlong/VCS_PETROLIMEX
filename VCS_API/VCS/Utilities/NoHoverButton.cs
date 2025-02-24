using System;
using System.Drawing;
using System.Windows.Forms;

public class NoHoverButton : Button
{
    protected override void OnMouseEnter(EventArgs e)
    {
       // this.Font = new Font(this.Font, FontStyle.Bold);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
       // this.Font = new Font(this.Font, FontStyle.Regular);
    }
}
