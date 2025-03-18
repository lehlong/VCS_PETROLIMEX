using DMS.CORE;
using DMS.CORE.Entities.MD;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Utilities;
using VCS.Areas.ViewAllCamera;
using Media = LibVLCSharp.Shared.Media;

namespace VCS.Areas.Home
{
    public partial class Home : Form
    {
        private readonly AppDbContextForm _dbContext;
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string, MediaPlayer>();
        private TblMdCamera CameraDetectIn { get; set; }
        private TblMdCamera CameraDetectOut { get; set; }
        private MediaPlayer _mediaPlayer;

        public Home(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }
        private void Home_Load(object sender, EventArgs e)
        {
            InitializeCamera(true, viewStreamIn);
            InitializeCamera(false, viewStreamOut);
        }
        private void InitializeCamera(bool isIn, VideoView videoView)
        {
            try
            {
                var camera = Global.lstCamera.FirstOrDefault(x =>
                    (isIn ? x.IsIn : x.IsOut) && x.IsRecognition);

                if (isIn)
                    CameraDetectIn = camera;
                else
                    CameraDetectOut = camera;

                if (camera != null)
                {
                    var media = new Media(Global._libVLC, camera.Rtsp, FromType.FromLocation);
                    var mediaPlayer = new MediaPlayer(media);
                    _mediaPlayers[isIn ? "in" : "out"] = mediaPlayer;

                    videoView.MediaPlayer = mediaPlayer;
                    mediaPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                string cameraType = isIn ? "vào" : "ra";
                MessageBox.Show($"Lỗi khởi tạo camera {cameraType}: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (var player in _mediaPlayers.Values)
            {
                player?.Stop();
                player?.Dispose();
            }
            _mediaPlayers.Clear();

            base.OnFormClosing(e);
        }

        private void btnViewAllIn_Click(object sender, EventArgs e)
        {
            try
            {
                var lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsIn)
                    .ToList();

                if (lstCamera.Any())
                {
                    AllCamera allCameraForm = new AllCamera(lstCamera);
                    allCameraForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không có camera nào được tìm thấy!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewAllOut_Click(object sender, EventArgs e)
        {
            try
            {
                var lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsOut)
                    .ToList();

                if (lstCamera.Any())
                {
                    AllCamera allCameraForm = new AllCamera(lstCamera);
                    allCameraForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không có camera nào được tìm thấy!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void vCFullscreenIn_Click(object sender, EventArgs e)
        {
            var v = new ViewCamera(CameraDetectIn);
            v.ShowDialog();
        }

        private void vCFullscreenOut_Click(object sender, EventArgs e)
        {

            var v = new ViewCamera(CameraDetectOut);
            v.ShowDialog();
        }

        private void btnDetectIn_Click(object sender, EventArgs e)
        {
            Global.OpenCheckIn.PerformClick();
        }

        private void btnDetectOut_Click(object sender, EventArgs e)
        {
            Global.OpenCheckOut.PerformClick();
        }
    }
}