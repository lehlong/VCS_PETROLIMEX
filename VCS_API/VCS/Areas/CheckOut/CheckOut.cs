using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.Areas.ViewAllCamera;
using VCS.DbContext.Common;
using VCS.DbContext.Entities.BU;
using VCS.DbContext.Entities.MD;
using VCS.Services;

namespace VCS.Areas.CheckOut
{
    public partial class CheckOut : Form
    {
        private AppDbContextForm _dbContext;
        private MediaPlayer _mediaPlayer;
        private string IMGPATH;
        private string PLATEPATH;
        private List<string> lstPathImageCapture = new List<string>();
        private bool IsCancel { get; set; } = false;
        private TblMdCamera CameraDetect { get; set; } = new TblMdCamera();
        private bool isHasInvoice { get; set; } = true;
        private bool isTriggerDetect { get; set; } = false;
        public CheckOut(AppDbContextForm dbContext, bool isTriggerDetect)
        {
            _dbContext = dbContext;
            this.isTriggerDetect = isTriggerDetect;
            InitializeComponent();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            StreamCamera();
            GetListQueue();
        }

        #region Khởi tạo và stream camera

        private async void StreamCamera()
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

                    if (isTriggerDetect)
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            await Task.Delay(500);
                            if (viewStream.MediaPlayer.IsPlaying)
                            {
                                btnDetect.PerformClick();
                                break;
                            }
                        }
                    }
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
            base.OnFormClosing(e);
            Task.Run(() =>
            {
                _mediaPlayer?.Stop();
                _mediaPlayer?.Dispose();
            });
        }
        #endregion

        #region Nhận diện xe

        private async void btnDetect_Click(object sender, EventArgs e)
        {
            try
            {
                var player = viewStream.MediaPlayer;
                if (player?.IsPlaying != true)
                {
                    CommonService.Alert("Không thể nhận diện khi camera không hoạt động!", Alert.Alert.enumType.Error);
                    return;
                }

                // Tạo đường dẫn thư mục và tệp ảnh
                string snapshotDir = Path.Combine(Global.PathSaveFile, DateTime.Now.ToString("yyyy/MM/dd"));
                Directory.CreateDirectory(snapshotDir);
                string snapshotPath = Path.Combine(snapshotDir, $"{Guid.NewGuid()}.jpg");
                string croppedPath = Path.Combine(snapshotDir, $"{Guid.NewGuid()}.jpg");

                // Chụp ảnh
                player.TakeSnapshot(0, snapshotPath, Global.CropWidth, Global.CropHeight);
                if (!File.Exists(snapshotPath))
                {
                    CommonService.Alert("Không thể chụp ảnh!", Alert.Alert.enumType.Error);
                    return;
                }

                pictureBoxVehicle.Image = new Bitmap(snapshotPath);
                IMGPATH = snapshotPath;

                // Nhận diện biển số
                var result = CommonService.DetectLicensePlate(snapshotPath, croppedPath);
                if (result?.ImageCrop == null || string.IsNullOrEmpty(result.LicensePlateNumber))
                {
                    CommonService.Alert("Không nhận diện được biển số!", Alert.Alert.enumType.Error);
                    return;
                }
                PLATEPATH = croppedPath;

                // Hiển thị kết quả
                pictureBoxLicensePlate.Image = result.ImageCrop;
                txtLicensePlate.Text = result.LicensePlateNumber;

                var headerId = await _dbContext.TblBuHeader.AsNoTracking().Where(x =>
                           x.VehicleCode == result.LicensePlateNumber &&
                           x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
                           x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
                           x.StatusVehicle != "04" && x.StatusVehicle != "05").Select(x => x.Id).FirstOrDefaultAsync();
                if (headerId != null)
                {
                    selectVehicle.SelectedValue = headerId;
                }
                else
                {
                    CommonService.Alert("Phương tiện không có trong kho! Vui lòng nhận diện lại!", Alert.Alert.enumType.Error);
                    return;
                }
                // Chụp từ các camera khác
                lstPathImageCapture = Global.lstCamera
                    .Where(x => x.IsOut && x.Code != CameraDetect.Code)
                    .Select(c => CommonService.SaveDetectedImage(CommonService.CaptureFrameFromRTSP(c.Rtsp)))
                    .ToList();
            }
            catch (Exception ex)
            {
                CommonService.Alert($"Lỗi không nhận diện được: {ex.Message} - {ex.StackTrace}", Alert.Alert.enumType.Error);
            }
        }

        #endregion

        #region Xử lý phương tiện chưa ra
        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuHeader.Where(x =>
            x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
            x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
            x.StatusVehicle != "04" && x.StatusVehicle != "01" && x.StatusProcess != "05").ToList();
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            items.Add(new ComboBoxItem("-", ""));
            foreach (var item in lstQueue)
            {
                items.Add(new ComboBoxItem($"{item.VehicleCode} - {item.VehicleName}", item.Id));
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



        private void CheckOutProcess()
        {
            try
            {
                if (selectVehicle.SelectedItem is not ComboBoxItem selectedItem || string.IsNullOrEmpty(selectedItem.Value))
                {
                    CommonService.Alert("Vui lòng chọn phương tiện!", Alert.Alert.enumType.Warning);
                    return;
                }

                string selectedValue = selectedItem.Value;

                if (!string.IsNullOrEmpty(IMGPATH))
                {
                    _dbContext.TblBuImage.Add(new TblBuImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = selectedValue,
                        Path = IMGPATH.Replace(Global.PathSaveFile, ""),
                        FullPath = IMGPATH,
                        InOut = "out",
                        IsPlate = false,
                        IsActive = true,
                    });
                }

                if (!string.IsNullOrEmpty(PLATEPATH))
                {
                    _dbContext.TblBuImage.Add(new TblBuImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = selectedValue,
                        Path = PLATEPATH.Replace(Global.PathSaveFile, ""),
                        FullPath = PLATEPATH,
                        InOut = "out",
                        IsPlate = true,
                        IsActive = true
                    });
                }

                if (lstPathImageCapture != null)
                {
                    foreach (var o in lstPathImageCapture)
                    {
                        if (string.IsNullOrWhiteSpace(o)) continue;

                        _dbContext.TblBuImage.Add(new TblBuImage
                        {
                            Id = Guid.NewGuid().ToString(),
                            HeaderId = selectedValue,
                            InOut = "out",
                            Path = o.Replace(Global.PathSaveFile, ""),
                            FullPath = o,
                            IsPlate = false,
                            IsActive = true
                        });
                    }
                }

                var i = _dbContext.TblBuHeader.Find(selectedValue);
                if (i == null)
                {
                    CommonService.Alert("Không tìm thấy thông tin xe!", Alert.Alert.enumType.Error);
                    return;
                }

                var lstCheckDoCheckIn = _dbContext.TblBuDetailDO
                    .Where(x => x.HeaderId == selectedValue)
                    .Select(x => x.Do1Sap)
                    .Distinct()
                    .ToList();

                var lstCheckDoTgbx = _dbContext.TblBuHeaderTgbx
                    .Where(x => x.HeaderId == selectedValue)
                    .Select(x => x.SoLenh)
                    .Distinct()
                    .ToList();

                i.IsCheckout = true;
                i.TimeCheckout = DateTime.Now;
                i.StatusVehicle = "04";
                i.NoteOut = txtNoteOut?.Text ?? "";
                _dbContext.TblBuHeader.Update(i);

                var w = _dbContext.TblMdWarehouse.Find(ProfileUtilities.User?.WarehouseCode);
                if (w?.Is_sms_out == true)
                {
                    var sms = _dbContext.TblAdSmsConfig.Find("SMS");
                    if (sms != null)
                    {
                        foreach (var _do in lstCheckDoTgbx)
                        {
                            var data = CommonService.GetDetailDO(_do, "1");
                            var firstDo = data?.DATA?.LIST_DO?.FirstOrDefault();
                            if (firstDo == null) continue;

                            _dbContext.TblBuSmsQueue.Add(new TblBuSmsQueue
                            {
                                Id = Guid.NewGuid().ToString(),
                                Phone = firstDo.PHONE ?? "",
                                SmsContent = sms.SmsOut?
                                    .Replace("[PHUONG_TIEN]", txtLicensePlate?.Text ?? "")
                                    .Replace("[KHACH_HANG]", firstDo.CUSTOMER_NAME ?? "")
                                    .Replace("[LENH_XUAT]", firstDo.DO_NUMBER ?? "")
                                    .Replace("[THOI_GIAN]", DateTime.Now.ToString("dd/MM/yyyy HH:mm")),
                                IsSend = false,
                                IsActive = true,
                                Count = 0,
                            });
                        }
                    }
                }

                _dbContext.SaveChanges();

                if (!string.IsNullOrWhiteSpace(IMGPATH)) lstPathImageCapture.Add(IMGPATH);
                if (!string.IsNullOrWhiteSpace(PLATEPATH)) lstPathImageCapture.Add(PLATEPATH);

                var filesToUpload = lstPathImageCapture
                    ?.Where(s => !string.IsNullOrWhiteSpace(s) && s != "undefined")
                    .ToList();

                if (filesToUpload != null && filesToUpload.Count > 0)
                {
                    CommonService.UploadImagesServer(filesToUpload);
                }

                try
                {
                    string licensePlate = txtLicensePlate?.Text ?? "";

                    if (IsCancel)
                    {
                        var model = new PostStatusVehicleToSMO
                        {
                            VEHICLE = licensePlate,
                            TYPE = "CANCEL",
                            LIST_DO = string.Join(",", lstCheckDoCheckIn),
                            DATE_INFO = DateTime.Now,
                        };
                        CommonService.PostStatusVehicleToSMO(model);
                    }
                    else
                    {
                        var model = new PostStatusVehicleToSMO
                        {
                            VEHICLE = licensePlate,
                            TYPE = "OUT",
                            LIST_DO = string.Join(",", lstCheckDoTgbx),
                            DATE_INFO = DateTime.Now,
                        };
                        CommonService.PostStatusVehicleToSMO(model);

                        var diff = lstCheckDoCheckIn.Except(lstCheckDoTgbx).ToList();
                        if (diff.Count > 0)
                        {
                            var model_cancel = new PostStatusVehicleToSMO
                            {
                                VEHICLE = licensePlate,
                                TYPE = "CANCEL",
                                LIST_DO = string.Join(",", diff),
                                DATE_INFO = DateTime.Now,
                            };
                            CommonService.PostStatusVehicleToSMO(model_cancel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonService.Alert("Lỗi hệ thống! Cho xe ra thất bại!", Alert.Alert.enumType.Success);
                }

                CommonService.Alert("Cho xe ra khỏi kho thành công!", Alert.Alert.enumType.Success);
                ResetForm();
            }
            catch (Exception ex)
            {
                CommonService.Alert("Lỗi! Vui lòng khởi động lại hệ thống!", Alert.Alert.enumType.Error);
            }
        }


        private void ResetForm()
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
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

        private async void selectVehicle_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                _dbContext.ChangeTracker.Clear();
                ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
                string selectedValue = selectedItem.Value;
                if (string.IsNullOrEmpty(selectedValue)) return;

                var detail = await Task.Run(() => GetCheckInDetail(selectedValue));
                if (detail == null) return;

                panelCheckIn.Controls.Clear();
                panelCheckOut.Controls.Clear();

                await Task.Run(() =>
                {
                    foreach (var doSap in detail.ListDOSAP)
                    {
                        Invoke(new Action(() => AppendPanelDetailCheckIn(doSap)));
                    }
                });

                // Truy vấn dữ liệu xuất kho
                var headerTgbx = await Task.Run(() => _dbContext.TblBuHeaderTgbx.Where(x => x.HeaderId == selectedValue).ToList());
                var detailTgbx = await Task.Run(() => _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == selectedValue).ToList());
                var lstDo = detailTgbx.Select(x => x.SoLenh).Distinct().ToList();
                var vehicle = await Task.Run(() => _dbContext.TblBuHeader.Find(selectedValue));

                // Append CheckOut Panels
                if (lstDo.Count > 0)
                {
                    await Task.Run(() =>
                    {
                        foreach (var doSap in lstDo)
                        {
                            var lstData = detailTgbx.Where(x => x.SoLenh == doSap).ToList();
                            Invoke(new Action(() => AppendPanelDetailChechkOut(lstData, headerTgbx.FirstOrDefault()?.MaPhuongTien, false)));
                        }
                    });
                }

                // Kiểm tra trạng thái và cảnh báo
                if (vehicle.StatusProcess == "02" || vehicle.StatusProcess == "05")
                {
                    CommonService.Alert($"Phương tiện không được xử lý!", Alert.Alert.enumType.Error);
                    txtNoteOut.Text = "Phương tiện không được xử lý!";
                    IsCancel = true;
                    return;
                }

                if (vehicle.StatusProcess == "02" || lstDo.Count == 0)
                {
                    CommonService.Alert($"Phương tiện không có ticket!", Alert.Alert.enumType.Error);
                    txtNoteOut.Text = "Phương tiện không có ticket!";
                    IsCancel = true;
                    return;
                }
                IsCancel = false;
                CommonService.Alert($"Kiểm tra thông tin thành công!", Alert.Alert.enumType.Success);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
            string selectedValue = selectedItem.Value;
            if (string.IsNullOrEmpty(selectedValue))
            {
                CommonService.Alert($"Vui lòng chọn phương tiện!", Alert.Alert.enumType.Error);
                return;
            }
            panelCheckOut.Controls.Clear();
            // Truy vấn dữ liệu xuất kho
            var headerTgbx = await Task.Run(() => _dbContext.TblBuHeaderTgbx.Where(x => x.HeaderId == selectedValue).ToList());
            var detailTgbx = await Task.Run(() => _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == selectedValue).ToList());
            var lstDo = detailTgbx.Select(x => x.SoLenh).Distinct().ToList();
            var vehicle = await Task.Run(() => _dbContext.TblBuHeader.Find(selectedValue));

            // Append CheckOut Panels
            if (lstDo.Count > 0)
            {
                await Task.Run(() =>
                {
                    foreach (var doSap in lstDo)
                    {
                        var lstData = detailTgbx.Where(x => x.SoLenh == doSap).ToList();
                        Invoke(new Action(() => AppendPanelDetailChechkOut(lstData, headerTgbx.FirstOrDefault()?.MaPhuongTien, true)));
                    }
                });
            }

        }

        private void AppendPanelDetailChechkOut(List<TblBuDetailTgbx> data, string vehicleCode, bool isCheck)
        {
            try
            {
                var text = "";
                var status = false;
                if (isCheck)
                {
                    var res = CommonService.CheckInvoice(data.FirstOrDefault().SoLenh);
                    text = res.STATUS ? $"ĐÃ XUẤT HOÁ ĐƠN" : $"CHƯA XUẤT HOÁ ĐƠN";
                    status = res.STATUS;
                }

                int yPosition = panelCheckOut.Controls.OfType<Panel>().Any()
                    ? panelCheckOut.Controls.OfType<Panel>().Max(p => p.Bottom) + 6
                    : 6;

                var containerPanel = new Panel
                {
                    Name = $"panel_{data.FirstOrDefault().SoLenh}",
                    BackColor = Color.WhiteSmoke,
                    Location = new Point(0, yPosition),
                    Size = new Size(860, 10),
                    Padding = new Padding(12),
                    BorderStyle = BorderStyle.None
                };

                int innerY = 6;
                var customerLabel = new Label
                {
                    Text = text,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(6, innerY),
                    ForeColor = status ? Color.Green : Color.Red
                };
                innerY += customerLabel.Height + 6;
                containerPanel.Controls.Add(customerLabel);

                var dataGridView = new DataGridView
                {
                    BackgroundColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                    ColumnHeadersHeight = 40,
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    Location = new Point(6, innerY),
                    Margin = new Padding(6),
                    Width = 750,
                    RowTemplate = { Height = 40 }
                };

                dataGridView.CellClick += (sender, e) => { };
                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 58, 64),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(6)
                };
                dataGridView.GridColor = Color.Gray;
                dataGridView.DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    SelectionBackColor = Color.WhiteSmoke,
                    SelectionForeColor = Color.Black,
                    Padding = new Padding(6)
                };

                dataGridView.CellMouseEnter += (sender, e) => { };
                dataGridView.SelectionChanged += (sender, e) => dataGridView.ClearSelection();

                var dataTable = new DataTable();
                dataTable.Columns.Add("SỐ LỆNH XUẤT", typeof(string));
                dataTable.Columns.Add("PHƯƠNG TIỆN", typeof(string));
                dataTable.Columns.Add("MẶT HÀNG", typeof(string));
                dataTable.Columns.Add("SỐ LƯỢNG (ĐVT)", typeof(string));

                foreach (var item in data)
                {
                    var materials = _dbContext.TblMdGoods.Find("00000000000" + item.MaHangHoa.Replace(" ", "").Trim());
                    string materialName = materials?.Name ?? "Unknown";
                    dataTable.Rows.Add(data.FirstOrDefault().SoLenh, vehicleCode, materialName, $"{item.TongDuXuat?.ToString("#,#")} ({item.DonViTinh})");
                }

                dataGridView.DataSource = dataTable;

                int totalGridViewHeight = dataGridView.ColumnHeadersHeight + (dataTable.Rows.Count * dataGridView.RowTemplate.Height) + 6;
                dataGridView.Size = new Size(790, totalGridViewHeight);

                containerPanel.Size = new Size(802, totalGridViewHeight + innerY + 6);
                containerPanel.Controls.Add(dataGridView);

                panelCheckOut.Controls.Add(containerPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendPanelDetailCheckIn(DOSAPDataDto data)
        {
            try
            {
                if (data?.DATA?.LIST_DO?.Any() != true || data.DATA.LIST_DO.FirstOrDefault() == null)
                    return;

                var firstDo = data.DATA.LIST_DO.FirstOrDefault();
                string doNumber = firstDo.DO_NUMBER;

                int yPosition = panelCheckIn.Controls.OfType<Panel>().Any()
                    ? panelCheckIn.Controls.OfType<Panel>().Max(p => p.Bottom) + 6
                    : 6;

                var containerPanel = new Panel
                {
                    Name = $"panel_{doNumber}",
                    BackColor = Color.WhiteSmoke,
                    Location = new Point(0, yPosition),
                    Size = new Size(860, 10),
                    Padding = new Padding(12),
                    BorderStyle = BorderStyle.None
                };

                int innerY = 6;
                var customerLabel = new Label
                {
                    Text = firstDo.CUSTOMER_NAME ?? "Không xác định",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(6, innerY),
                    ForeColor = Color.Black
                };
                innerY += customerLabel.Height + 6;
                containerPanel.Controls.Add(customerLabel);

                var nguonLabel = new Label
                {
                    Text = $"{CommonService.GetText(firstDo.MODUL_TYPE)} - {firstDo.NGUON_HANG ?? "Không xác định"}",
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    AutoSize = true,
                    Location = new Point(6, innerY),
                    ForeColor = Color.Black
                };
                innerY += nguonLabel.Height + 6;
                containerPanel.Controls.Add(nguonLabel);

                var dataGridView = new DataGridView
                {
                    BackgroundColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                    ColumnHeadersHeight = 40,
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    Location = new Point(6, innerY),
                    Margin = new Padding(6),
                    Width = 750,
                    RowTemplate = { Height = 40 }
                };

                dataGridView.CellClick += (sender, e) => { };
                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 58, 64),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(6)
                };
                dataGridView.GridColor = Color.Gray;
                dataGridView.DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    SelectionBackColor = Color.WhiteSmoke,
                    SelectionForeColor = Color.Black,
                    Padding = new Padding(6)
                };

                dataGridView.CellMouseEnter += (sender, e) => { };
                dataGridView.SelectionChanged += (sender, e) => dataGridView.ClearSelection();

                var dataTable = new DataTable();
                dataTable.Columns.Add("SỐ LỆNH XUẤT", typeof(string));
                dataTable.Columns.Add("PHƯƠNG TIỆN", typeof(string));
                dataTable.Columns.Add("MẶT HÀNG", typeof(string));
                dataTable.Columns.Add("SỐ LƯỢNG (ĐVT)", typeof(string));

                foreach (var item in firstDo.LIST_MATERIAL)
                {
                    var materials = _dbContext.TblMdGoods.Find(item.MATERIAL);
                    string materialName = materials?.Name ?? "Không xác định";

                    dataTable.Rows.Add(
                        doNumber,
                        data.DATA.VEHICLE ?? "Không xác định",
                        materialName,
                        $"{item.QUANTITY.ToString("#,#")} ({item.UNIT ?? "N/A"})"
                    );
                }

                dataGridView.DataSource = dataTable;

                int totalGridViewHeight = dataGridView.ColumnHeadersHeight + (dataTable.Rows.Count * dataGridView.RowTemplate.Height) + 6;
                dataGridView.Size = new Size(790, totalGridViewHeight);

                containerPanel.Size = new Size(802, totalGridViewHeight + innerY + 6);
                containerPanel.Controls.Add(dataGridView);

                panelCheckIn.Controls.Add(containerPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private CheckInDetailModel GetCheckInDetail(string headerId)
        {
            try
            {
                var header = _dbContext.TblBuHeader.Find(headerId);

                if (header == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy header với ID: {headerId}");
                }

                var result = new CheckInDetailModel
                {
                    LicensePlate = header.VehicleCode,
                    VehicleName = header.VehicleName,
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
                                    NGUON_HANG = doDetail.NguonHang,
                                    TANK_GROUP = doDetail.TankGroup,
                                    MODUL_TYPE = doDetail.ModulType,
                                    CUSTOMER_CODE = doDetail.CustomerCode,
                                    CUSTOMER_NAME = doDetail.CustomerName,
                                    PHONE = doDetail.Phone,
                                    EMAIL = doDetail.Email,
                                    TAI_XE = doDetail.TaiXe,

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

        private void pictureBoxVehicle_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;

            if (clickedPictureBox != null && clickedPictureBox.Image != null)
            {
                Form fullscreenForm = new Form();
                fullscreenForm.WindowState = FormWindowState.Maximized;
                fullscreenForm.FormBorderStyle = FormBorderStyle.FixedSingle;

                PictureBox fullscreenPictureBox = new PictureBox();
                fullscreenPictureBox.Image = clickedPictureBox.Image;
                fullscreenPictureBox.Dock = DockStyle.Fill;
                fullscreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                fullscreenForm.Controls.Add(fullscreenPictureBox);
                fullscreenForm.ShowDialog();
            }
        }

        private void pictureBoxLicensePlate_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;

            if (clickedPictureBox != null && clickedPictureBox.Image != null)
            {
                Form fullscreenForm = new Form();
                fullscreenForm.WindowState = FormWindowState.Maximized;
                fullscreenForm.FormBorderStyle = FormBorderStyle.FixedSingle;

                PictureBox fullscreenPictureBox = new PictureBox();
                fullscreenPictureBox.Image = clickedPictureBox.Image;
                fullscreenPictureBox.Dock = DockStyle.Fill;
                fullscreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                fullscreenForm.Controls.Add(fullscreenPictureBox);
                fullscreenForm.ShowDialog();
            }
        }

        private void viewCameraFullscreen_Click(object sender, EventArgs e)
        {
            var v = new ViewCamera(CameraDetect);
            v.ShowDialog();
        }

        private void txtLicensePlate_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                txtVehicleName.Text = string.Empty;
                return;
            }
            else
            {
                // Truy xuất thông tin phương tiện
                var vehicleInfo = _dbContext.TblMdVehicle
                    .AsNoTracking()
                    .Where(v => v.Code == txtLicensePlate.Text)
                    .Select(v => new { v.OicPbatch, v.OicPtrip })
                    .FirstOrDefault();
                txtVehicleName.Text = vehicleInfo != null ? $"{vehicleInfo.OicPbatch}{vehicleInfo.OicPtrip}" : string.Empty;
            }
        }
    }
}
