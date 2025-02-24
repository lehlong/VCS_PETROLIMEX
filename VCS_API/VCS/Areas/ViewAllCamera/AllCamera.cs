using DMS.CORE.Entities.MD;
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
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace VCS.APP.Areas.ViewAllCamera
{
    public partial class AllCamera : Form
    {

        private LibVLC _libVLC;
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string, MediaPlayer>();
        public List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        public AllCamera(List<TblMdCamera> lstCamera)
        {
            InitializeComponent();
            _lstCamera = lstCamera;
            InitializeLibVLC();
            InitializeCameraStreams();
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
        private void InitializeCameraStreams()
        {
            foreach (var camera in _lstCamera)
            {
                try
                {
                    var cameraContainer = new Panel
                    {
                        Width = 620,
                        Height = 360,
                        Margin = new Padding(10),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.FromArgb(52, 58, 64)
                    };

                    var label = new Label
                    {
                        Text = camera.IsIn ? $"{camera.Name} - CAMERA CỔNG VÀO" : $"{camera.Name} - CAMERA CỔNG RA ",
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
                        //Width = 620,
                        //Height = 360,
                        Dock = DockStyle.Fill
                    };

                    string rtspUrl = $"{camera.Rtsp}";
                    var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
                    var player = new MediaPlayer(media);

                    videoView.MediaPlayer = player;
                    _mediaPlayers[camera.Code] = player;

                    cameraContainer.Controls.Add(videoView);
                    cameraPanel.Controls.Add(cameraContainer);

                    player.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khởi tạo camera {camera.Name}: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (var player in _mediaPlayers.Values)
            {
                player.Stop();
                player.Dispose();
            }
            _libVLC.Dispose();
            base.OnFormClosing(e);
        }
    }
}
