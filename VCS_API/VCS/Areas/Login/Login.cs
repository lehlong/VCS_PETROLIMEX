using Common.Util;
using DMS.CORE;
using IWshRuntimeLibrary;
using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime;
using VCS.APP.Services;
using VCS.APP.Utilities;

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
            LoadSavedCredentials();
        }

        private void btnLogin_Click(object sender, EventArgs e) => LoginProcess();
        private void username_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) LoginProcess(); }
        private void password_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) LoginProcess(); }

        private void LoadSavedCredentials()
        {
            username.Text = Properties.Settings.Default.UserName;
            password.Text = Properties.Settings.Default.Password;
        }

        private async void LoginProcess()
        {
            if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
            {
                CommonService.Alert("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", Alert.Alert.enumType.Error);
                return;
            }

            var user = _dbContext.TblAdAccount
                .Include(x => x.Account_AccountGroups).ThenInclude(x => x.AccountGroup).ThenInclude(x => x.ListAccountGroupRight).ThenInclude(x => x.Right)
                .Include(x => x.AccountRights).ThenInclude(x => x.Right)
                .FirstOrDefault(x => x.UserName == username.Text.Trim() && x.Password == Utils.CryptographyMD5(password.Text.Trim()));

            if (user == null)
            {
                CommonService.Alert("Tài khoản hoặc mật khẩu không đúng!", Alert.Alert.enumType.Error);
                return;
            }
            if (user.AccountType != "III")
            {
                CommonService.Alert("Hệ thống chỉ dành cho nhân viên bảo vệ!", Alert.Alert.enumType.Error);
                return;
            }

            ProfileUtilities.User = user;
            CommonService.LoadUserConfig();
            CommonService.LoadUserPermissions(user);
            SaveCredentials();
            Global.lstCamera = _dbContext.TblMdCamera.Where(x => x.WarehouseCode == user.WarehouseCode && x.OrgCode == user.OrganizeCode).ToList();
            InitializeLibVLC();
            LoadOnnxModel();
            CommonService.Alert("Đăng nhập thành công!", Alert.Alert.enumType.Success);

            new Main(_dbContext).Show();
            this.Hide();
        }

        private void SaveCredentials()
        {
            Properties.Settings.Default.UserName = username.Text;
            Properties.Settings.Default.Password = password.Text;
            Properties.Settings.Default.Save();
        }

        private void CreateDesktopShortcut()
        {
            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VCS.lnk");
            if (System.IO.File.Exists(shortcutPath)) return;

            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                shortcut.Description = "Hệ thống VCS";
                shortcut.IconLocation = Application.ExecutablePath;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tạo shortcut phần mềm: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e) => Application.Exit();

        private void InitializeLibVLC()
        {
            Core.Initialize();
            Global._libVLC = new LibVLC("--network-caching=30", "--live-caching=30", "--file-caching=30", "--clock-jitter=0", "--clock-synchro=0", "--no-audio", "--rtsp-tcp");
        }

        private void LoadOnnxModel() => Global._session = new InferenceSession(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "model.onnx"));
    }
}
