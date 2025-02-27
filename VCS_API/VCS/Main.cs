using DMS.CORE;
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
using VCS.APP.Areas.ConfigApp;
using VCS.APP.Areas.History;
using VCS.APP.Areas.StatusSystem;
using VCS.APP.Utilities;
using VCS.Areas.CheckIn;
using VCS.Areas.CheckOut;
using VCS.Areas.Home;
using VCS.Areas.Login;

namespace VCS
{
    public partial class Main : Form
    {
        private AppDbContextForm _dbContext;
        private Form activeForm;
        public Main(AppDbContextForm dbContext)
        {
            InitializeComponent();
             _dbContext = dbContext;
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            txtUsername.Text = ProfileUtilities.User.FullName;

            txtWarehouse.Text = await Task.Run(() =>
            {
                return _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name;
            });
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
            notifyIcon.Visible = false;
        }
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                this.panelMain.Controls.Clear();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;


            this.panelMain.Controls.Add(childForm);
            this.panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }


        #region Trang chủ
        private void pHome_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
            p1.BackColor = Color.FromArgb(66, 66, 66);
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
            p1.BackColor = Color.FromArgb(66, 66, 66);
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Home(_dbContext));
            txtTitle.Text = "- Trang chủ";
            p1.BackColor = Color.FromArgb(66, 66, 66);
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }
        #endregion

        #region Quản lý cổng vào
        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckIn(_dbContext));
            txtTitle.Text = "- Quản lý cổng vào";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.FromArgb(66, 66, 66);
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckIn(_dbContext));
            txtTitle.Text = "- Quản lý cổng vào";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.FromArgb(66, 66, 66);
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckIn(_dbContext));
            txtTitle.Text = "- Quản lý cổng vào";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.FromArgb(66, 66, 66);
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }
        #endregion

        #region Quản lý cổng ra
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckOut(_dbContext));
            txtTitle.Text = "- Quản lý cổng ra";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.FromArgb(66, 66, 66);
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckOut(_dbContext));
            txtTitle.Text = "- Quản lý cổng ra";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.FromArgb(66, 66, 66);
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CheckOut(_dbContext));
            txtTitle.Text = "- Quản lý cổng ra";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.FromArgb(66, 66, 66);
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }
        #endregion

        #region Trạng thái kết nối
        private void label2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StatusSystem(_dbContext));
            txtTitle.Text = "- Trạng thái kết nối";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.FromArgb(66, 66, 66);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StatusSystem(_dbContext));
            txtTitle.Text = "- Trạng thái kết nối";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.FromArgb(66, 66, 66);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StatusSystem(_dbContext));
            txtTitle.Text = "- Trạng thái kết nối";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.FromArgb(66, 66, 66);
        }
        #endregion

        #region Cấu hình chung
        private void label3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ConfigApp(_dbContext));
            txtTitle.Text = "- Cấu hình chung";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.FromArgb(66, 66, 66);
            p6.BackColor = Color.Transparent;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ConfigApp(_dbContext));
            txtTitle.Text = "- Cấu hình chung";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.FromArgb(66, 66, 66);
            p6.BackColor = Color.Transparent;
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ConfigApp(_dbContext));
            txtTitle.Text = "- Cấu hình chung";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.Transparent;
            p5.BackColor = Color.FromArgb(66, 66, 66);
            p6.BackColor = Color.Transparent;
        }
        #endregion

        #region Lịch sử vào ra
        private void label4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new History(_dbContext));
            txtTitle.Text = "- Lịch sử vào ra";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.FromArgb(66, 66, 66);
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new History(_dbContext));
            txtTitle.Text = "- Lịch sử vào ra";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.FromArgb(66, 66, 66);
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }
        private void btnHistory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new History(_dbContext));
            txtTitle.Text = "- Lịch sử vào ra";
            p1.BackColor = Color.Transparent;
            p2.BackColor = Color.Transparent;
            p3.BackColor = Color.Transparent;
            p4.BackColor = Color.FromArgb(66, 66, 66);
            p5.BackColor = Color.Transparent;
            p6.BackColor = Color.Transparent;
        }
        #endregion

        #region Đăng xuất
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
               "Xác nhận đăng xuất",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new Login(_dbContext);
                loginForm.Show();
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
               "Xác nhận đăng xuất",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new Login(_dbContext);
                loginForm.Show();
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
               "Xác nhận đăng xuất",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new Login(_dbContext);
                loginForm.Show();
            }
        }
        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon.Visible = true;
            }
            base.OnFormClosing(e);
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://d2s.com.vn/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
