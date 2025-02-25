using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Microsoft.EntityFrameworkCore;
using VCS.APP.Utilities;

namespace VCS.Areas.Home
{
    public partial class Home : Form
    {
        private readonly AppDbContextForm _dbContext;
        private LibVLC _libVLC;
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string, MediaPlayer>();

        public Home(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            InitializeLibVLC();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            Task.Run(InitializeCameraStreams);
        }

        private void InitializeLibVLC()
        {
            Core.Initialize();
            _libVLC = new LibVLC(
                "--network-caching=300",
                "--live-caching=300",
                "--file-caching=300",
                "--clock-jitter=0",
                "--clock-synchro=0",
                "--no-audio",
                "--rtsp-tcp",
                "--h264-hw-decoding"
            );
        }

        private async Task InitializeCameraStreams()
        {
            try
            {
                var cameras = await _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode && x.WarehouseCode == ProfileUtilities.User.WarehouseCode)
                    .ToListAsync();

                Invoke((MethodInvoker)delegate
                {
                    foreach (var camera in cameras)
                    {
                        AddCameraStream(camera);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách camera: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddCameraStream(TblMdCamera camera)
        {
            var cameraContainer = new Panel
            {
                Width = 640,
                Height = 360,
                Margin = new Padding(0, 0, 0, 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(52, 58, 64)
            };

            var videoView = new VideoView { Dock = DockStyle.Fill };
            var media = new Media(_libVLC, camera.Rtsp, FromType.FromLocation);
            var player = new MediaPlayer(media);

            videoView.MediaPlayer = player;
            _mediaPlayers[camera.Code] = player;
            cameraContainer.Controls.Add(videoView);
            (camera.IsIn ? cameraPanelIn : cameraPanelOut).Controls.Add(cameraContainer);
            player.Play();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (var player in _mediaPlayers.Values)
            {
                if (player.IsPlaying)
                    player.Stop();
                player.Dispose();
            }
            _mediaPlayers.Clear();
            cameraPanelIn.Controls.Clear();
            cameraPanelOut.Controls.Clear();
            _libVLC?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
