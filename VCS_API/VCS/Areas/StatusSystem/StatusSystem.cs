using DMS.CORE;
using System.Diagnostics;
using System.Management;
using VCS.APP.Services;
using VCS.APP.Utilities;

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
        private void StatusSystem_Load(object sender, EventArgs e)
        {
        }
        private async void CheckStatusSystem()
        {
            try
            {
                if (!await _dbContext.Database.CanConnectAsync())
                {
                    label4.Text = "Mất kết nối";
                    label4.ForeColor = Color.Red;
                }
                else
                {
                    label4.Text = "Kết nối bình thường";
                    label4.ForeColor = Color.LimeGreen;
                }
                var token = CommonService.LoginSmoApi();
                if (string.IsNullOrEmpty(token))
                {
                    label3.Text = "Mất kết nối";
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.Text = "Kết nối bình thường";
                    label3.ForeColor = Color.LimeGreen;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateCPUInfo()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            float cpuUsage = cpuCounter.NextValue();
            double maxClockSpeed = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                maxClockSpeed = Math.Round(Convert.ToDouble(obj["MaxClockSpeed"]) / 1000, 2);
            }
            label16.Text = $"{Math.Round(cpuUsage, 2)}% / {maxClockSpeed} GHz";
        }
        private void UpdateGPUInfo()
        {
            string gpuName = "Unknown GPU";
            float gpuUsage = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                gpuName = obj["Name"].ToString();
                break;
            }
            ManagementObjectSearcher gpuUsageSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");
            foreach (ManagementObject obj in gpuUsageSearcher.Get())
            {
                gpuUsage = Convert.ToSingle(obj["UtilizationPercentage"]);
                break;
            }

            label18.Text = $"{gpuName} ({gpuUsage}%)";
        }
        private void UpdateRAMInfo()
        {
            double totalRAM = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                totalRAM = Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / 1024 / 1024 / 1024, 2);
            }
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            double freeRAM = Math.Round(ramCounter.NextValue() / 1024, 2);
            double usedRAM = Math.Round(totalRAM - freeRAM, 2);
            label17.Text = $"{usedRAM} GB / {totalRAM} GB";
        }
        private void UpdateStorageInfo()
        {
            double totalStorage = 0;
            double usedStorage = 0;

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed) // Chỉ lấy ổ cứng chính
                {
                    totalStorage += drive.TotalSize / (1024.0 * 1024 * 1024); // Chuyển từ byte -> GB
                    usedStorage += (drive.TotalSize - drive.AvailableFreeSpace) / (1024.0 * 1024 * 1024);
                }
            }

            label11.Text = $"{Math.Round(usedStorage, 2)} GB / {Math.Round(totalStorage, 2)} GB";
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
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
