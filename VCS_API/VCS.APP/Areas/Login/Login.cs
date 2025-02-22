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
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using VCS.APP.Services;
using Common.Util;
using System.Diagnostics;

namespace VCS.APP.Areas.Login
{
    public partial class Login : Form
    {
        private readonly AppDbContextForm _dbContext;
        public Login(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadSavedCredentials();
            username.KeyPress += TextBox_KeyPress;
            password.KeyPress += TextBox_KeyPress;
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnLogin.PerformClick();
            }
        }

        private void LoadSavedCredentials()
        {
            var savedCredentials = CredentialManager.GetSavedCredentials();
            if (savedCredentials != null)
            {
                username.Text = savedCredentials.Username;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validate
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                #region IsValid
                var user = _dbContext.TblAdAccount.FirstOrDefault(x => x.UserName == username.Text.Trim()
                && x.Password == Utils.CryptographyMD5(password.Text.Trim()));

                if (user == null)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                ProfileUtilities.User = user;
                CommonService.LoadUserConfig();
                var main = new Main(_dbContext);
                main.Show();
                this.Hide();
                Process.Start(Global.DetectFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
