using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Management;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.DbContext.Common;

namespace VCS.APP.Areas.StatusSystem
{
    public partial class StatusSystem : Form
    {
        private readonly AppDbContextForm _dbContext;
        public StatusSystem(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }
        private void CheckStatusSystem()
        {
            try
            {
                var dbStatus = _dbContext.Database.CanConnect();
                SetStatusLabel(label4, dbStatus);
                if (dbStatus)
                {
                    var w = _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode);
                    CheckConnection(w.Tgbx, tgbx);
                    CheckConnection(w.Tdh, tdh);
                    CheckConnection(w.Tdh_e5, tdhe5);
                }
                var token = CommonService.LoginSmoApi();
                SetStatusLabel(label3, !string.IsNullOrEmpty(token));
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void CheckConnection(string connectionString, Label label)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                SetStatusLabel(label, true);
            }
            catch
            {
                SetStatusLabel(label, false);
            }
        }
        private void SetStatusLabel(Label label, bool isConnected)
        {
            label.Text = isConnected ? "Kết nối bình thường" : "Mất kết nối";
            label.ForeColor = isConnected ? Color.LimeGreen : Color.Red;
        }
        private void UpdateCPUInfo()
        {
            try
            {
                var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                var cpuUsage = cpuCounter.NextValue();
                var maxClockSpeed = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor")
                                    .Get().Cast<ManagementObject>().FirstOrDefault()?["MaxClockSpeed"] ?? 0;
                label16.Text = $"{Math.Round(cpuUsage, 2)}% / {Math.Round(Convert.ToDouble(maxClockSpeed) / 1000, 2)} GHz";
            }
            catch
            {
                label16.Text = "Không có thông tin CPU";
            }
        }
        private void UpdateGPUInfo()
        {
            try
            {
                var gpuInfo = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController")
                                    .Get().Cast<ManagementObject>().FirstOrDefault()?["Name"].ToString() ?? "Unknown GPU";
                label18.Text = gpuInfo;
            }
            catch
            {
                label18.Text = "Không có thông tin GPU";
            }
        }
        private void UpdateRAMInfo()
        {
            try
            {
                var totalRAM = Math.Round(Convert.ToDouble(new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem")
                                    .Get().Cast<ManagementObject>().FirstOrDefault()?["TotalPhysicalMemory"] ?? 0) / 1024 / 1024 / 1024, 2);
                var freeRAM = Math.Round(new PerformanceCounter("Memory", "Available MBytes").NextValue() / 1024, 2);
                label17.Text = $"{Math.Round(totalRAM - freeRAM, 2)} GB / {totalRAM} GB";
            }
            catch
            {
                label17.Text = "Không có thông tin RAM";
            }
        }
        private void UpdateStorageInfo()
        {
            try
            {
                var drives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed).ToList();
                var totalStorage = drives.Sum(d => d.TotalSize) / (1024.0 * 1024 * 1024);
                var usedStorage = drives.Sum(d => d.TotalSize - d.AvailableFreeSpace) / (1024.0 * 1024 * 1024);
                label11.Text = $"{Math.Round(usedStorage, 2)} GB / {Math.Round(totalStorage, 2)} GB";
            }
            catch
            {
                label11.Text = "Không có thông tin lưu trữ";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateRAMInfo();
            UpdateCPUInfo();
            UpdateStorageInfo();
            UpdateGPUInfo();
            CheckStatusSystem();
        }
        private void btnClean_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete(Global.PathSaveFile, true);
                CommonService.Alert("Dọn dẹp bộ nhớ thành công!", VCS.Areas.Alert.Alert.enumType.Success);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void ShowError(Exception ex)
        {
            MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
