using LibVLCSharp.Shared;
using VCS.APP.Utilities;
using VCS.DbContext.Entities.MD;

namespace VCS.Areas.ViewAllCamera
{
    public partial class ViewCamera : Form
    {
        private MediaPlayer _mediaPlayer;
        public ViewCamera(TblMdCamera camera)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            LibVLCSharp.WinForms.VideoView v = new LibVLCSharp.WinForms.VideoView();
            v.Dock = DockStyle.Fill;
            var media = new Media(Global._libVLC, camera.Rtsp, FromType.FromLocation);
            var mediaPlayer = new MediaPlayer(media);
            _mediaPlayer = mediaPlayer;
            v.MediaPlayer = mediaPlayer;
            mediaPlayer.Play();
            this.Controls.Add(v);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
