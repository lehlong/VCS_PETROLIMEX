using DMS.CORE;
using System.Diagnostics;
using VCS.APP.Areas.ConfigApp;
using VCS.APP.Areas.History;
using VCS.APP.Areas.StatusSystem;
using VCS.APP.Services;
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
        private Panel[] panels;

        public Main(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            panels = new Panel[] { p1, p2, p3, p4, p5, p6 };
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            txtUsername.Text = ProfileUtilities.User.FullName;
            txtWarehouse.Text = await Task.Run(() => _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name);
            notifyIcon.Visible = false;
            OpenChildForm(new Home(_dbContext), 0, "Trang chủ");
        }

        private void OpenChildForm(Form childForm, int activePanelIndex, string title)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                panelMain.Controls.Clear();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            SetActivePanel(activePanelIndex);
            this.Text = $"Hệ thống VCS - {title}";
        }

        private void SetActivePanel(int index)
        {
            for (int i = 0; i < panels.Length; i++)
                panels[i].BackColor = i == index ? Color.FromArgb(13, 92, 171) : Color.Transparent;
        }

        private void OpenSection(Form form, int panelIndex, string permission, string title)
        {
            if (CommonService.HasPermission(permission))
                OpenChildForm(form, panelIndex, title);
            else
                MessageBox.Show("Tài khoản không có quyền truy cập vào chức năng này");
        }

        private void OpenHome(object sender, EventArgs e) => OpenChildForm(new Home(_dbContext), 0, "Trang chủ");
        private void OpenCheckIn(object sender, EventArgs e) => OpenSection(new CheckIn(_dbContext), 1, "R410", "Quản lý cổng vào");
        private void OpenCheckOut(object sender, EventArgs e) => OpenSection(new CheckOut(_dbContext), 2, "R411", "Quản lý cổng ra");
        private void OpenHistory(object sender, EventArgs e) => OpenSection(new History(_dbContext), 3, "R412", "Lịch sử vào ra");
        private void OpenSetting(object sender, EventArgs e) => OpenSection(new ConfigApp(_dbContext), 4, "R413", "Cấu hình chung");
        private void OpenStatus(object sender, EventArgs e) => OpenSection(new StatusSystem(_dbContext), 5, "R414", "Trạng thái kết nối");

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                new Login(_dbContext).Show();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo { FileName = "https://d2s.com.vn/", UseShellExecute = true }); }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void label1_Click(object sender, EventArgs e) => OpenHome(sender, e);
        private void pictureBox2_Click(object sender, EventArgs e) => OpenHome(sender, e);
        private void pHome_Click(object sender, EventArgs e) => OpenHome(sender, e);
        private void label6_Click(object sender, EventArgs e) => OpenCheckIn(sender, e);
        private void pictureBox7_Click(object sender, EventArgs e) => OpenCheckIn(sender, e);
        private void btnCheckIn_Click(object sender, EventArgs e) => OpenCheckIn(sender, e);
        private void label5_Click(object sender, EventArgs e) => OpenCheckOut(sender, e);
        private void pictureBox6_Click(object sender, EventArgs e) => OpenCheckOut(sender, e);
        private void btnCheckOut_Click(object sender, EventArgs e) => OpenCheckOut(sender, e);
        private void label4_Click(object sender, EventArgs e) => OpenHistory(sender, e);
        private void pictureBox5_Click(object sender, EventArgs e) => OpenHistory(sender, e);
        private void btnHistory_Click(object sender, EventArgs e) => OpenHistory(sender, e);
        private void label3_Click(object sender, EventArgs e) => OpenSetting(sender, e);
        private void pictureBox4_Click(object sender, EventArgs e) => OpenSetting(sender, e);
        private void btnSetting_Click(object sender, EventArgs e) => OpenSetting(sender, e);
        private void label2_Click(object sender, EventArgs e) => OpenStatus(sender, e);
        private void pictureBox3_Click(object sender, EventArgs e) => OpenStatus(sender, e);
        private void btnStatus_Click(object sender, EventArgs e) => OpenStatus(sender, e);
        private void label9_Click(object sender, EventArgs e) => btnLogOut_Click(sender, e);
        private void pictureBox10_Click(object sender, EventArgs e) => btnLogOut_Click(sender, e);
    }
}
