using Microsoft.EntityFrameworkCore;
using Python.Runtime;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.DbContext.Common;

namespace VCS.Areas.Login
{
    public partial class Login : Form
    {
        private readonly AppDbContextForm _dbContext;
        public Login(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            LoadSavedCredentials();
            AttachKeyDownEvents();
        }

        private void btnLogin_Click(object sender, EventArgs e) => LoginProcess();

        private void AttachKeyDownEvents()
        {
            username.KeyDown += HandleEnterKey;
            password.KeyDown += HandleEnterKey;
        }

        private void HandleEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) LoginProcess();
        }

        private void LoadSavedCredentials()
        {
            username.Text = Properties.Settings.Default.UserName;
            password.Text = Properties.Settings.Default.Password;
        }

        private async void LoginProcess()
        {
            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                CommonService.Alert("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", Alert.Alert.enumType.Error);
                return;
            }

            string hashedPassword = CommonService.CryptographyMD5(password.Text.Trim());

            var user = _dbContext.TblAdAccount
                .AsNoTracking()
                .Include(x => x.Account_AccountGroups)
                    .ThenInclude(x => x.AccountGroup)
                        .ThenInclude(x => x.ListAccountGroupRight)
                            .ThenInclude(x => x.Right)
                .Include(x => x.AccountRights)
                    .ThenInclude(x => x.Right)
                .FirstOrDefault(x => x.UserName == username.Text.Trim() && x.Password == hashedPassword);

            if (user == null)
            {
                CommonService.Alert("Tài khoản hoặc mật khẩu không đúng!", Alert.Alert.enumType.Error);
                return;
            }

            //if (user.AccountType != "III")
            //{
            //    CommonService.Alert("Hệ thống chỉ dành cho nhân viên bảo vệ!", Alert.Alert.enumType.Error);
            //    return;
            //}

            var main = new Main(_dbContext);

            // Đăng nhập thành công
            ProfileUtilities.User = user;
            CommonService.LoadUserPermissions(user);
            SaveCredentials();

            Global.lstCamera = _dbContext.TblMdCamera
                .AsNoTracking()
                .Where(x => x.WarehouseCode == user.WarehouseCode && x.OrgCode == user.OrganizeCode)
                .ToList();

            CommonService.Alert("Đăng nhập thành công!", Alert.Alert.enumType.Success);

            main.Show();
            this.Hide();
        }

        private void SaveCredentials()
        {
            Properties.Settings.Default.UserName = username.Text;
            Properties.Settings.Default.Password = password.Text;
            Properties.Settings.Default.Save();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            PythonEngine.Shutdown();
            Application.Exit();
        }
    }
}
