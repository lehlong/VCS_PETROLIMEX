using DMS.CORE;
using DMS.CORE.Entities.MD;
using LibVLCSharp.Shared;
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
using VCS.APP.Utilities;

namespace VCS.APP.Areas.CheckOut
{
    public partial class CheckOut : Form
    {
        private readonly AppDbContext _dbContext;
        private LibVLCSharp.Shared.LibVLC? _libVLC;
        private List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        private Dictionary<string, LibVLCSharp.Shared.MediaPlayer> _mediaPlayers = new Dictionary<string, LibVLCSharp.Shared.MediaPlayer>();
        public CheckOut(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            InitializeLibVLC();
            GetListCameras();
            InitializeControls();
        }
        private void InitializeLibVLC()
        {
            Core.Initialize();
            _libVLC = new LibVLC(
                "--network-caching=100",
                "--live-caching=100",
                "--file-caching=100",
                "--clock-jitter=0",
                "--clock-synchro=0",
                "--no-audio",
                "--rtsp-tcp"
            );
        }
        private void GetListCameras()
        {
            try
            {
                _lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsOut) // Lọc camera cổng vào
                    .ToList();

                InitializeCameraStreams();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeCameraStreams()
        {
            foreach (var camera in _lstCamera)
            {
                try
                {
                    var media = new Media(_libVLC, camera.Rtsp, FromType.FromLocation);
                    var mediaPlayer = new MediaPlayer(media);
                    _mediaPlayers[camera.Code] = mediaPlayer;

                    // Nếu là camera đầu tiên, hiển thị trong videoView
                    if (_lstCamera.IndexOf(camera) == 0)
                    {
                        videoView.MediaPlayer = mediaPlayer;
                        mediaPlayer.Play();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi khởi tạo camera {camera.Code}: {ex.Message}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void InitializeControls()
        {
            // Khởi tạo các control khác nếu cần
        }
      
        private void CheckOut_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //var number = txtNumber.Text.Trim();
            //if (string.IsNullOrEmpty(number))
            //{
            //    txtStatus.Text = "Vui lòng nhập số lệnh xuất";
            //    txtStatus.ForeColor = Color.Red;
            //    return;
            //}
            //var _s = new CommonService();
            //var token = _s.LoginSmoApi();
            //if (string.IsNullOrEmpty(token))
            //{
            //    txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
            //    txtStatus.ForeColor = Color.Red;
            //    return;
            //}

            //var dataDetail = _s.CheckInvoice(number, token);
            //if (!dataDetail.STATUS)
            //{
            //    txtStatus.Text = $"Lệnh xuất chưa có hoá đơn: {dataDetail.DATA}!";
            //    txtStatus.ForeColor = Color.Red;
            //}
            //else
            //{
            //    txtStatus.Text = $"Thành công! Lệnh xuất đã có hoá đơn!";
            //    txtStatus.ForeColor = Color.Green;
            //}
        }

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    var number = txtNumber.Text.Trim();
            //    if (string.IsNullOrEmpty(number))
            //    {
            //        txtStatus.Text = "Vui lòng nhập số lệnh xuất";
            //        txtStatus.ForeColor = Color.Red;
            //        return;
            //    }
            //    var _s = new CommonService();
            //    var token = _s.LoginSmoApi();
            //    if (string.IsNullOrEmpty(token))
            //    {
            //        txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
            //        txtStatus.ForeColor = Color.Red;
            //        return;
            //    }

            //    var dataDetail = _s.CheckInvoice(number, token);
            //    if (!dataDetail.STATUS)
            //    {
            //        txtStatus.Text = $"Lệnh xuất chưa có hoá đơn: {dataDetail.DATA}!";
            //        txtStatus.ForeColor = Color.Red;
            //    }
            //    else
            //    {
            //        txtStatus.Text = $"Thành công! Lệnh xuất đã có hoá đơn!";
            //        txtStatus.ForeColor = Color.Green;
            //    }
            //}
        }
    }
}
