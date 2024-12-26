using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using VCS.FORM.Model;

namespace VCS.FORM.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomePages.xaml
    /// </summary>
    public partial class HomePages : Page
    {
        private LibVLC libVLC;
        private ObservableCollection<CameraViewModel> Cameras { get; set; }

        public HomePages()
        {
            InitializeComponent();
            Cameras = new ObservableCollection<CameraViewModel>();
            CameraItemsControl.ItemsSource = Cameras;
            InitializeLibVLC();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePlayer();
        }

        private void InitializeLibVLC()
        {
            Core.Initialize();
            libVLC = new LibVLC(
                "--network-caching=100",
                "--live-caching=100",
                "--file-caching=100",
                "--clock-jitter=0",
                "--clock-synchro=0",
                "--no-audio",
                "--rtsp-tcp"
            );
        }

        private void InitializePlayer()
        {
            try
            {
                // Ví dụ thêm camera
                AddCamera("Camera cổng vào 1", "rtsp://admin:D2s@2024@192.168.110.6/Streaming/Channels/1");
                AddCamera("Camera cổng ra 1", "rtsp://admin:D2s@2024@192.168.110.6/Streaming/Channels/1");
                AddCamera("Camera cổng vào 2", "rtsp://admin:D2s@2024@192.168.110.6/Streaming/Channels/1");
                // Thêm camera khác nếu cần
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo camera: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCamera(string name, string rtspUrl)
        {
            var media = new Media(libVLC, rtspUrl, FromType.FromLocation);
            var player = new MediaPlayer(media);
            var camera = new CameraViewModel
            {
                CameraName = name,
                MediaPlayer = player
            };
            Cameras.Add(camera);
            player.Play();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAndDisposeMediaPlayers();
        }

        private void StopAndDisposeMediaPlayers()
        {
            foreach (var camera in Cameras)
            {
                camera.MediaPlayer?.Stop();
                camera.MediaPlayer?.Dispose();
            }
            Cameras.Clear();
            libVLC?.Dispose();
        }
    }

}
