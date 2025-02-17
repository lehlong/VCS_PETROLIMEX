using DMS.CORE;
using DMS.CORE.Entities.AD;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Utilities;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace VCS.APP.Areas.ConfigApp
{
    public partial class ConfigApp : Form
    {
        private readonly AppDbContext _dbContext;
        private TblAdConfigApp _config = new TblAdConfigApp();
        public ConfigApp(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            GetConfig();
        }

        private void ConfigApp_Load(object sender, EventArgs e)
        {

        }

        private void GetConfig()
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            txtSmoApiUsername.Text = config["Setting:SmoApiUsername"];
            txtSmoApiPassword.Text = config["Setting:SmoApiPassword"];
            txtSmoApiUrl.Text = config["Setting:SmoApiUrl"];
            txtPathSaveFile.Text = config["Setting:PathSaveFile"];
            txtUrlDetect.Text = config["Setting:DetectApiUrl"];
            txtDetectFilePath.Text = config["Setting:DetectFilePath"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "appsettings.json";
                string json = File.ReadAllText(filePath);
                var jsonObj = JsonNode.Parse(json);

                if (jsonObj != null)
                {
                    jsonObj["Setting"]["SmoApiUrl"] = txtSmoApiUrl.Text;
                    jsonObj["Setting"]["SmoApiUsername"] = txtSmoApiUsername.Text;
                    jsonObj["Setting"]["SmoApiPassword"] = txtSmoApiPassword.Text;
                    jsonObj["Setting"]["PathSaveFile"] = txtPathSaveFile.Text;
                    jsonObj["Setting"]["DetectApiUrl"] = txtUrlDetect.Text;
                    jsonObj["Setting"]["DetectFilePath"] = txtDetectFilePath.Text;

                    File.WriteAllText(filePath, jsonObj.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                }

                Global.SmoApiUrl = txtSmoApiUrl.Text;
                Global.SmoApiUsername = txtSmoApiUsername.Text;
                Global.SmoApiPassword = txtSmoApiPassword.Text;
                Global.PathSaveFile = txtPathSaveFile.Text;
                Global.DetectApiUrl = txtUrlDetect.Text;
                Global.DetectFilePath = txtDetectFilePath.Text;

                var result = MessageBox.Show("Vui lòng khởi động lại hệ thống để áp dụng cài đặt!",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    RestartApplication();
                }
                else
                {
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống! Vui lòng liên hệ với quản trị viên!");
            }
        }

        private void RestartApplication()
        {
            Application.Restart();
            Application.Exit();
        }
    }
}
