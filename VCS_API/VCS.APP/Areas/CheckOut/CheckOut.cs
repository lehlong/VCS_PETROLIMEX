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
            GetListQueue();
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
                        && x.IsOut && x.IsRecognition) // Lọc camera cổng ra
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

        private void btnCheckOut_Click(object sender, EventArgs e)
        {

        }

        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuHeader.Where(x => x.IsCheckout == false).ToList();
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            items.Add(new ComboBoxItem(" -", ""));
            foreach (var item in lstQueue)
            {
                var v = _dbContext.TblMdVehicle.FirstOrDefault(x => x.Code == item.VehicleCode)?.OicPbatch +
                    _dbContext.TblMdVehicle.FirstOrDefault(x => x.Code == item.VehicleCode)?.OicPtrip;
                items.Add(new ComboBoxItem($"{item.VehicleCode} - {v}", item.Id));
            }
            comboBox1.DataSource = items;
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
        }
        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;

            Color backColor = combo.BackColor;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backColor = Color.FromArgb(230, 230, 230);
            }
            else if ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
            {
                backColor = Color.FromArgb(245, 245, 245);
            }
            e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
            if (e.Index >= 0)
            {
                string text = combo.Items[e.Index].ToString();
                SizeF textSize = e.Graphics.MeasureString(text, e.Font);
                float yPos = e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2;

                e.Graphics.DrawString(text,
                    e.Font,
                    new SolidBrush(Color.Black),
                    new Point(e.Bounds.X + 3, (int)yPos));
            }
        }

        private void btnCheck_Click_1(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}
