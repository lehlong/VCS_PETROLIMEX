using DMS.CORE;
using DMS.CORE.Entities.AD;
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
            _dbContext.ChangeTracker.Clear();
            var config = _dbContext.TblAdConfigApp.FirstOrDefault(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode);
            if (config != null)
            {
                _config = config;
                txtSmoApiUrl.Text = config.SmoApiUrl;
                txtSmoApiUsername.Text = config.SmoApiUsername;
                txtSmoApiPassword.Text = config.SmoApiPassword;
                txtPathSaveFile.Text = config.PathSaveFile;
                txtUrlDetect.Text = config.DetectApiUrl;
                txtDetectFilePath.Text = config.DetectFilePath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _config.SmoApiUrl = txtSmoApiUrl.Text;
                _config.SmoApiUsername = txtSmoApiUsername.Text;
                _config.SmoApiPassword = txtSmoApiPassword.Text;
                _config.PathSaveFile = txtPathSaveFile.Text;
                _config.DetectApiUrl = txtUrlDetect.Text;
                _config.DetectFilePath = txtDetectFilePath.Text;
                _dbContext.TblAdConfigApp.Update(_config);
                _dbContext.SaveChanges();

                MessageBox.Show("Cập nhật thông tin thành công!");
                Global.SmoApiUrl = _config.SmoApiUrl;
                Global.SmoApiUsername= _config.SmoApiUsername;
                Global.SmoApiPassword= _config.SmoApiPassword;
                Global.PathSaveFile= _config.PathSaveFile;
                Global.DetectApiUrl= _config.DetectApiUrl;
                Global.DetectFilePath = _config.DetectFilePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống! Vui lòng liên hệ với quản trị viên!");
            }
        }
    }
}
