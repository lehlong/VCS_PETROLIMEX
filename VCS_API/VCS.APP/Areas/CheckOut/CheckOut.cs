using DMS.BUSINESS.Dtos.SMO;
using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DocumentFormat.OpenXml;
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
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Services;
using VCS.APP.Utilities;

namespace VCS.APP.Areas.CheckOut
{
    public partial class CheckOut : Form
    {
        private readonly AppDbContext _dbContext;
        private LibVLC? _libVLC;
        private List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string, MediaPlayer>();
        private string IMGPATH;
        private string PLATEPATH;
        private List<DOSAPDataDto> _lstDOSAP = new List<DOSAPDataDto>();
        private bool isHasInvoice { get; set; } = false;
        public CheckOut(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            InitializeLibVLC();
        }
        private void CheckOut_Load(object sender, EventArgs e)
        {
            GetListCameras();
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

        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuHeader.Where(x =>
            x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
            x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
            x.IsCheckout == false).ToList();
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                btnReset.Enabled = true;
                CleanupResources();
                InitializeLibVLC();
                GetListCameras();
                txtStatus.Text = "Reset camera thành công!";
                txtStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private async void btnDetect_Click(object sender, EventArgs e)
        {
            try
            {
                btnDetect.Enabled = false;
                var (filePath, snapshotImage) = await CommonService.TakeSnapshot(videoView.MediaPlayer);
                IMGPATH = filePath;
                if (!string.IsNullOrEmpty(filePath))
                {
                    pictureBoxVehicle.Image = snapshotImage;

                    var (licensePlate, croppedImage, savedImagePath) = await CommonService.DetectLicensePlateAsync(filePath);
                    PLATEPATH = savedImagePath;
                    if (!string.IsNullOrEmpty(licensePlate))
                    {
                        var i = _dbContext.TblBuHeader.Where(x =>
                            x.VehicleCode == licensePlate &&
                            x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
                            x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
                            x.IsCheckout == false).ToList();
                        if (i.Count() == 1)
                        {
                            comboBox1.SelectedValue = i.FirstOrDefault().Id;
                        }

                        txtLicensePlate.Text = licensePlate;
                        pictureBoxLicensePlate.Image = croppedImage;
                    }
                }
                txtStatus.Text = "Nhận diện thành công";
                txtStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Lỗi không nhận diện được biển số";
                txtStatus.ForeColor = Color.Red;
                txtLicensePlate.Text = "";
            }
            finally
            {
                btnDetect.Enabled = true;
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                _lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsOut)
                    .ToList();

                if (_lstCamera.Any())
                {
                    AllCamera allCameraForm = new AllCamera(_lstCamera);
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

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                string selectedValue = selectedItem.Value;
                if (string.IsNullOrEmpty(selectedValue)) return;

                txtStatus.Text = "Đang kiểm tra thông tin! Vui lòng chờ...";
                txtStatus.ForeColor = Color.DarkGoldenrod;

                var detail = GetCheckInDetail(selectedValue);
                if (detail == null) return;
                txtLicensePlate.Text = detail.LicensePlate;
                _lstDOSAP.Clear();
                panel1.Controls.OfType<DataGridView>().ToList()
                    .ForEach(x => { x.Dispose(); panel1.Controls.Remove(x); });
                panel1.Controls.OfType<Button>()
                    .Where(x => x.Size.Width == 30)
                    .ToList()
                    .ForEach(x => { x.Dispose(); panel1.Controls.Remove(x); });

                _lstDOSAP.AddRange(detail.ListDOSAP);

                foreach (var doSap in detail.ListDOSAP)
                {
                    AppendPanelDetail(doSap);
                }
                var number = "";

                var lstDo = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == selectedValue).ToList();
                foreach (var x in lstDo) number += x.Do1Sap + ", ";
                var _s = new CommonService();
                var token = _s.LoginSmoApi();
                if (string.IsNullOrEmpty(token))
                {
                    txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }

                var dataDetail = _s.CheckInvoice(number, token);
                if (!dataDetail.STATUS)
                {
                    txtStatus.Text = $"Lệnh xuất chưa có hoá đơn: {dataDetail.DATA}!";
                    txtStatus.ForeColor = Color.Red;
                    this.isHasInvoice = false;
                }
                else
                {
                    txtStatus.Text = $"Các lệnh xuất đã có hoá đơn!";
                    txtStatus.ForeColor = Color.Green;
                    this.isHasInvoice = true;
                }

            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Lỗi khi tải thông tin: {ex.Message}";
                txtStatus.ForeColor = Color.Red;
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private CheckInDetailModel GetCheckInDetail(string headerId)
        {
            try
            {
                var header = _dbContext.TblBuHeader
                    .FirstOrDefault(x => x.Id == headerId);

                if (header == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy header với ID: {headerId}");
                }

                var result = new CheckInDetailModel
                {
                    LicensePlate = header.VehicleCode,
                    ListDOSAP = new List<DOSAPDataDto>()
                };

                var images = _dbContext.TblBuImage
                    .Where(x => x.HeaderId.Contains(headerId))
                    .ToArray();

                result.VehicleImagePath = images.FirstOrDefault(x => !x.IsPlate)?.FullPath;
                result.PlateImagePath = images.FirstOrDefault(x => x.IsPlate)?.FullPath;
                var doDetails = _dbContext.TblBuDetailDO
                    .Where(x => x.HeaderId == headerId)
                    .ToList();

                foreach (var doDetail in doDetails)
                {
                    var materials = _dbContext.TblBuDetailMaterial
                        .Where(x => x.HeaderId == doDetail.Id)
                        .ToList();

                    var doSapData = new DOSAPDataDto
                    {
                        STATUS = true,
                        DATA = new DMS.BUSINESS.Dtos.SMO.Data
                        {
                            VEHICLE = doDetail.VehicleCode,
                            LIST_DO = new List<DO>
                            {
                                new DO
                                {
                                    DO_NUMBER = doDetail.Do1Sap,
                                    LIST_MATERIAL = materials.Select(m => new LIST_MATERIAL
                                    {
                                        MATERIAL = m.MaterialCode,
                                        QUANTITY = m.Quantity,
                                        UNIT = m.UnitCode
                                    }).ToList()
                                }
                            }
                        }
                    };

                    result.ListDOSAP.Add(doSapData);
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin chi tiết: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void AppendPanelDetail(DOSAPDataDto data)
        {
            try
            {
                int yPosition = 155;
                if (_lstDOSAP.Count > 1)
                {
                    var existingGrids = panel1.Controls.OfType<DataGridView>().ToList();
                    if (existingGrids.Any())
                    {
                        var lastGrid = existingGrids.Last();
                        yPosition = lastGrid.Bottom + 1;
                    }
                }
                var dataGridView1 = new DataGridView
                {
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                    ColumnHeadersHeight = 35,
                    Location = new Point(18, yPosition),
                    Name = $"dataGridView_{_lstDOSAP.Count}",
                    TabIndex = 14,
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.CellSelect,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        SelectionBackColor = Color.Transparent,
                        SelectionForeColor = Color.Black,
                        Padding = new Padding(5, 0, 5, 0),
                        Font = new Font("Segoe UI", 12, FontStyle.Regular)
                    },
                    RowTemplate = { Height = 35 }
                };
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 58, 64),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                };
                dataGridView1.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
                dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridView1.GridColor = Color.Gray;
                System.Data.DataTable dataTable = new System.Data.DataTable();
                dataTable.Columns.Add("SỐ LỆNH XUẤT", typeof(string));
                dataTable.Columns.Add("PHƯƠNG TIỆN", typeof(string));
                dataTable.Columns.Add("MẶT HÀNG", typeof(string));
                dataTable.Columns.Add("SỐ LƯỢNG (ĐVT)", typeof(string));

                if (data.DATA.LIST_DO.FirstOrDefault() != null)
                {
                    foreach (var i in data.DATA.LIST_DO.FirstOrDefault().LIST_MATERIAL)
                    {
                        var materials = _dbContext.TblMdGoods.Find(i.MATERIAL);
                        dataTable.Rows.Add(
                            data.DATA.LIST_DO.FirstOrDefault()?.DO_NUMBER,
                            data.DATA.VEHICLE,
                            materials?.Name,
                            $"{i.QUANTITY} ({i.UNIT})"
                        );
                    }
                }

                dataGridView1.DataSource = dataTable;

                // Căn giữa nội dung trong các ô cột
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                // Tính chiều cao tổng cộng của bảng
                int totalHeight = dataGridView1.ColumnHeadersHeight +
                    (dataTable.Rows.Count * dataGridView1.RowTemplate.Height) + 20;
                dataGridView1.Size = new System.Drawing.Size(809, totalHeight);

                // Thêm bảng vào panel
                panel1.Controls.Add(dataGridView1);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vui lòng liên hệ đến quản trị viên hệ thống: {ex.Message}",
                        "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            string selectedValue = selectedItem.Value;
            if (string.IsNullOrEmpty(selectedValue))
            {
                txtStatus.Text = "Vui lòng chọn phương tiện!";
                txtStatus.ForeColor = Color.Red;
                return;
            };

            if (!this.isHasInvoice)
            {
                var result = MessageBox.Show("Hoá đơn chưa đầy đủ! Bạn có chắc chắn muốn cho xe ra!",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    CheckOutProcess();
                }
            }
            else
            {
                CheckOutProcess();
            }
        }

        private void ReloadForm()
        {
            this.Controls.Clear();
            InitializeComponent();
            InitializeLibVLC();
            GetListCameras();
            GetListQueue();
        }
        private void CheckOutProcess()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            string selectedValue = selectedItem.Value;
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = selectedValue,
                Path = IMGPATH.Replace(Global.PathSaveFile,""),
                FullPath = IMGPATH,
                InOut = "out",
                IsPlate = true,
                IsActive = true,
            });
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = selectedValue,
                Path = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH.Replace(Global.PathSaveFile, ""),
                FullPath = PLATEPATH,
                InOut = "out",
                IsPlate = true,
                IsActive = true,
            });
            var i = _dbContext.TblBuHeader.Find(selectedValue);
            i.IsCheckout = true;
            i.TimeCheckout = DateTime.Now;
            i.NoteOut = this.isHasInvoice ? "" : "Chưa đủ hoá đơn";
            _dbContext.TblBuHeader.Update(i);
            _dbContext.SaveChanges();
            ReloadForm();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ReloadForm();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            string selectedValue = selectedItem.Value;
            if (string.IsNullOrEmpty(selectedValue))
            {
                txtStatus.Text = "Vui lòng chọn phương tiện!";
                txtStatus.ForeColor = Color.Red;
                return;
            };
            var number = "";
            var lstDo = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == selectedValue).ToList();
            foreach (var x in lstDo) number += x.Do1Sap + ", ";
            var _s = new CommonService();
            var token = _s.LoginSmoApi();
            if (string.IsNullOrEmpty(token))
            {
                txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
                txtStatus.ForeColor = Color.Red;
                return;
            }

            var dataDetail = _s.CheckInvoice(number, token);
            if (!dataDetail.STATUS)
            {
                txtStatus.Text = $"Lệnh xuất chưa có hoá đơn: {dataDetail.DATA}!";
                txtStatus.ForeColor = Color.Red;
                this.isHasInvoice = false;
            }
            else
            {
                txtStatus.Text = $"Các lệnh xuất đã có hoá đơn!";
                txtStatus.ForeColor = Color.Green;
                this.isHasInvoice = true;
            }
        }
    }
}
