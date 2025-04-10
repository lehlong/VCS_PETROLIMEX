using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VCS.APP.Areas.PrintStt;
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.Areas.ViewAllCamera;
using VCS.DbContext.Common;
using VCS.DbContext.Entities.BU;
using VCS.DbContext.Entities.MD;
using VCS.Services;

namespace VCS.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private MediaPlayer _mediaPlayer;
        private AppDbContextForm _dbContext;
        private List<DOSAPDataDto> _lstDOSAP;
        private List<string> lstCheckDo;
        private List<string> lstPathImageCapture;
        private string IMGPATH;
        private string PLATEPATH;
        private TblMdCamera CameraDetect { get; set; }
        private System.Windows.Forms.Timer resetTimer;
        private bool isTriggerDetect { get; set; } = false;
        public CheckIn(AppDbContextForm dbContext, bool isTriggerDetect)
        {
            _dbContext = dbContext;
            _lstDOSAP = new List<DOSAPDataDto>();
            lstCheckDo = new List<string>();
            lstPathImageCapture = new List<string>();
            this.isTriggerDetect = isTriggerDetect;

            InitializeComponent();
            resetTimer = new System.Windows.Forms.Timer();
            resetTimer.Interval = 500;
            resetTimer.Tick += ResetTimer_Tick;
            txtNumberDO.Focus();
            txtLicensePlate.Enabled = CommonService.HasPermission("R415") ? true : false;
           
        }

        private void CheckIn_Load(object sender, EventArgs e)
        {
            StreamCamera();
            GetListQueue();
        }

        #region Khởi tạo và stream camera
        private async void StreamCamera()
        {
            try
            {
                var camera = Global.lstCamera.FirstOrDefault(x => x.IsIn && x.IsRecognition);
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
        #endregion

        #region Reset Form
        private void btnResetForm_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _lstDOSAP = new List<DOSAPDataDto>();
            lstCheckDo = new List<string>();
            lstPathImageCapture = new List<string>();
            IMGPATH = "";
            PLATEPATH = "";

            this.Controls.Clear();
            InitializeComponent();
            txtLicensePlate.Enabled = CommonService.HasPermission("R415") ? true : false;
            StreamCamera();
            GetListQueue();
        }
        #endregion

        #region Phương tiện trong hàng chờ

        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuHeader.Where(x => x.StatusVehicle == "01" && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
            && x.CompanyCode == ProfileUtilities.User.OrganizeCode);

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

        private void selectVehicle_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
            string selectedValue = selectedItem.Value;

            if (string.IsNullOrEmpty(selectedValue))
            {
                btnUpdateQueue.Visible = false;
                btnDeleteQueue.Visible = false;
                return;
            }
            else
            {
                btnDeleteQueue.Visible = true;
                btnUpdateQueue.Visible = true;
            }
            var detail = GetCheckInDetail(selectedValue);
            if (detail == null) return;

            txtLicensePlate.Text = detail.LicensePlate;
            txtVehicleName.Text = detail.VehicleName;

            if (!string.IsNullOrEmpty(detail.VehicleImagePath))
            {
                if (File.Exists(detail.VehicleImagePath))
                {
                    using (var stream = new FileStream(detail.VehicleImagePath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBoxVehicle.Image?.Dispose();
                        pictureBoxVehicle.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    pictureBoxVehicle.Image = null;
                }
            }
            else
            {
                pictureBoxVehicle.Image = null;
            }
            if (!string.IsNullOrEmpty(detail.PlateImagePath))
            {
                if (File.Exists(detail.PlateImagePath))
                {
                    using (var stream = new FileStream(detail.PlateImagePath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBoxLicensePlate.Image?.Dispose();
                        pictureBoxLicensePlate.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    pictureBoxLicensePlate.Image = null;
                }
            }
            else
            {
                pictureBoxLicensePlate.Image = null;
            }

            _lstDOSAP.Clear();

            panelDODetail.Controls.Clear();

            _lstDOSAP.AddRange(detail.ListDOSAP);
            foreach (var doSap in detail.ListDOSAP)
            {
                AppendPanelDetail(doSap);
            }
        }

        private CheckInDetailModel GetCheckInDetail(string headerId)
        {
            try
            {
                lstCheckDo = new List<string>();

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
                    lstCheckDo.Add(doDetail.Do1Sap);

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
        #endregion

        #region Xử lý ô textbox lệnh xuất
        private void txtNumberDO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetDetailDO();
            }
        }
        private void txtNumberDO_TextChanged(object sender, EventArgs e)
        {
            if (txtNumberDO.Text.Length >= 10)
            {
                this.ActiveControl = null;
                txtNumberDO.Text = txtNumberDO.Text.Substring(0, 10);

                GetDetailDO();

                resetTimer.Start();
            }

        }

        private void ResetTimer_Tick(object sender, EventArgs e)
        {
            resetTimer.Stop();
            txtNumberDO.Focus();
        }
        private void btnCheckDetailDO_Click(object sender, EventArgs e)
        {
            txtNumberDO.Text = txtNumberDO.Text.Replace(" ", "");
            GetDetailDO();
        }
        private void GetDetailDO()
        {
            txtNumberDO.Text = txtNumberDO.Text.Replace(" ", "");
            if (txtNumberDO.Text.Trim().Length != 10)
            {
                CommonService.Alert("Số lệnh xuất không đúng định dạng!", Alert.Alert.enumType.Error);
                return;
            }
            if (lstCheckDo.Any(x => x == txtNumberDO.Text.Trim()))
            {
                CommonService.Alert("Đã có thông tin! Vui lòng thử lệnh xuất khác!", Alert.Alert.enumType.Error);
                return;
            }
            var dataDetail = CommonService.GetDetailDO(txtNumberDO.Text.Trim(), "0");
            if (!dataDetail.STATUS)
            {
                CommonService.Alert("Số lệnh xuất không tồn tại hoặc đã hết hạn!", Alert.Alert.enumType.Error);
                return;
            }

            _lstDOSAP.Add(dataDetail);
            lstCheckDo.Add(txtNumberDO.Text.Trim());
            txtVehicleName.Text = dataDetail.DATA.LIST_DO.FirstOrDefault()?.TAI_XE;
            AppendPanelDetail(dataDetail);

            CommonService.Alert("Kiểm tra lệnh xuất thành công!", Alert.Alert.enumType.Success);
            txtNumberDO.Text = "";

        }


        private void AppendPanelDetail(DOSAPDataDto data)
        {
            try
            {
                if (data?.DATA?.LIST_DO?.Any() != true || data.DATA.LIST_DO.FirstOrDefault() == null)
                    return;

                var firstDo = data.DATA.LIST_DO.FirstOrDefault();
                string doNumber = firstDo.DO_NUMBER;

                int yPosition = panelDODetail.Controls.OfType<Panel>().Any()
                    ? panelDODetail.Controls.OfType<Panel>().Max(p => p.Bottom) + 6
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

                var deleteButton = new Button
                {
                    Size = new Size(30, 30),
                    Location = new Point(760, 6),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Cursor = Cursors.Hand,
                    Image = Properties.Resources.delete != null ? new Bitmap(Properties.Resources.delete, new Size(16, 16)) : null,
                    ImageAlign = ContentAlignment.MiddleCenter,
                    Tag = doNumber
                };
                deleteButton.FlatAppearance.BorderSize = 0;
                deleteButton.Click += (sender, e) =>
                {
                    string doNum = deleteButton.Tag.ToString();
                    _lstDOSAP.RemoveAll(x => x.DATA.LIST_DO.Any(d => d.DO_NUMBER == doNum));
                    lstCheckDo.Remove(doNum);
                    panelDODetail.Controls.Remove(containerPanel);

                    int newYPosition = 6;
                    foreach (var panel in panelDODetail.Controls.OfType<Panel>().OrderBy(p => p.Top))
                    {
                        panel.Location = new Point(panel.Left, newYPosition);
                        newYPosition = panel.Bottom + 6;
                    }
                    panelDODetail.PerformLayout();
                };
                containerPanel.Controls.Add(deleteButton);

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

                panelDODetail.Controls.Add(containerPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Cho vào kho cấp số
        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                CommonService.Alert("Không có thông tin phương tiện!", Alert.Alert.enumType.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtVehicleName.Text))
            {
                CommonService.Alert("Không có thông tin tài xế!", Alert.Alert.enumType.Error);
                return;
            }

            var c = _dbContext.TblBuHeader.Where(x => x.VehicleCode == txtLicensePlate.Text && x.StatusVehicle != "04"
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).Count();

            ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
            string selectedHeaderId = selectedItem.Value;

            if (c != 0 && string.IsNullOrEmpty(selectedHeaderId))
            {
                CommonService.Alert("Phương tiện đã có trong hàng chờ hoặc trong kho!", Alert.Alert.enumType.Error);
                return;
            }

            if (_lstDOSAP.Count() == 0)
            {
                var result = MessageBox.Show("Không có thông tin lệnh xuất! Bạn có chắc chắn muốn cho xe vào!",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            if (_lstDOSAP.Count() != 0)
            {
                if (txtLicensePlate.Text != _lstDOSAP.FirstOrDefault().DATA.VEHICLE)
                {
                    var result = MessageBox.Show("Thông tin phương tiện không trùng khớp với lệnh xuất! Bạn có chắc chắn muốn cho xe vào!",
                                                 "Xác nhận",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
            }


            var _stt = 0;
            var stt = _dbContext.tblMdSequence.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date
            && q.WarehouseCode == ProfileUtilities.User.WarehouseCode
            && q.OrgCode == ProfileUtilities.User.OrganizeCode).ToList();
            _stt = stt.Count() == 0 ? 1 : stt.Max(x => x.STT) + 1;

            var sms = _dbContext.TblAdSmsConfig.Find("SMS");
            var w = _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode);

            if (string.IsNullOrEmpty(selectedHeaderId))
            {
                var headerId = Guid.NewGuid().ToString();

                _dbContext.TblBuHeader.Add(new TblBuHeader
                {
                    Id = headerId,
                    VehicleCode = txtLicensePlate.Text,
                    VehicleName = txtVehicleName.Text,
                    CompanyCode = ProfileUtilities.User.OrganizeCode,
                    WarehouseCode = ProfileUtilities.User.WarehouseCode,
                    NoteIn = txtNoteIn.Text,
                    Stt = _stt,
                    StatusProcess = "00",
                    StatusVehicle = "02",
                });
                foreach (var i in _lstDOSAP)
                {
                    var hId = Guid.NewGuid().ToString();
                    var _do = i.DATA.LIST_DO.FirstOrDefault();
                    _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                    {
                        Id = hId,
                        HeaderId = headerId,
                        Do1Sap = _do.DO_NUMBER,
                        VehicleCode = i.DATA.VEHICLE,
                        TankGroup = _do?.TANK_GROUP,
                        ModulType = _do?.MODUL_TYPE,
                        CustomerCode = _do?.CUSTOMER_CODE,
                        CustomerName = _do?.CUSTOMER_NAME,
                        Phone = _do?.PHONE,
                        Email = _do?.EMAIL,
                        TaiXe = _do?.TAI_XE,
                        NguonHang = _do?.NGUON_HANG,
                    });

                    foreach (var l in i.DATA.LIST_DO.FirstOrDefault().LIST_MATERIAL)
                    {
                        _dbContext.TblBuDetailMaterial.Add(new TblBuDetailMaterial
                        {
                            Id = Guid.NewGuid().ToString(),
                            HeaderId = hId,
                            MaterialCode = l.MATERIAL,
                            Quantity = l.QUANTITY,
                            UnitCode = l.UNIT,
                        });

                    }
                }
                if (!string.IsNullOrEmpty(IMGPATH))
                {
                    _dbContext.TblBuImage.Add(new TblBuImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = headerId,
                        Path = IMGPATH.Replace(Global.PathSaveFile, ""),
                        FullPath = IMGPATH,
                        InOut = "in",
                        IsPlate = false,
                        IsActive = true,
                    });
                }
                if (!string.IsNullOrEmpty(PLATEPATH))
                {
                    _dbContext.TblBuImage.Add(new TblBuImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = headerId,
                        Path = PLATEPATH.Replace(Global.PathSaveFile, ""),
                        FullPath = PLATEPATH,
                        InOut = "in",
                        IsPlate = true,
                        IsActive = true
                    });
                }


                foreach (var i in lstPathImageCapture)
                {
                    _dbContext.TblBuImage.Add(new TblBuImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = headerId,
                        InOut = "in",
                        Path = string.IsNullOrEmpty(i) ? "" : i.Replace(Global.PathSaveFile, ""),
                        FullPath = string.IsNullOrEmpty(i) ? "" : i,
                        IsPlate = false,
                        IsActive = true
                    });
                }
                _dbContext.tblMdSequence.Add(new TblMdSequence
                {
                    Code = Guid.NewGuid().ToString(),
                    STT = _stt,
                    WarehouseCode = ProfileUtilities.User.WarehouseCode,
                    OrgCode = ProfileUtilities.User.OrganizeCode
                });
            }
            else
            {
                var h = _dbContext.TblBuHeader.Find(selectedHeaderId);
                h.StatusVehicle = "02";
                h.Stt = _stt;
                h.NoteIn = txtNoteIn.Text;
                h.CreateDate = DateTime.Now;
                _dbContext.TblBuHeader.Update(h);
            }

            var vehicle = _dbContext.TblMdVehicle.Find(txtLicensePlate.Text);

            if (vehicle != null)
            {
                vehicle.OicPbatch = txtVehicleName.Text;
                _dbContext.TblMdVehicle.Update(vehicle);
            }
            else
            {
                _dbContext.TblMdVehicle.Add(new TblMdVehicle
                {
                    Code = txtLicensePlate.Text,
                    OicPbatch = txtVehicleName.Text,
                    IsActive = true,
                    CreateDate = DateTime.Now,
                    CreateBy = ProfileUtilities.User.UserName,
                });
            }

            if (w.Is_sms_in == true)
            {
                foreach (var i in _lstDOSAP)
                {
                    _dbContext.TblBuSmsQueue.Add(new TblBuSmsQueue
                    {
                        Id = Guid.NewGuid().ToString(),
                        Phone = i.DATA.LIST_DO.FirstOrDefault()?.PHONE.Replace(" ", "") ?? "",
                        SmsContent = sms.SmsIn.Replace("[PHUONG_TIEN]", txtLicensePlate.Text).Replace("[KHACH_HANG]", i.DATA.LIST_DO.FirstOrDefault()?.CUSTOMER_NAME).Replace("[LENH_XUAT]", i.DATA.LIST_DO.FirstOrDefault().DO_NUMBER).Replace("[THOI_GIAN]", DateTime.Now.ToString("dd/MM/yyyy hh:mm")),
                        IsSend = false,
                        IsActive = true,
                        Count = 0,
                    });
                }
            }

            _dbContext.SaveChanges();

            lstPathImageCapture.Add(IMGPATH);
            lstPathImageCapture.Add(PLATEPATH);
            CommonService.UploadImagesServer(lstPathImageCapture.Where(s => !string.IsNullOrWhiteSpace(s) && s != "undefined").ToList());

            if (lstCheckDo.Count() > 0)
            {
                var model = new PostStatusVehicleToSMO
                {
                    VEHICLE = txtLicensePlate.Text,
                    TYPE = "IN",
                    LIST_DO = string.Join(",", lstCheckDo),
                    DATE_INFO = DateTime.Now,
                };
                CommonService.PostStatusVehicleToSMO(model);
            }

            var ticketInfo = new TicketInfo
            {
                WarehouseName = _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name,
                Vehicle = txtLicensePlate.Text,
                STT = _stt.ToString("00"),
            };
            STT sttForm = new STT(ticketInfo, lstCheckDo);
            sttForm.ShowDialog();
            CommonService.Alert("Cho xe vào thành công!", Alert.Alert.enumType.Success);
            ResetForm();
        }
        #endregion

        #region Cho vào hàng chờ
        private void btnQueue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                CommonService.Alert("Không có thông tin phương tiện!", Alert.Alert.enumType.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtVehicleName.Text))
            {
                CommonService.Alert("Không có thông tin tài xế!", Alert.Alert.enumType.Error);
                return;
            }

            var c = _dbContext.TblBuHeader.Where(x => x.VehicleCode == txtLicensePlate.Text && x.StatusVehicle != "04"
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).Count();
            if (c != 0)
            {
                CommonService.Alert("Phương tiện đã có trong hàng chờ hoặc trong kho!", Alert.Alert.enumType.Error);
                return;
            }

            var headerId = Guid.NewGuid().ToString();
            _dbContext.TblBuHeader.Add(new TblBuHeader
            {
                Id = headerId,
                VehicleCode = txtLicensePlate.Text,
                VehicleName = txtVehicleName.Text,
                CompanyCode = ProfileUtilities.User.OrganizeCode,
                WarehouseCode = ProfileUtilities.User.WarehouseCode,
                NoteIn = txtNoteIn.Text,
                StatusVehicle = "01",
                StatusProcess = "00",
            });

            foreach (var i in _lstDOSAP)
            {
                var hId = Guid.NewGuid().ToString();
                var _do = i.DATA.LIST_DO.FirstOrDefault();
                _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                {
                    Id = hId,
                    HeaderId = headerId,
                    Do1Sap = _do.DO_NUMBER,
                    VehicleCode = i.DATA.VEHICLE,
                    TankGroup = _do?.TANK_GROUP,
                    ModulType = _do?.MODUL_TYPE,
                    CustomerCode = _do?.CUSTOMER_CODE,
                    CustomerName = _do?.CUSTOMER_NAME,
                    Phone = _do?.PHONE,
                    Email = _do?.EMAIL,
                    TaiXe = _do?.TAI_XE,
                    NguonHang = _do?.NGUON_HANG,
                });
                foreach (var l in _do.LIST_MATERIAL)
                {
                    _dbContext.TblBuDetailMaterial.Add(new TblBuDetailMaterial
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = hId,
                        MaterialCode = l.MATERIAL,
                        Quantity = l.QUANTITY,
                        UnitCode = l.UNIT,
                    });

                }
            }
            if (!string.IsNullOrEmpty(IMGPATH))
            {
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    Path = IMGPATH.Replace(Global.PathSaveFile, ""),
                    FullPath = IMGPATH,
                    InOut = "in",
                    IsPlate = false,
                    IsActive = true,
                });
            }
            if (!string.IsNullOrEmpty(PLATEPATH))
            {
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    Path = PLATEPATH.Replace(Global.PathSaveFile, ""),
                    FullPath = PLATEPATH,
                    InOut = "in",
                    IsPlate = true,
                    IsActive = true
                });
            }

            foreach (var i in lstPathImageCapture)
            {
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    InOut = "in",
                    Path = string.IsNullOrEmpty(i) ? "" : i.Replace(Global.PathSaveFile, ""),
                    FullPath = string.IsNullOrEmpty(i) ? "" : i,
                    IsPlate = false,
                    IsActive = true
                });
            }
            _dbContext.SaveChanges();

            lstPathImageCapture.Add(IMGPATH);
            lstPathImageCapture.Add(PLATEPATH);
            CommonService.UploadImagesServer(lstPathImageCapture.Where(s => !string.IsNullOrWhiteSpace(s) && s != "undefined").ToList());

            CommonService.Alert("Cho phương tiện vào hàng chờ thành công!", Alert.Alert.enumType.Success);
            ResetForm();
        }
        #endregion

        #region Cập nhật phương tiện trong hàng chờ
        private void btnUpdateQueue_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)selectVehicle.SelectedItem;
                string selectedHeaderId = selectedItem.Value;

                if (string.IsNullOrEmpty(selectedHeaderId))
                {
                    CommonService.Alert("Vui lòng chọn một phương tiện!", Alert.Alert.enumType.Error);
                    return;
                }
                if (txtLicensePlate.Text.Length > 8)
                {
                    CommonService.Alert("Biển số sai định dạng!", Alert.Alert.enumType.Error);
                    return;
                }
                _dbContext.ChangeTracker.Clear();

                var header = _dbContext.TblBuHeader.Find(selectedHeaderId);
                if (header != null)
                {
                    header.VehicleCode = txtLicensePlate.Text;
                    header.VehicleName = txtVehicleName.Text;
                    _dbContext.TblBuHeader.Update(header);
                }

                var oldDOs = _dbContext.TblBuDetailDO
                    .Where(x => x.HeaderId == selectedHeaderId)
                    .ToList();

                foreach (var oldDO in oldDOs)
                {
                    var oldMaterials = _dbContext.TblBuDetailMaterial
                        .Where(x => x.HeaderId == oldDO.Id)
                        .ToList();
                    _dbContext.TblBuDetailMaterial.RemoveRange(oldMaterials);
                    _dbContext.TblBuDetailDO.Remove(oldDO);
                }

                foreach (var doSap in _lstDOSAP)
                {
                    var hId = Guid.NewGuid().ToString();
                    var _do = doSap.DATA.LIST_DO.FirstOrDefault();
                    _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                    {
                        Id = hId,
                        HeaderId = selectedHeaderId,
                        Do1Sap = _do.DO_NUMBER,
                        VehicleCode = doSap.DATA.VEHICLE,
                        TankGroup = _do?.TANK_GROUP,
                        ModulType = _do?.MODUL_TYPE,
                        CustomerCode = _do?.CUSTOMER_CODE,
                        CustomerName = _do?.CUSTOMER_NAME,
                        Phone = _do?.PHONE,
                        Email = _do?.EMAIL,
                        TaiXe = _do?.TAI_XE,
                        NguonHang = _do?.NGUON_HANG,
                    });
                    foreach (var material in _do.LIST_MATERIAL)
                    {
                        _dbContext.TblBuDetailMaterial.Add(new TblBuDetailMaterial
                        {
                            Id = Guid.NewGuid().ToString(),
                            HeaderId = hId,
                            MaterialCode = material.MATERIAL,
                            Quantity = material.QUANTITY,
                            UnitCode = material.UNIT,
                        });
                    }
                }

                _dbContext.SaveChanges();
                CommonService.Alert("Cập nhật thông tin thành công!", Alert.Alert.enumType.Success);

                string currentSelectedText = selectedItem.Text;

                GetListQueue();
                for (int i = 0; i < selectVehicle.Items.Count; i++)
                {
                    var item = (ComboBoxItem)selectVehicle.Items[i];
                    if (item.Text == currentSelectedText)
                    {
                        selectVehicle.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Xoá phương tiện khỏi hàng chờ
        private void btnDeleteQueue_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xoá phương tiện này ra khỏi hàng chờ?",
                                                 "Xác nhận",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                _dbContext.TblBuHeader.RemoveRange(_dbContext.TblBuHeader.Where(x => x.Id == ((ComboBoxItem)selectVehicle.SelectedItem).Value));
                _dbContext.TblBuDetailDO.RemoveRange(_dbContext.TblBuDetailDO.Where(x => x.HeaderId == ((ComboBoxItem)selectVehicle.SelectedItem).Value));
                _dbContext.TblBuImage.RemoveRange(_dbContext.TblBuImage.Where(x => x.HeaderId == ((ComboBoxItem)selectVehicle.SelectedItem).Value));
                _dbContext.SaveChanges();
                CommonService.Alert("Xoá phương tiện thành công!", Alert.Alert.enumType.Success);
                ResetForm();
            }
        }
        #endregion

        #region Chụp ảnh và nhận diện xe
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

                // Hiển thị kết quả
                pictureBoxLicensePlate.Image = result.ImageCrop;
                txtLicensePlate.Text = result.LicensePlateNumber;

                PLATEPATH = croppedPath;

                // Chụp từ các camera khác
                lstPathImageCapture = Global.lstCamera
                    .Where(x => x.IsIn && x.Code != CameraDetect.Code)
                    .Select(c => CommonService.SaveDetectedImage(CommonService.CaptureFrameFromRTSP(c.Rtsp)))
                    .ToList();

                CommonService.Alert("Nhận diện phương tiện thành công!", Alert.Alert.enumType.Success);
                txtNumberDO.Focus();
            }
            catch (Exception ex)
            {
                CommonService.Alert($"Lỗi: {ex.Message}", Alert.Alert.enumType.Error);
            }
        }
        #endregion

        #region Xem tất cả Camera ở cổng vào
        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                var lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsIn)
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
        #endregion

        #region Xem ảnh và camera toàn màn hình
        private void pictureBoxVehicle_Click(object sender, EventArgs e) => ViewImageFullScreen(sender, e);
        private void pictureBoxLicensePlate_Click(object sender, EventArgs e) => ViewImageFullScreen(sender, e);
        private void viewCameraFullscreen_Click(object sender, EventArgs e)
        {
            var v = new ViewCamera(CameraDetect);
            v.ShowDialog();
        }
        private void ViewImageFullScreen(object sender, EventArgs e)
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
        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Task.Run(() =>
            {
                _mediaPlayer?.Stop();
                _mediaPlayer?.Dispose();
            });
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
