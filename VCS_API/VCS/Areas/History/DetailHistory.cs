using DMS.CORE;
using DMS.CORE.Entities.BU;
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
using VCS.Services;

namespace VCS.APP.Areas.History
{
    public partial class DetailHistory : Form
    {
        private AppDbContextForm _dbContext;
        private string _headerId;
        public DetailHistory(AppDbContextForm dbContext, string headerId)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _headerId = headerId;
        }

        private void DetailHistory_Load(object sender, EventArgs e)
        {
            var _stt = _dbContext.TblBuHeader.Where(x => x.Id == _headerId).Select(x => x.Stt).FirstOrDefault();
            var data = _dbContext.TblBuHeader.Where(x => x.Id == _headerId).ToList();
            lblWarehouse.Text = GetNameWarehouse();
            foreach (var i in data)
            {
                lblVehicle.Text = i.VehicleCode.ToString();
                lblDriver.Text = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == i.VehicleCode.ToString())?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == i.VehicleCode.ToString())?.OicPtrip ?? "";
                lblTimeIn.Text = i.CreateDate.ToString();
                lblTimeout.Text = i.TimeCheckout.ToString();
                lblNotein.Text = i.NoteIn;
                lblNoteout.Text = i.NoteOut;
                lblStt.Text = _stt != null ? _stt.ToString("D2") : "";
            }
            var imgINList = _dbContext.TblBuImage
                                    .Where(x => x.HeaderId == _headerId && x.InOut == "in")
                                    .Select(x => x.FullPath)
                                    .ToList();


            var pictureBoxes = new List<PictureBox> { ptbIn1, ptbIn2, pcbIn3, pcbIn4 };
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                if (imgINList.Count > i)
                {
                    if (File.Exists(imgINList[i]))
                        pictureBoxes[i].Image = Image.FromFile(imgINList[i]);
                    else
                        pictureBoxes[i].Image = null;
                }
                else
                {
                    pictureBoxes[i].Image = null;
                }
            }
                var imgOUTList = _dbContext.TblBuImage
                                    .Where(x => x.HeaderId == _headerId && x.InOut == "out")
                                    .Select(x => x.FullPath)
                                    .ToList();
            var pictureBoxesOut = new List<PictureBox> { ptcOut1, ptbOut2, ptcOut3, ptcOut4 };
            for (int i = 0; i < pictureBoxesOut.Count; i++)
            {
                if (imgOUTList.Count > i)
                {
                    if (File.Exists(imgOUTList[i]))
                        pictureBoxesOut[i].Image = Image.FromFile(imgOUTList[i]);
                    else
                        pictureBoxesOut[i].Image = null;
                }
                else
                {
                    pictureBoxesOut[i].Image = null;
                }
            }
            var lstDO = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == _headerId).ToList();
            var lstDOOUT = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == _headerId).ToList();



            var detail = GetCheckInDetail(_headerId);
            if (detail != null)
            {
                panelCheckIn.Controls.Clear();
                foreach (var doSap in detail.ListDOSAP)
                {
                    AppendPanelDetailCheckIn(doSap);
                }
            }
            var headerTgbx = _dbContext.TblBuHeaderTgbx.Where(x => x.HeaderId == _headerId).ToList();
            var detailTgbx = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == _headerId).ToList();
            var lstDo = detailTgbx.Select(x => x.SoLenh).Distinct().ToList();

            if (lstDo.Count > 0)
            {
                foreach (var doSap in lstDo)
                {
                    var lstData = detailTgbx.Where(x => x.SoLenh == doSap).ToList();
                    AppendPanelDetailChechkOut(lstData, headerTgbx.FirstOrDefault()?.MaPhuongTien);
                }
            }
        }
      
        private string? GetNameWarehouse()
        {
            try
            {
                return _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name;
            }
            catch (Exception ex)
            {
                return null;
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
                    Size = new Size(734, 10),
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
                    Width = 650,
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
                dataGridView.Size = new Size(734, totalGridViewHeight);

                containerPanel.Size = new Size(746, totalGridViewHeight + innerY + 6);
                containerPanel.Controls.Add(dataGridView);

                panelCheckIn.Controls.Add(containerPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendPanelDetailChechkOut(List<TblBuDetailTgbx> data, string vehicleCode)
        {
            try
            {
                var text = "Không xử lý hoặc chưa có ticket!";
                var status = false;
                //if (isHasInvoice)
                //{
                //    var res = CommonService.CheckInvoice(data.FirstOrDefault().SoLenh);
                //    text = res.STATUS ? $"ĐÃ XUẤT HOÁ ĐƠN" : $"CHƯA XUẤT HOÁ ĐƠN";
                //    status = res.STATUS;
                //}


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
                    var materials = _dbContext.TblMdGoods.Find("000000000000" + item.MaHangHoa);
                    string materialName = materials?.Name ?? "Unknown";
                    dataTable.Rows.Add(data.FirstOrDefault().SoLenh, vehicleCode, materialName, $"{item.TongDuXuat?.ToString("#,#")} ({item.DonViTinh})");
                }

                dataGridView.DataSource = dataTable;

                int totalGridViewHeight = dataGridView.ColumnHeadersHeight + (dataTable.Rows.Count * dataGridView.RowTemplate.Height) + 6;
                dataGridView.Size = new Size(734, totalGridViewHeight);

                containerPanel.Size = new Size(746, totalGridViewHeight + innerY + 6);
                containerPanel.Controls.Add(dataGridView);

                panelCheckOut.Controls.Add(containerPanel);
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

        //private void ptbIn1_Click(object sender, EventArgs e)
        //{
        //    Form fullscreenForm = new Form();
        //    fullscreenForm.WindowState = FormWindowState.Maximized;
        //    fullscreenForm.FormBorderStyle = FormBorderStyle.FixedSingle;
        //    int newWidth = (int)(this.ClientSize.Width * 0.8);
        //    int newHeight = (int)(this.ClientSize.Height * 0.8);

        //    PictureBox fullscreenPictureBox = new PictureBox();
        //    fullscreenPictureBox.Image = ptbIn1.Image;
        //    fullscreenPictureBox.Size = new Size(newWidth, newHeight);
        //    fullscreenPictureBox.Dock = DockStyle.Fill;
        //    fullscreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        //    fullscreenForm.Controls.Add(fullscreenPictureBox);
        //    fullscreenForm.ShowDialog();
        //}

        private void pictureBox_Click(object sender, EventArgs e)
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
            private void btnIn_Click(object sender, EventArgs e)
        {
            var imgINList = _dbContext.TblBuImage
                                  .Where(x => x.HeaderId == _headerId && x.InOut == "in")
                                  .Select(x => x.FullPath)
                                  .ToList();
            VAImage view = new VAImage(imgINList);
            view.ShowDialog();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            var imgOUTList = _dbContext.TblBuImage
                               .Where(x => x.HeaderId == _headerId && x.InOut == "out")
                               .Select(x => x.FullPath)
                               .ToList();
            VAImage view = new VAImage(imgOUTList);
            view.ShowDialog();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
