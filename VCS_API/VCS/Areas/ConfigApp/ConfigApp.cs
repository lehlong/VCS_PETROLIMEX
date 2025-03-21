﻿using DMS.CORE;
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
using System.ServiceProcess;
using VCS.APP.Utilities;
using System;

using System.Diagnostics;
using System.Threading;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Win32;

namespace VCS.APP.Areas.ConfigApp
{
    public partial class ConfigApp : Form
    {
        private readonly AppDbContextForm _dbContext;

        private string AppName = "VCS"; 
        private RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

        public ConfigApp(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            GetConfig();
        }

        private void ConfigApp_Load(object sender, EventArgs e)
        {
            if (key.GetValue(AppName) == null)
            {
                isStarup.Checked = false;
            }
            else
            {
                isStarup.Checked = true;
            }
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
            txtCropWidth.Text = config["Setting:CropImagesWidth"];
            txtCropHeight.Text = config["Setting:CropImagesHeight"];
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "appsettings.json";
                string json = File.ReadAllText(filePath);
                var jsonObj = JsonNode.Parse(json);
                FileInfo fileInfo = new FileInfo(filePath);
                string serviceName = "VCS.SERVICE.IMAGE";


                if (jsonObj != null)
                {
                    jsonObj["Setting"]["SmoApiUrl"] = txtSmoApiUrl.Text;
                    jsonObj["Setting"]["SmoApiUsername"] = txtSmoApiUsername.Text;
                    jsonObj["Setting"]["SmoApiPassword"] = txtSmoApiPassword.Text;
                    jsonObj["Setting"]["PathSaveFile"] = txtPathSaveFile.Text;
                    jsonObj["Setting"]["CropImagesWidth"] = txtCropWidth.Text;
                    jsonObj["Setting"]["CropImagesHeight"] = txtCropHeight.Text;
                   

                    File.WriteAllText(filePath, jsonObj.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                }



                Global.SmoApiUrl = txtSmoApiUrl.Text;
                Global.SmoApiUsername = txtSmoApiUsername.Text;
                Global.SmoApiPassword = txtSmoApiPassword.Text;
                Global.PathSaveFile = txtPathSaveFile.Text;
                Global.CropWidth = Convert.ToUInt32(txtCropWidth.Text);
                Global.CropHeight = Convert.ToUInt32(txtCropHeight.Text);


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

        private void isStarup_CheckedChanged(object sender, EventArgs e)
        {
            if (isStarup.Checked)
            {
                key.SetValue(AppName, $"\"{Application.ExecutablePath}\"");
            }
            else
            {
                key.DeleteValue(AppName);
            }
        }
    }
}
