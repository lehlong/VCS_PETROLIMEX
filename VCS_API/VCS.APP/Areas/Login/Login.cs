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
using VCS.APP.Utilities;

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
            LoadSavedCredentials();
            // Đăng ký sự kiện KeyPress cho các TextBox
            username.KeyPress += TextBox_KeyPress;
            password.KeyPress += TextBox_KeyPress;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu phím nhấn là Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Ngăn không cho phát ra tiếng beep
                btnLogin.PerformClick(); // Kích hoạt sự kiện click của nút đăng nhập
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void LoadSavedCredentials()
        {
            var savedCredentials = CredentialManager.GetSavedCredentials();
            if (savedCredentials != null)
            {
                username.Text = savedCredentials.Username;
                password.Text = savedCredentials.Password;
                rememberMe.Checked = true;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!await _dbContext.Database.CanConnectAsync())
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu!", "Lỗi kết nối",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var query = new LoginDto
                {
                    UserName = username.Text.Trim(),
                    Password = password.Text.Trim(),
                };
                var response = await _authService.Login(query);

                if (response == null)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _authService.Status = true;
                    return;
                }

                // Lưu thông tin đăng nhập nếu checkbox được chọn
                if (rememberMe.Checked)
                {
                    CredentialManager.SaveCredentials(new LoginCredentials
                    {
                        Username = username.Text,
                        Password = password.Text,
                        RememberMe = true
                    });
                }
                else
                {
                    // Xóa thông tin đăng nhập đã lưu nếu không check
                    CredentialManager.DeleteCredentials();
                }

                ProfileUtilities.User = _dbContext.TblAdAccount.Find(query.UserName);
                var mainForm = Program.ServiceProvider.GetRequiredService<Main>();
                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
