using DMS.CORE;
using DMS.CORE.Entities.MD;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using VCS.APP.Utilities;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using VCS.APP.Services;

namespace VCS.APP.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private readonly AppDbContext _dbContext;
        private List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
         private Dictionary<string, LibVLCSharp.Shared.MediaPlayer> _mediaPlayers = new Dictionary<string, LibVLCSharp.Shared.MediaPlayer>();
        private LibVLCSharp.Shared.LibVLC? _libVLC;

        public CheckIn(AppDbContext dbContext)
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
                        && x.IsIn) // Lọc camera cổng vào
                    .ToList();

                InitializeCameraStreams();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDetect_Click(object sender, EventArgs e)
        {
            try
            {
                btnDetect.Enabled = false;
                var (filePath, snapshotImage) = await CommonService.TakeSnapshot(videoView.MediaPlayer);
                
                if (!string.IsNullOrEmpty(filePath))
                {
                    pictureBoxVehicle.Image = snapshotImage;
                    
                    var (licensePlate, croppedImage, savedImagePath) = await CommonService.DetectLicensePlateAsync(filePath);
                    
                    if (!string.IsNullOrEmpty(licensePlate))
                    {
                        txtLicensePlate.Text = licensePlate;
                        pictureBoxLicensePlate.Image = croppedImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDetect.Enabled = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                btnReset.Enabled = false;
                CleanupResources();
                InitializeLibVLC();
                GetListCameras();
                MessageBox.Show("Đã khởi tạo lại camera thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CommonService.CleanupCameraResources(_mediaPlayers, _libVLC);
            base.OnFormClosing(e);
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

        private void CleanupResources()
        {
            foreach (var player in _mediaPlayers.Values)
            {
                player.Stop();
                player.Dispose();
            }
            _mediaPlayers.Clear();
            
            _libVLC?.Dispose();
            _libVLC = null;
            
            videoView.MediaPlayer = null;
        }
    }
}
