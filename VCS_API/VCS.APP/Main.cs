using DMS.BUSINESS.Services.BU;
using DMS.CORE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
using VCS.APP.Areas.ConfigApp;
using VCS.APP.Areas.History;
using VCS.APP.Areas.Home;
using VCS.APP.Areas.Login;
using VCS.APP.Areas.StatusSystem;
using VCS.APP.Services;
using VCS.APP.Utilities;

namespace VCS.APP
{
    public partial class Main : Form
    {
        private Form activeForm;
        private AppDbContext _dbContext;
        private IWOrderService _orderService;
        public Main(AppDbContext dbContext, IWOrderService orderService)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _orderService = orderService;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtUsername.Text = ProfileUtilities.User.FullName;
            txtWarehouse.Text = GetNameWarehouse();
        }

        private string? GetNameWarehouse()
        {
            try
            {
                return _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
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


        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Quản lý cổng vào";
            OpenChildForm(new CheckIn(_dbContext, _orderService));

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Trang chủ";
            //btnHome.BackColor = Color.C;
            OpenChildForm(new Home(_dbContext));
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Quản lý cổng ra";
            OpenChildForm(new CheckOut(_dbContext));

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Trạng thái kết nối";
            OpenChildForm(new StatusSystem(_dbContext));

        }
        private void button2_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Lịch sử ra vào";
            OpenChildForm(new History(_dbContext));

        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Cấu hình chung";
            OpenChildForm(new ConfigApp(_dbContext));

        }
        private void btnOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
               "Xác nhận đăng xuất",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = Program.ServiceProvider.GetRequiredService<Login>();
                loginForm.Show();
            }
        }
    }
}
