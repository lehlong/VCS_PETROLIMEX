using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCS.APP.Areas.PrintStt
{
    public partial class STT : Form
    {
        private PrintDocument printDocument1 = new PrintDocument();
        private Bitmap panelBitmap;
        public STT()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(PrintDocument1_PrintPage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CapturePanel(panel1); // Capture panel1 content

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        private void CapturePanel(Panel panel)
        {
            panelBitmap = new Bitmap(panel.Width, panel.Height);
            panel.DrawToBitmap(panelBitmap, new Rectangle(0, 0, panel.Width, panel.Height));
        }

        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (panelBitmap != null)
            {
                e.Graphics.DrawImage(panelBitmap, 50, 50); // Adjust position if needed
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void labelWareHouse_Click(object sender, EventArgs e)
        {

        }
    }
}
