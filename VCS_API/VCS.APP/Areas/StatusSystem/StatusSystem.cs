using DMS.CORE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Services;

namespace VCS.APP.Areas.StatusSystem
{
    public partial class StatusSystem : Form
    {
        private readonly AppDbContext _dbContext;
        public StatusSystem(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            CheckStatusSystem();
        }

        private void StatusSystem_Load(object sender, EventArgs e)
        {

        }

        private async void CheckStatusSystem()
        {
            try
            {
                if (!await _dbContext.Database.CanConnectAsync())
                {
                    statusDB.BackColor = Color.Red;
                    label4.Text = "( Mất kết nối)";
                }
                else
                {
                    statusDB.BackColor = Color.LimeGreen;
                    label4.Text = "( Kết nối bình thường)";
                }
                var _s = new CommonService();
                var token = _s.LoginSmoApi();
                if (string.IsNullOrEmpty(token))
                {
                    statusSMO.BackColor = Color.Red;
                    label3.Text = "( Mất kết nối)";
                }
                else
                {
                    statusSMO.BackColor = Color.LimeGreen;
                    label3.Text = "( Kết nối bình thường)";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckStatusSystem();
        }
    }
}
