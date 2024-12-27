using DMS.CORE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Areas.CheckIn;
using VCS.APP.Areas.CheckOut;
using VCS.APP.Areas.Home;
using VCS.APP.Areas.Login;

namespace VCS.APP
{
    public partial class Main : Form
    {
        private Form activeForm;
        private AppDbContext _dbContext;
        public Main(AppDbContext dbContext)
        {
            InitializeComponent();
            OpenChildForm(new Home(dbContext));
            _dbContext = dbContext;
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "QUẢN LÝ CỔNG VÀO";
            OpenChildForm(new CheckIn(_dbContext));
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "TRANG CHỦ";
            OpenChildForm(new Home(_dbContext));
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "QUẢN LÝ CỔNG RA";
            OpenChildForm(new CheckOut(_dbContext));
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {

        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelMain.Controls.Add(childForm);
            this.panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelTitle_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
