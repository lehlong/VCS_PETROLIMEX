using DMS.BUSINESS.Services.Auth;
using DMS.CORE;
using DMS.BUSINESS.Dtos.Auth;
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

namespace VCS.APP.Areas.Login
{
    public partial class Login : Form
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _dbContext;

        public Login(IAuthService authService, AppDbContext dbContext)
        {
            InitializeComponent();
            _authService = authService;
            _dbContext = dbContext;
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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và mật khẩu!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra kết nối database
                bool canConnect = await _dbContext.Database.CanConnectAsync();
                if (!canConnect)
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu!", "Lỗi kết nối", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var response = await _authService.Login(new LoginDto
                {
                    UserName = username.Text.Trim(),
                    Password = password.Text.Trim(),
                });

                if (response != null)
                {
                    var mainForm = Program.ServiceProvider.GetRequiredService<Main>();
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi đăng nhập", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
