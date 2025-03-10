using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DocumentFormat.OpenXml;
using ICSharpCode.SharpZipLib.Zip;
using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
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
using VCS.Services;

namespace VCS.Areas.CheckOut
{
    public partial class CheckOut : Form
    {
        private AppDbContextForm _dbContext;
        private MediaPlayer _mediaPlayer;
        private string IMGPATH;
        private string PLATEPATH;
        private List<DOSAPDataDto> _lstDOSAP = new List<DOSAPDataDto>();
        private List<string> lstPathImageCapture = new List<string>();
        private TblMdCamera CameraDetect { get; set; } = new TblMdCamera();
        private bool isHasInvoice { get; set; } = false;
        public CheckOut(AppDbContextForm dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            StreamCamera();
            GetListQueue();
        }

        #region Khởi tạo và stream camera
       
        private void StreamCamera()
        {
            try
            {
                var camera = Global.lstCamera.FirstOrDefault(x => x.IsOut && x.IsRecognition);
                CameraDetect = camera;
                if (camera != null)
                {
                    var media = new Media(Global._libVLC, camera.Rtsp, FromType.FromLocation);
                    var mediaPlayer = new MediaPlayer(media);
                    _mediaPlayer = mediaPlayer;
                    viewStream.MediaPlayer = mediaPlayer;
                    mediaPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo camera: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            base.OnFormClosing(e);
        }
        #endregion

        #region Nhận diện xe
        private async void btnDetect_Click(object sender, EventArgs e)
        {
            try
            {
                btnDetect.Enabled = false;
                var (filePath, snapshotImage) = CommonService.TakeSnapshot(viewStream.MediaPlayer);
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
                            selectVehicle.SelectedValue = i.FirstOrDefault().Id;
                        }

                        txtLicensePlate.Text = licensePlate;
                        pictureBoxLicensePlate.Image = croppedImage;
                    }
                }
                CommonService.Alert("Nhận diện biển số thành công!",Alert.Alert.enumType.Success);

                //Lưu các ảnh từ camera vào thư mục
                var lstCamera = Global.lstCamera.Where(x => x.IsIn == true && x.Code != CameraDetect.Code).ToList();
                lstPathImageCapture = new List<string>();
                foreach (var c in lstCamera)
                {
                    byte[] imageBytes = CommonService.CaptureFrameFromRTSP(c.Rtsp);
                    var path = CommonService.SaveDetectedImage(imageBytes);
                    lstPathImageCapture.Add(path);
                }
            }
            catch (Exception ex)
            {
                CommonService.Alert($"Lỗi không nhận diện được biển số!", Alert.Alert.enumType.Error);
                txtLicensePlate.Text = "";
            }
            finally
            {
                btnDetect.Enabled = true;
            }
        }
        #endregion

