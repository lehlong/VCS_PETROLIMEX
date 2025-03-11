using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DocumentFormat.OpenXml;
using ICSharpCode.SharpZipLib.Zip;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record.Chart;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Areas.PrintStt;
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.Areas.ViewAllCamera;
using VCS.Services;

namespace VCS.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private MediaPlayer _mediaPlayer;
        private AppDbContextForm _dbContext;
        private List<DOSAPDataDto> _lstDOSAP = new List<DOSAPDataDto>();
        private List<string> lstCheckDo = new List<string>();
        private List<string> lstPathImageCapture = new List<string>();
        private string IMGPATH;
        private string PLATEPATH;
        private TblMdCamera CameraDetect { get; set; } = new TblMdCamera();

        private System.Windows.Forms.Timer resetTimer;
        public CheckIn(AppDbContextForm dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            resetTimer = new System.Windows.Forms.Timer();
            resetTimer.Interval = 500;
            resetTimer.Tick += ResetTimer_Tick;
            txtNumberDO.Focus();
        }

        private void CheckIn_Load(object sender, EventArgs e)
        {
            StreamCamera();
            GetListQueue();
        }

        #region Khởi tạo và stream camera
        private void StreamCamera()
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
            items.Add(new ComboBoxItem(" -", ""));
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
            panelDODetail.Controls.OfType<DataGridView>().ToList()
                .ForEach(x => { x.Dispose(); panelDODetail.Controls.Remove(x); });
            panelDODetail.Controls.OfType<Button>()
                .Where(x => x.Size.Width == 30)
                .ToList()
                .ForEach(x => { x.Dispose(); panelDODetail.Controls.Remove(x); });

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

            var dataDetail = CommonService.GetDetailDO(txtNumberDO.Text.Trim());
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
        public static string GetText(string type)
        {
            if (type == "DCCH")
            {
                return "Di chuyển ra cửa hàng";
            }
            else if (type == "DCNB")
            {
                return "Di chuyển nội bộ ngành";
            }
            else if (type == "XBTX")
            {
                return "Xuất bán tái xuất";
            }
            else if (type == "XBND")
            {
                return "Xuất bán nội địa";
            }
            else if (type == "XTHG")
            {
                return "Xuất trả hàng gửi";
            }
            else if (type == "MHGL")
            {
                return "Mua hàng gửi lại";
            }
            else if (type == "HHK")
            {
                return "Hàng hóa khác";
            }
            else if (type == "KHLH")
            {
                return "Kế hoạch lấy hàng";
            }
            else if (type == "SUM")
            {
                return "Đơn hàng tổng";
            }
            else if (type == "XDTH")
            {
                return "Xuất đổi trả hàng";
            }
            else if (type == "XHND")
            {
                return "Xuất hàng nội dụng";
            }
            return string.Empty;
        }

        private void AppendPanelDetail(DOSAPDataDto data)
        {
            try
            {
                if (data?.DATA?.LIST_DO?.Any() != true || data.DATA.LIST_DO.FirstOrDefault() == null)
                    return;

                var firstDo = data.DATA.LIST_DO.FirstOrDefault();
                string doNumber = firstDo.DO_NUMBER;

                if (panelDODetail.Controls.OfType<Panel>().Any(p => p.Name == $"panel_{doNumber}"))
                {
                    MessageBox.Show("Lệnh xuất này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                    Text = $"{GetText(firstDo.MODUL_TYPE)} - {firstDo.NGUON_HANG ?? "Không xác định"}",
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
            var name = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPtrip ?? "";

            var _stt = _dbContext.tblMdSequence.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date
            && q.WarehouseCode == ProfileUtilities.User.WarehouseCode
            && q.OrgCode == ProfileUtilities.User.OrganizeCode).Count();

            _stt = _stt == 0 ? 1 :
                _dbContext.tblMdSequence.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date
            && q.WarehouseCode == ProfileUtilities.User.WarehouseCode
            && q.OrgCode == ProfileUtilities.User.OrganizeCode).Max(x => x.STT) + 1;

            if (string.IsNullOrEmpty(selectedHeaderId))
            {
                var headerId = Guid.NewGuid().ToString();

                _dbContext.TblBuHeader.Add(new TblBuHeader
                {
                    Id = headerId,
                    VehicleCode = txtLicensePlate.Text,
                    VehicleName = name,
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
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    Path = string.IsNullOrEmpty(IMGPATH) ? "" : IMGPATH.Replace(Global.PathSaveFile, ""),
                    FullPath = string.IsNullOrEmpty(IMGPATH) ? "" : IMGPATH,
                    InOut = "in",
                    IsPlate = true,
                    IsActive = true,
                });
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    Path = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH.Replace(Global.PathSaveFile, ""),
                    FullPath = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH,
                    InOut = "in",
                    IsPlate = false,
                    IsActive = true
                });
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
            _dbContext.SaveChanges();

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

            var c = _dbContext.TblBuHeader.Where(x => x.VehicleCode == txtLicensePlate.Text && x.StatusVehicle != "04"
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).Count();
            if (c != 0)
            {
                CommonService.Alert("Phương tiện đã có trong hàng chờ hoặc trong kho!", Alert.Alert.enumType.Error);
                return;
            }

            var headerId = Guid.NewGuid().ToString();
            var name = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPtrip ?? "";
            _dbContext.TblBuHeader.Add(new TblBuHeader
            {
                Id = headerId,
                VehicleCode = txtLicensePlate.Text,
                VehicleName = name,
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
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = headerId,
                Path = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH.Replace(Global.PathSaveFile, ""),
                FullPath = string.IsNullOrEmpty(PLATEPATH) ? "" : PLATEPATH,
                InOut = "in",
                IsPlate = true,
                IsActive = true,
            }); ;
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = headerId,
                InOut = "in",
                Path = string.IsNullOrEmpty(IMGPATH) ? "" : IMGPATH.Replace(Global.PathSaveFile, ""),
                FullPath = string.IsNullOrEmpty(IMGPATH) ? "" : IMGPATH,
                IsPlate = false,
                IsActive = true
            });

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
                if (header != null && header.VehicleCode != txtLicensePlate.Text)
                {
                    header.VehicleCode = txtLicensePlate.Text;
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

                var queue = _dbContext.TblBuQueue
                    .FirstOrDefault(x => x.HeaderId == selectedHeaderId);
                if (queue != null && queue.VehicleCode != txtLicensePlate.Text)
                {
                    queue.VehicleCode = txtLicensePlate.Text;
                    var name = _dbContext.TblMdVehicle
                        .FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPbatch +
                        _dbContext.TblMdVehicle
                        .FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPtrip ?? "";
                    queue.Name = name;
                    _dbContext.TblBuQueue.Update(queue);
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
                btnDetect.Enabled = false;

                //Chụp và lưu ảnh nhận diện
                var (filePath, snapshotImage) = CommonService.TakeSnapshot(viewStream.MediaPlayer);
                IMGPATH = filePath;

                if (!string.IsNullOrEmpty(filePath))
                {
                    pictureBoxVehicle.Image = snapshotImage;

                    var (licensePlate, croppedImage, savedImagePath) = await CommonService.DetectLicensePlateAsync(filePath);
                    PLATEPATH = savedImagePath;
                    if (!string.IsNullOrEmpty(licensePlate))
                    {
                        txtLicensePlate.Text = licensePlate;
                        pictureBoxLicensePlate.Image = croppedImage;
                        if (string.IsNullOrEmpty(txtVehicleName.Text))
                        {
                            var name = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == licensePlate)?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == licensePlate)?.OicPtrip ?? "";
                            txtVehicleName.Text = name;
                        }

                    }
                }
                CommonService.Alert("Nhận diện thành công!", Alert.Alert.enumType.Success);

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
                txtLicensePlate.Text = "";
                pictureBoxLicensePlate.Image = null;
                CommonService.Alert("Lỗi không nhận diện được biển số!", Alert.Alert.enumType.Error);
                txtLicensePlate.Text = "";
            }
            finally
            {
                btnDetect.Enabled = true;
            }
            txtNumberDO.Focus();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            base.OnFormClosing(e);
        }

        private void viewCameraFullscreen_Click(object sender, EventArgs e)
        {
            var v = new ViewCamera(CameraDetect);
            v.ShowDialog();
        }
    }
}
