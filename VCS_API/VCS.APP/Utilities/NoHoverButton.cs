using System;
using System.Drawing;
using System.Windows.Forms;

public class NoHoverButton : Button
{
    protected override void OnMouseEnter(EventArgs e)
    {
        // Do nothing to prevent hover effect
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        // Do nothing to prevent hover effect
    }
}
