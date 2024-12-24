using LibVLCSharp.Shared;
using System; 
using LibVLCSharp.WPF;
using System;
using System.Windows;
using System.Windows.Controls;


namespace VCS.FORM.Views.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private LibVLC libVLC;
        private Media mediaIn;
        private Media mediaOut;
        private MediaPlayer playerIn;
        private MediaPlayer playerOut;
        private bool isPlaying = false;
        public Home()
        {
            InitializeComponent();
            InitializeLibVLC();
            InitializePlayer();
        }
        private void InitializeLibVLC()
        {
            // Khởi tạo LibVLC với các tùy chọn tối ưu cho streaming
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
                // Khởi tạo camera vào
                string rtspUrlIn = "rtsp://admin:D2s@2024@192.168.110.6/Streaming/Channels/1";
                mediaIn = new Media(libVLC, rtspUrlIn, FromType.FromLocation);
                playerIn = new MediaPlayer(mediaIn);
                CameraIn.MediaPlayer = playerIn;

                // Khởi tạo camera ra
                string rtspUrlOut = "rtsp://admin:D2s@2024@192.168.110.6/Streaming/Channels/1";
                mediaOut = new Media(libVLC, rtspUrlOut, FromType.FromLocation);
                playerOut = new MediaPlayer(mediaOut);
                CameraOut.MediaPlayer = playerOut;

                // Bắt đầu phát
                StartStreaming();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo camera: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartStreaming()
        {
            if (!isPlaying)
            {
                CameraIn.MediaPlayer?.Play();
                CameraOut.MediaPlayer?.Play();
                isPlaying = true;
            }
        }

        // Cleanup khi đóng page
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAndDisposeMediaPlayers();
        }

        private void StopAndDisposeMediaPlayers()
        {
            playerIn?.Stop();
            playerOut?.Stop();
            playerIn?.Dispose();
            playerOut?.Dispose();
            mediaIn?.Dispose();
            mediaOut?.Dispose();
            libVLC?.Dispose();
        }

    }
}
