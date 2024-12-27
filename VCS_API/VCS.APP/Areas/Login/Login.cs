using DMS.BUSINESS.Services.Auth;
using DMS.CORE;
using DMS.BUSINESS.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCS.APP.Areas.Login
{
    public partial class Login : Form
    {
       
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và mật khẩu!");
                return;
            }
            //var response = _service.Login(new LoginDto
            //{
            //    UserName = username.Text.Trim(),
            //    Password = password.Text.Trim(),
            //});

            //MessageBox.Show(response.ToString());

        }
    }
}
