using DMS.BUSINESS.Services.BU;
using DMS.CORE;
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
using VCS.APP.Areas.Home;
using VCS.APP.Areas.Login;
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
            OpenChildForm(new Home(dbContext));
            _dbContext = dbContext;
            _orderService = orderService;
            btnUser.Text = ProfileUtilities.User.FullName;
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Quản lý cổng vào";
            OpenChildForm(new CheckIn(_dbContext, _orderService));
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Trang chủ";
            OpenChildForm(new Home(_dbContext));
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "/ Quản lý cổng ra";
            OpenChildForm(new CheckOut(_dbContext));
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Đóng form hiện tại
                this.Hide();

                // Tạo và hiển thị form đăng nhập mới
                var loginForm = Program.ServiceProvider.GetRequiredService<Login>();
                loginForm.Show();
            }
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
            this.Hide();
        }

        private void panelTitle_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
