using Common.Util;
using DMS.CORE;
using IWshRuntimeLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Services;
using VCS.APP.Utilities;
using System.Security.Cryptography;
using LibVLCSharp.Shared;
namespace VCS.Areas.Login
{
    public partial class Login : Form
    {
        private readonly AppDbContextForm _dbContext;
        public Login(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            CreateDesktopShortcut();
            username.Text = Properties.Settings.Default.UserName;
            password.Text = Properties.Settings.Default.Password;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginProcess();
            }
        }
        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginProcess();
            }
        }

        private async void LoginProcess()
        {
            try
            {
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var user = _dbContext.TblAdAccount
                .Include(x => x.Account_AccountGroups)
                    .ThenInclude(x => x.AccountGroup)
                    .ThenInclude(x => x.ListAccountGroupRight)
                    .ThenInclude(x => x.Right)
                .Include(x => x.AccountRights)
                    .ThenInclude(x => x.Right)
                .FirstOrDefault(x => x.UserName == username.Text.Trim()
                && x.Password == Utils.CryptographyMD5(password.Text.Trim()));

                if (user == null)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (user.AccountType != "III")
                {
                    MessageBox.Show("Hệ thống chỉ dành cho nhân viên bảo vệ!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ProfileUtilities.User = user;
                await Task.Run(() => CommonService.LoadUserConfig());
                await Task.Run(() => CommonService.LoadUserPermissions(user));
                Properties.Settings.Default.UserName = username.Text;
                Properties.Settings.Default.Password = password.Text;
                Properties.Settings.Default.Save();
                Global.lstCamera = _dbContext.TblMdCamera.Where(x => x.WarehouseCode == user.WarehouseCode && x.OrgCode == user.OrganizeCode).ToList();
                InitializeLibVLC();

                var main = new Main(_dbContext);
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDesktopShortcut()
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VCS.lnk");

            if (!System.IO.File.Exists(shortcutPath))
            {
                try
                {
                    WshShell shell = new WshShell();
                    string exePath = Application.ExecutablePath;

                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                    shortcut.TargetPath = exePath;
                    shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
                    shortcut.Description = "Hệ thống VCS";
                    shortcut.IconLocation = exePath;
                    shortcut.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tạo shorcut phần mềm: " + ex.Message);
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Global._libVLC?.Dispose();
            Application.Exit();
        }

        private void InitializeLibVLC()
        {
            Core.Initialize();
            Global._libVLC = new LibVLC(
   "--network-caching=30",
   "--live-caching=30",
   "--file-caching=30",
   "--clock-jitter=0",
   "--clock-synchro=0",
   "--no-audio",
   "--rtsp-tcp"
);

        }
    }
}
