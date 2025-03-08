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
using VCS.APP.Utilities;
using Media = LibVLCSharp.Shared.Media;

namespace VCS.Areas.Home
{
    public partial class Home : Form
    {
        private readonly AppDbContextForm _dbContext;
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string, MediaPlayer>();

        public Home(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }
        private void Home_Load(object sender, EventArgs e)
        {
            InitializeCameraStreams();
        }

        
        private async void InitializeCameraStreams()
        {
            try
            {

                foreach (var camera in Global.lstCamera)
                {
                    AddCameraStream(camera);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddCameraStream(TblMdCamera camera)
        {
            var cameraContainer = new Panel
            {
                Width = 615,
                Height = 335,
                Margin = new Padding(0, 0, 0, 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(52, 58, 64)
            };

            var plateText = camera.IsRecognition ? "(Camera nhận diện)" : "";
            var label = new Label
            {
                Text = camera.IsIn ? $"{camera.Name} - CAMERA CỔNG VÀO {plateText}" : $"{camera.Name} - CAMERA CỔNG RA {plateText}",
                Dock = DockStyle.Top,
                Height = 30,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 58, 64),
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };

            cameraContainer.Controls.Add(label);

            var videoView = new VideoView
            {
                Dock = DockStyle.Fill
            };

            string rtspUrl = camera.Rtsp;
            var media = new Media(Global._libVLC, rtspUrl, FromType.FromLocation);
            var player = new MediaPlayer(media);

            videoView.MediaPlayer = player;
            _mediaPlayers[camera.Code] = player;

            cameraContainer.Controls.Add(videoView);

            if (camera.IsIn)
            {
                cameraPanelIn.Controls.Add(cameraContainer);
            }
            else
            {
                cameraPanelOut.Controls.Add(cameraContainer);
            }

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

            base.OnFormClosing(e);
        }
    }
}