        #region Xử lý phương tiện chưa ra
        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuHeader.Where(x =>
            x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
            x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
            x.StatusVehicle != "04" && x.StatusVehicle != "01").ToList();
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            items.Add(new ComboBoxItem(" -", ""));
            foreach (var item in lstQueue)
            {
                var v = _dbContext.TblMdVehicle.FirstOrDefault(x => x.Code == item.VehicleCode)?.OicPbatch +
                    _dbContext.TblMdVehicle.FirstOrDefault(x => x.Code == item.VehicleCode)?.OicPtrip;
                items.Add(new ComboBoxItem($"{item.VehicleCode} - {v}", item.Id));
            }
            selectVehicle.DataSource = items;
            selectVehicle.DisplayMember = "Text";
            selectVehicle.ValueMember = "Value";
        }
        private void selectVehicle_DrawItem(object sender, DrawItemEventArgs e)
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
        #endregion


        private void btnResetForm_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
            string selectedValue = selectedItem.Value;
            if (string.IsNullOrEmpty(selectedValue))
            {
                CommonService.Alert($"Vui lòng chọn phương tiện!", Alert.Alert.enumType.Error);
                return;
            }
            ;
            if (txtLicensePlate.Text.Length > 8)
            {
                CommonService.Alert($"Biển số sai định dạng!", Alert.Alert.enumType.Error);
                return;
            }

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

        private void btnCheck_Click(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
            string selectedValue = selectedItem.Value;
            if (string.IsNullOrEmpty(selectedValue))
            {
                CommonService.Alert($"Vui lòng chọn phương tiện!", Alert.Alert.enumType.Error);
                return;
            }
            ;
            var number = "";
            var lstDo = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == selectedValue).ToList();
            foreach (var x in lstDo) number += x.Do1Sap + ", ";


            var dataDetail = CommonService.CheckInvoice(number);
            //if (!dataDetail.STATUS)
            //{
            //    lblStatus.Text = $"Lệnh xuất chưa có hoá đơn: {dataDetail.DATA}!";
            //    lblStatus.ForeColor = Color.Red;
            //    this.isHasInvoice = false;
            //}
            //else
            //{
            //    lblStatus.Text = $"Các lệnh xuất đã có hoá đơn!";
            //    lblStatus.ForeColor = Color.Green;
            //    this.isHasInvoice = true;
            //}
        }

        private void CheckOutProcess()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
            string selectedValue = selectedItem.Value;
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = selectedValue,
                Path = string.IsNullOrEmpty(IMGPATH) ? "" : IMGPATH.Replace(Global.PathSaveFile, ""),
                FullPath = string.IsNullOrEmpty(IMGPATH) ? "" : IMGPATH,
                InOut = "out",
                IsPlate = true,
                IsActive = true,
            });
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = selectedValue,
                Path = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH.Replace(Global.PathSaveFile, ""),
                FullPath = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH,
                InOut = "out",
                IsPlate = true,
                IsActive = true,
            });
            foreach (var o in lstPathImageCapture)
            {
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = selectedValue,
                    InOut = "out",
                    Path = string.IsNullOrEmpty(o) ? "" : o.Replace(Global.PathSaveFile, ""),
                    FullPath = string.IsNullOrEmpty(o) ? "" : o,
                    IsPlate = false,
                    IsActive = true
                });
            }
            var i = _dbContext.TblBuHeader.Find(selectedValue);
            i.IsCheckout = true;
            i.TimeCheckout = DateTime.Now;
            i.StatusVehicle = "04";
            i.NoteOut = txtNoteOut.Text;
            _dbContext.TblBuHeader.Update(i);
            _dbContext.SaveChanges();

            CommonService.Alert($"Cho xe ra khỏi kho thành công!", Alert.Alert.enumType.Success);
            ResetForm();
        }

        private void ResetForm()
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _lstDOSAP = new List<DOSAPDataDto>();
            lstPathImageCapture = new List<string>();
            IMGPATH = "";
            PLATEPATH = "";

            this.Controls.Clear();
            InitializeComponent();
            StreamCamera();
            GetListQueue();
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                var lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsOut)
                    .ToList();

                if (lstCamera.Any())
                {
                    AllCamera allCameraForm = new AllCamera(lstCamera);
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

        private void selectVehicle_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
                string selectedValue = selectedItem.Value;
                if (string.IsNullOrEmpty(selectedValue)) return;

                var detail = GetCheckInDetail(selectedValue);
                if (detail == null) return;
                txtLicensePlate.Text = detail.LicensePlate;
                _lstDOSAP.Clear();
                panelDODetail.Controls.OfType<DataGridView>().ToList()
                    .ForEach(x => { x.Dispose(); panelDODetail.Controls.Remove(x); });
                panelDODetail.Controls.OfType<Button>()
                    .Where(x => x.Size.Width == 30)
                    .ToList()
                    .ForEach(x => { x.Dispose(); panelDODetail.Controls.Remove(x); });

                _lstDOSAP.AddRange(detail.ListDOSAP);

                // Kiểm tra trạng thái xe

                var headerTgbx = _dbContext.TblBuHeaderTgbx.Where(x => x.HeaderId == selectedValue).ToList();
                var detailTgbx = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == selectedValue).ToList();
                var lstDo = detailTgbx.Select(x => x.SoLenh).Distinct().ToList();
                var vehicle = _dbContext.TblBuHeader.Find(selectedValue);
                if (vehicle.StatusProcess == "02" || vehicle.StatusProcess == "05" || lstDo.Count() == 0)
                {
                    CommonService.Alert($"Phương tiện không có ticket hoặc không xử lý!", Alert.Alert.enumType.Error);
                    this.isHasInvoice = true; // Bỏ qua kiểm tra hóa đơn
                    txtNoteOut.Text = "Phương tiện không có ticket hoặc không xử lý";
                    return;
                }

                foreach (var doSap in lstDo)
                {
                    var lstData = detailTgbx.Where(x => x.SoLenh == doSap).ToList();
                    AppendPanelDetail(lstData, headerTgbx.FirstOrDefault().MaPhuongTien);
                }
                CommonService.Alert($"Kiểm tra thông tin thành công!", Alert.Alert.enumType.Success);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AppendPanelDetail(List<TblBuDetailTgbx> data, string vehicleCode)
        {
            try
            {
                int yPosition = 6;
                var existingGrids = panelDODetail.Controls.OfType<DataGridView>().ToList();
                if (existingGrids.Any())
                {
                    yPosition = existingGrids.Last().Bottom + 6;
                }

                var res = CommonService.CheckInvoice(data.FirstOrDefault().SoLenh);
                var text = res.STATUS ? $"SỐ LỆNH XUẤT {data.FirstOrDefault().SoLenh} đã có hoá đơn" : $"SỐ LỆNH XUẤT {data.FirstOrDefault().SoLenh} chưa có hoá đơn";

                // CREATE LABEL
                var titleLabel = new Label
                {
                    Text = text,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, yPosition),
                };
                titleLabel.ForeColor = res.STATUS ? Color.Green : Color.Red;

                // DATA GRID VIEW
                var dataGridView1 = new DataGridView
                {
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                    ColumnHeadersHeight = 35,
                    Location = new Point(0, yPosition + 35),
                    Name = $"dataGridView_{panelDODetail.Controls.Count + 1}",
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.RowHeaderSelect,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        SelectionBackColor = Color.Transparent,
                        SelectionForeColor = Color.Black,
                        Padding = new Padding(5),
                        Font = new Font("Segoe UI", 12, FontStyle.Regular)
                    },
                    RowTemplate = { Height = 35 }
                };

                // HEADER STYLE
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 58, 64),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                };
                dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridView1.GridColor = Color.Gray;

                // CREATE DATA TABLE
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("SỐ LỆNH XUẤT", typeof(string));
                dataTable.Columns.Add("PHƯƠNG TIỆN", typeof(string));
                dataTable.Columns.Add("MẶT HÀNG", typeof(string));
                dataTable.Columns.Add("SỐ LƯỢNG (ĐVT)", typeof(string));

                foreach (var item in data)
                {
                    var materials = _dbContext.TblMdGoods.Find("000000000000" + item.MaHangHoa);
                    string materialName = materials?.Name ?? "Unknown";
                    dataTable.Rows.Add(data.FirstOrDefault().SoLenh, vehicleCode, materialName, $"{item.TongDuXuat} ({item.DonViTinh})");
                }

                dataGridView1.DataSource = dataTable;

                // CENTER ALIGNMENT
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // ADJUST GRID HEIGHT
                int totalHeight = dataGridView1.ColumnHeadersHeight + (dataTable.Rows.Count * dataGridView1.RowTemplate.Height) + 20;
                dataGridView1.Size = new Size(809, totalHeight);

                // ADD TO PANEL
                panelDODetail.Controls.Add(titleLabel);
                panelDODetail.Controls.Add(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\nVui lòng liên hệ quản trị viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        DATA = new Data
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
    }
}
