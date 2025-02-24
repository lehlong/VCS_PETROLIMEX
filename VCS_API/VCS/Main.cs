using DMS.CORE;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Utilities;
using VCS.Areas.CheckIn;
using VCS.Areas.CheckOut;
using VCS.Areas.Home;

namespace VCS
{
    public partial class Main : Form
    {
        private readonly AppDbContextForm _dbContext;
        private Form activeForm;
        public Main(AppDbContextForm dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtUsername.Text = ProfileUtilities.User.FullName;
            txtWarehouse.Text = _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name;
        }
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                this.panelMain.Controls.Clear();
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


        #region Trang chủ
        private void pHome_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
        }
        #endregion

        #region Quản lý cổng vào
        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckIn(_dbContext));
            txtTitle.Text = "- Quản lý cổng vào";
        }

        private void label6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckIn(_dbContext));
            txtTitle.Text = "- Quản lý cổng vào";
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckIn(_dbContext));
            txtTitle.Text = "- Quản lý cổng vào";
        }
        #endregion

        #region Quản lý cổng ra
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckOut(_dbContext));
            txtTitle.Text = "- Quản lý cổng ra";
        }

        private void label5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckOut(_dbContext));
            txtTitle.Text = "- Quản lý cổng ra";
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckOut(_dbContext));
            txtTitle.Text = "- Quản lý cổng ra";
        }
        #endregion
    }
}
