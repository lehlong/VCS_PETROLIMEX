using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
using System.Text.Json;
using VCS.APP.Utilities;
using Microsoft.Win32;
using VCS.DbContext.Common;

namespace VCS.APP.Areas.ConfigApp
{
    public partial class ConfigApp : Form
    {
        private readonly AppDbContextForm _dbContext;
        private const string AppName = "VCS";
        private readonly RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        public ConfigApp(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            GetConfig();
        }
        private void ConfigApp_Load(object sender, EventArgs e) => isStarup.Checked = key.GetValue(AppName) != null;
        private void GetConfig()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            txtSmoApiUsername.Text = config["Setting:SmoApiUsername"];
            txtSmoApiPassword.Text = config["Setting:SmoApiPassword"];
            txtSmoApiUrl.Text = config["Setting:SmoApiUrl"];
            txtPathSaveFile.Text = config["Setting:PathSaveFile"];
            txtCropWidth.Text = config["Setting:CropImagesWidth"];
            txtCropHeight.Text = config["Setting:CropImagesHeight"];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "appsettings.json";
                var jsonObj = JsonNode.Parse(File.ReadAllText(filePath));
                if (jsonObj != null)
                {
                    var settings = jsonObj["Setting"];
                    settings["SmoApiUrl"] = txtSmoApiUrl.Text;
                    settings["SmoApiUsername"] = txtSmoApiUsername.Text;
                    settings["SmoApiPassword"] = txtSmoApiPassword.Text;
                    settings["PathSaveFile"] = txtPathSaveFile.Text;
                    settings["CropImagesWidth"] = txtCropWidth.Text;
                    settings["CropImagesHeight"] = txtCropHeight.Text;
                    File.WriteAllText(filePath, jsonObj.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                }
                Global.SmoApiUrl = txtSmoApiUrl.Text;
                Global.SmoApiUsername = txtSmoApiUsername.Text;
                Global.SmoApiPassword = txtSmoApiPassword.Text;
                Global.PathSaveFile = txtPathSaveFile.Text;
                Global.CropWidth = Convert.ToUInt32(txtCropWidth.Text);
                Global.CropHeight = Convert.ToUInt32(txtCropHeight.Text);

                if (MessageBox.Show("Vui lòng khởi động lại hệ thống để áp dụng cài đặt!", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    RestartApplication();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi hệ thống! Vui lòng liên hệ với quản trị viên!");
            }
        }
        private void RestartApplication()
        {
            Application.Restart();
            Application.Exit();
        }
        private void isStarup_CheckedChanged(object sender, EventArgs e)
        {
            if (isStarup.Checked)
                key.SetValue(AppName, $"\"{Application.ExecutablePath}\"");
            else
                key.DeleteValue(AppName);
        }
    }
}
