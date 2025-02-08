using DMS.CORE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Services;
using VCS.APP.Utilities;

namespace VCS.APP.Areas.StatusSystem
{
    public partial class StatusSystem : Form
    {
        private readonly AppDbContext _dbContext;
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter gpuCounter = new PerformanceCounter("GPU Engine", "Utilization Percentage", "engtype_3D");
        public StatusSystem(AppDbContext dbContext)
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
                var _s = new CommonService();
                var token = _s.LoginSmoApi();
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
        private string GetCPUInfo()
        {
            // Lấy % CPU sử dụng
            float cpuUsage = cpuCounter.NextValue();

            // Lấy tốc độ tối đa của CPU (GHz)
            double maxClockSpeed = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                maxClockSpeed = Math.Round(Convert.ToDouble(obj["MaxClockSpeed"]) / 1000, 2); // Chuyển từ MHz -> GHz
            }

            return $"{Math.Round(cpuUsage, 2)}% / {maxClockSpeed} GHz";
        }
        private void UpdateGPUInfo()
        {
            label18.Text = GetGPUInfo();
        }
        private string GetGPUInfo()
        {
            string gpuName = "Unknown GPU";
            float gpuUsage = 0;

            // Lấy tên GPU
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                gpuName = obj["Name"].ToString();
                break; // Chỉ lấy GPU đầu tiên
            }

            // Lấy % GPU Usage
            ManagementObjectSearcher gpuUsageSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");
            foreach (ManagementObject obj in gpuUsageSearcher.Get())
            {
                gpuUsage = Convert.ToSingle(obj["UtilizationPercentage"]);
                break; // Chỉ lấy giá trị đầu tiên
            }

            return $"{gpuName} ({gpuUsage}%)";
        }

        private void UpdateCPUInfo()
        {
            label16.Text = GetCPUInfo();
        }
        private string GetRAMInfo()
        {
            // Lấy tổng dung lượng RAM
            double totalRAM = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                totalRAM = Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / 1024 / 1024 / 1024, 2);
            }

            // Lấy RAM còn trống
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            double freeRAM = Math.Round(ramCounter.NextValue() / 1024, 2);

            // Tính RAM đã sử dụng
            double usedRAM = Math.Round(totalRAM - freeRAM, 2);

            // Trả về chuỗi định dạng: {RAM đang sử dụng} / {Tổng RAM}
            return $"{usedRAM} GB / {totalRAM} GB";
        }
        private void UpdateRAMInfo()
        {
            label17.Text = GetRAMInfo();
        }

        private string GetStorageInfo()
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

            return $"{Math.Round(usedStorage, 2)} GB / {Math.Round(totalStorage, 2)} GB";
        }
        private void UpdateStorageInfo()
        {
            label11.Text = GetStorageInfo();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            UpdateRAMInfo();
            UpdateCPUInfo();
            UpdateStorageInfo();
            UpdateGPUInfo();
            CheckStatusSystem();
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void btnClean_Click(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRestartDetect_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Global.DetectFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                   "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
