﻿using DMS.CORE;
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
        public List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        private LibVLC _libVLC;
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string, MediaPlayer>();

        public Home(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }
        private void Home_Load(object sender, EventArgs e)
        {
            InitializeLibVLC();
            GetListCameras();
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
        public void GetListCameras()
        {
            try
            {
                _lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                    && x.WarehouseCode == ProfileUtilities.User.WarehouseCode).ToList();
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
                    var cameraContainer = new Panel
                    {
                        Width = 640,
                        Height = 360,
                        Margin = new Padding(0,0,0,10), 
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khởi tạo camera {camera.Name}: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CleanupResources()
        {
            try
            {
                foreach (var player in _mediaPlayers.Values)
                {
                    player.Stop();
                    player.Dispose();
                }
                _mediaPlayers.Clear();
                cameraPanelIn.Controls.Clear();
                _libVLC?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi dọn dẹp resources: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CleanupResources();
                InitializeLibVLC();
                GetListCameras();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi reset camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CleanupResources();
            base.OnFormClosing(e);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
