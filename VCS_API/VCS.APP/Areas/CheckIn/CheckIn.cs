using DMS.CORE;
using DMS.CORE.Entities.MD;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using VCS.APP.Utilities;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using VCS.APP.Services;
using DMS.BUSINESS.Services.SMO;
using DMS.BUSINESS.Dtos.SMO;
using System.Data;
using DMS.CORE.Entities.BU;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using DMS.BUSINESS.Dtos.Auth;
using Microsoft.Extensions.DependencyInjection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using AutoMapper;
using System.Windows.Forms;
using System.Windows;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Services.BU;


namespace VCS.APP.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private readonly IWOrderService _orderService;
        private AppDbContext _dbContext;
        private List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        private Dictionary<string, LibVLCSharp.Shared.MediaPlayer> _mediaPlayers = new Dictionary<string, LibVLCSharp.Shared.MediaPlayer>();
        private LibVLCSharp.Shared.LibVLC? _libVLC;
        private List<DOSAPDataDto> _lstDOSAP = new List<DOSAPDataDto>();
        private List<string> lstCheckDo = new List<string>();
        private string IMGPATH;
        private string PLATEPATH;

        public CheckIn(AppDbContext dbContext, IWOrderService orderService)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _orderService = orderService;
            InitializeLibVLC();
            GetListCameras();
            InitializeControls();
            CheckStatusSystem();
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
                        && x.IsIn) // Lọc camera cổng vào
                    .ToList();

                InitializeCameraStreams();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách camera: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            }
            finally
            {
                btnDetect.Enabled = true;
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CommonService.CleanupCameraResources(_mediaPlayers, _libVLC);
            base.OnFormClosing(e);
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

        private void lblCameraTitle_Click(object sender, EventArgs e)
        {

        }

        private void infoPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckIn_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCheckNumber_Click(object sender, EventArgs e)
        {
            try
            {
                var number = txtNumber.Text.Trim();
                if (string.IsNullOrEmpty(number))
                {
                    txtStatus.Text = "Vui lòng nhập số lệnh xuất";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }
                if (lstCheckDo.Where(x => x == number).Count() == 1)
                {
                    txtStatus.Text = "Đã kiểm tra thông tin lệnh xuất! Vui lòng thử lệnh xuất khác!";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }
                var _s = new CommonService();
                var token = _s.LoginSmoApi();
                if (string.IsNullOrEmpty(token))
                {
                    txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }

                var dataDetail = _s.GetInformationNumber(number, token);
                if (!dataDetail.STATUS)
                {
                    txtStatus.Text = "Số lệnh xuất không tồn tại hoặc đã hết hạn! Vui lòng kiểm tra lại!";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }

                txtStatus.Text = "Kiểm tra lệnh xuất thành công!";
                txtStatus.ForeColor = Color.Green;

                _lstDOSAP.Add(dataDetail);
                lstCheckDo.Add(number);


                AppendPanelDetail(dataDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vui lòng liện hệ đến quản trị viên hệ thống: {ex.Message}",
                        "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DeleteDOSAPDetail(string doNumber, Button deleteButton)
        {
            try
            {
                var itemToRemove = _lstDOSAP.FirstOrDefault(x =>
                    x.DATA.LIST_DO.FirstOrDefault()?.DO_NUMBER == doNumber);
                if (itemToRemove != null)
                {
                    _lstDOSAP.Remove(itemToRemove);
                }
                lstCheckDo.Remove(doNumber);

                int deletedY = deleteButton.Location.Y;

                var gridToRemove = panel1.Controls.OfType<DataGridView>()
                    .FirstOrDefault(g => g.Location.Y == deleteButton.Location.Y + 35);
                int heightRemoved = 0;
                if (gridToRemove != null)
                {
                    heightRemoved = gridToRemove.Height + 35;
                    panel1.Controls.Remove(gridToRemove);
                    gridToRemove.Dispose();
                }
                panel1.Controls.Remove(deleteButton);
                deleteButton.Dispose();

                foreach (Control control in panel1.Controls)
                {
                    if (control.Location.Y > deletedY && (control is DataGridView || (control is Button && control.Size.Width == 30)))
                    {
                        control.Location = new Point(control.Location.X, control.Location.Y - heightRemoved);
                    }
                }

                panel1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                var deleteButton = new Button
                {
                    Size = new System.Drawing.Size(30, 30),
                    Location = new Point(797, yPosition),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(230, 230, 230),
                    ForeColor = Color.Black,
                    Cursor = Cursors.Hand,
                    Image = Properties.Resources.delete_icon,
                    ImageAlign = ContentAlignment.MiddleCenter
                };
                deleteButton.FlatAppearance.BorderSize = 0;

                if (deleteButton.Image != null)
                {
                    deleteButton.Image = new Bitmap(deleteButton.Image, new System.Drawing.Size(16, 16));
                }

                deleteButton.Click += (sender, e) =>
                {
                    var doNumber = data.DATA.LIST_DO.FirstOrDefault()?.DO_NUMBER;
                    DeleteDOSAPDetail(doNumber, deleteButton);
                };

                panel1.Controls.Add(deleteButton);

                var dataGridView1 = new DataGridView();
                dataGridView1.BackgroundColor = Color.White;
                dataGridView1.BorderStyle = BorderStyle.None;
                dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dataGridView1.Location = new Point(18, yPosition + 35);
                dataGridView1.Name = $"dataGridView_{_lstDOSAP.Count}";
                dataGridView1.TabIndex = 14;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ColumnHeadersHeight = 30;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
                dataGridView1.RowTemplate.Height = 35;
                dataGridView1.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);

                System.Data.DataTable dataTable = new System.Data.DataTable();
                dataTable.Columns.Add("Số lệnh xuất", typeof(string));
                dataTable.Columns.Add("Phương tiện", typeof(string));
                dataTable.Columns.Add("Mặt hàng", typeof(string));
                dataTable.Columns.Add("Số lượng (ĐVT)", typeof(string));

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

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                int totalHeight = dataGridView1.ColumnHeadersHeight +
                    (dataTable.Rows.Count * dataGridView1.RowTemplate.Height) + 20;
                dataGridView1.Size = new System.Drawing.Size(809, totalHeight);

                panel1.Controls.Add(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vui lòng liên hệ đến quản trị viên hệ thống: {ex.Message}",
                        "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtLicensePlate_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                txtStatus.Text = "Không có thông tin phương tiện! Vui lòng kiểm tra lại!";
                txtStatus.ForeColor = Color.Red;
                return;
            }
            var check = _dbContext.TblBuQueue.Where(x => x.CreateDate.Value.Date == DateTime.Now.Date && x.VehicleCode == txtLicensePlate.Text).Count();
            if (check > 0)
            {
                txtStatus.Text = "Phương tiện đã có trong hàng chờ! Vui lòng kiểm tra lại!";
                txtStatus.ForeColor = Color.Red;
                return;
            }

            var headerId = Guid.NewGuid().ToString();
            var name = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPtrip ?? "";
            _dbContext.TblBuHeader.Add(new TblBuHeader
            {
                Id = headerId,
                VehicleCode = txtLicensePlate.Text,
                CompanyCode = ProfileUtilities.User.OrganizeCode,
                WarehouseCode = ProfileUtilities.User.WarehouseCode
            });

            foreach (var i in _lstDOSAP)
            {
                var hId = Guid.NewGuid().ToString();
                _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                {
                    Id = hId,
                    HeaderId = headerId,
                    Do1Sap = i.DATA.LIST_DO.FirstOrDefault().DO_NUMBER,
                    VehicleCode = i.DATA.VEHICLE
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
                Path = PLATEPATH,
                FullPath = PLATEPATH,
                IsPlate = true,
                IsActive = true,
            });
            _dbContext.TblBuImage.Add(new TblBuImage
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = headerId,
                Path = IMGPATH,
                FullPath = IMGPATH,
                IsPlate = false,
                IsActive = true
            });
            _dbContext.TblBuQueue.Add(new TblBuQueue
            {
                Id = Guid.NewGuid().ToString(),
                HeaderId = headerId,
                VehicleCode = txtLicensePlate.Text,
                Name = name,
                Order = _dbContext.TblBuQueue.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date).Count() + 1,
                Count = 0,
                IsActive = true
            });
            await _dbContext.SaveChangesAsync();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                var number = txtNumber.Text.Trim();
                if (string.IsNullOrEmpty(number))
                {
                    txtStatus.Text = "Vui lòng nhập số lệnh xuất";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }
                if (lstCheckDo.Where(x => x == number).Count() == 1)
                {
                    txtStatus.Text = "Đã có thông tin! Vui lòng thử lệnh xuất khác!";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }
                var _s = new CommonService();
                var token = _s.LoginSmoApi();
                if (string.IsNullOrEmpty(token))
                {
                    txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }

                var dataDetail = _s.GetInformationNumber(number, token);
                if (!dataDetail.STATUS)
                {
                    txtStatus.Text = "Số lệnh xuất không tồn tại hoặc đã hết hạn! Vui lòng kiểm tra lại!";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }

                txtStatus.Text = "Kiểm tra lệnh xuất thành công!";
                txtStatus.ForeColor = Color.Green;

                _lstDOSAP.Add(dataDetail);
                lstCheckDo.Add(number);


                AppendPanelDetail(dataDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vui lòng liện hệ đến quản trị viên hệ thống: {ex.Message}",
                        "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCheckIn_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                txtStatus.Text = "Không có thông tin phương tiện! Vui lòng kiểm tra lại!";
                txtStatus.ForeColor = Color.Red;
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
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            string selectedHeaderId = selectedItem.Value;
            if (string.IsNullOrEmpty(selectedHeaderId))
            {
                var headerId = Guid.NewGuid().ToString();

                _dbContext.TblBuHeader.Add(new TblBuHeader
                {
                    Id = headerId,
                    VehicleCode = txtLicensePlate.Text,
                    CompanyCode = ProfileUtilities.User.OrganizeCode,
                    WarehouseCode = ProfileUtilities.User.WarehouseCode
                });

                foreach (var i in _lstDOSAP)
                {
                    var hId = Guid.NewGuid().ToString();
                    _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                    {
                        Id = hId,
                        HeaderId = headerId,
                        Do1Sap = i.DATA.LIST_DO.FirstOrDefault().DO_NUMBER,
                        VehicleCode = i.DATA.VEHICLE
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
                    Path = PLATEPATH,
                    FullPath = PLATEPATH,
                    IsPlate = true,
                    IsActive = true,
                });
                _dbContext.TblBuImage.Add(new TblBuImage
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    Path = IMGPATH,
                    FullPath = IMGPATH,
                    IsPlate = false,
                    IsActive = true
                });

                var _stt = _dbContext.tblMdSequence.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date).Count();
                _stt = _stt == 0 ? 1 : _dbContext.tblMdSequence.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date).Max(x => x.STT) + 1;

                _dbContext.tblMdSequence.Add(new TblMdSequence
                {
                    Code = Guid.NewGuid().ToString(),
                    STT = _stt,
                    WarehouseCode = ProfileUtilities.User.WarehouseCode,
                    OrgCode = ProfileUtilities.User.OrganizeCode
                });

                var orderDto = new OrderDto
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = headerId,
                    VehicleCode = txtLicensePlate.Text,
                    Name = name,
                    Count = "0",
                    IsActive = true,
                    WarehouseCode = ProfileUtilities.User.WarehouseCode,
                    CompanyCode = ProfileUtilities.User.OrganizeCode
                };

                var result = await _orderService.Add(orderDto);

                if (result == null)
                {
                    MessageBox.Show("Thêm mới thất bại!");
                    return;
                }


                MessageBox.Show("Cho vào kho cấp số thành công!");
                ReloadForm(_dbContext);
            }
            else
            {
                var itemDelete = _dbContext.TblBuQueue.FirstOrDefault(x => x.HeaderId == selectedHeaderId);
                _dbContext.TblBuQueue.Remove(itemDelete);
                _dbContext.TblBuOrders.Add(new TblBuOrder
                {
                    Id = Guid.NewGuid().ToString(),
                    HeaderId = selectedHeaderId,
                    VehicleCode = txtLicensePlate.Text,
                    Name = name,
                    Order = _dbContext.TblBuQueue.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date).Count() + 1,
                    Stt = _dbContext.TblBuQueue.Where(q => q.CreateDate.Value.Date == DateTime.Now.Date).Count() + 1,
                    Count = 0,
                    IsActive = true
                });
                _dbContext.SaveChanges();
            }
        }

        private async void CheckStatusSystem()
        {
            try
            {
                if (!await _dbContext.Database.CanConnectAsync())
                {
                    statusDB.BackColor = Color.Red;
                }
                else
                {
                    statusDB.BackColor = Color.LimeGreen;
                }
                var _s = new CommonService();
                var token = _s.LoginSmoApi();
                if (string.IsNullOrEmpty(token))
                {
                    statusSMO.BackColor = Color.Red;
                }
                else
                {
                    statusSMO.BackColor = Color.LimeGreen;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadForm(AppDbContext dbContext)
        {
            this.Controls.Clear();
            InitializeComponent();
            _dbContext = dbContext;
            InitializeLibVLC();
            GetListCameras();
            InitializeControls();
            CheckStatusSystem();
            GetListQueue();
            ResetVarible();
        }
        private void ResetVarible()
        {
            _lstDOSAP = new List<DOSAPDataDto>();
            lstCheckDo = new List<string>();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ReloadForm(_dbContext);
        }

        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuQueue.Where(x => x.CreateDate.Value.Year == DateTime.Now.Year
            && x.CreateDate.Value.Month == DateTime.Now.Month && x.CreateDate.Value.Day == DateTime.Now.Day).ToList();
            //var lstQueue = _dbContext.TblBuQueue.ToList();
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            items.Add(new ComboBoxItem(" -", ""));
            foreach (var item in lstQueue)
            {
                items.Add(new ComboBoxItem($"{item.VehicleCode} - {item.Name}", item.HeaderId));
            }
            comboBox1.DataSource = items;
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                string selectedValue = selectedItem.Value;

                if (string.IsNullOrEmpty(selectedValue)) return;

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

                txtStatus.Text = "Đã tải thông tin thành công";
                txtStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Lỗi khi tải thông tin: {ex.Message}";
                txtStatus.ForeColor = Color.Red;
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                string selectedHeaderId = selectedItem.Value;

                if (string.IsNullOrEmpty(selectedHeaderId))
                {
                    txtStatus.Text = "Vui lòng chọn một phương tiện để cập nhật";
                    txtStatus.ForeColor = Color.Red;
                    return;
                }
                _dbContext.ChangeTracker.Clear();

                var header = await _dbContext.TblBuHeader.FindAsync(selectedHeaderId);
                if (header != null && header.VehicleCode != txtLicensePlate.Text)
                {
                    header.VehicleCode = txtLicensePlate.Text;
                    _dbContext.TblBuHeader.Update(header);
                }

                var oldDOs = await _dbContext.TblBuDetailDO
                    .Where(x => x.HeaderId == selectedHeaderId)
                    .ToListAsync();

                foreach (var oldDO in oldDOs)
                {
                    var oldMaterials = await _dbContext.TblBuDetailMaterial
                        .Where(x => x.HeaderId == oldDO.Id)
                        .ToListAsync();
                    _dbContext.TblBuDetailMaterial.RemoveRange(oldMaterials);
                    _dbContext.TblBuDetailDO.Remove(oldDO);
                }

                foreach (var doSap in _lstDOSAP)
                {
                    var hId = Guid.NewGuid().ToString();
                    _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                    {
                        Id = hId,
                        HeaderId = selectedHeaderId,
                        Do1Sap = doSap.DATA.LIST_DO.FirstOrDefault().DO_NUMBER,
                        VehicleCode = doSap.DATA.VEHICLE,
                    });

                    foreach (var material in doSap.DATA.LIST_DO.FirstOrDefault().LIST_MATERIAL)
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

                var queue = await _dbContext.TblBuQueue
                    .FirstOrDefaultAsync(x => x.HeaderId == selectedHeaderId);
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

                await _dbContext.SaveChangesAsync();
                txtStatus.Text = "Cập nhật thông tin thành công";
                txtStatus.ForeColor = Color.Green;

                string currentSelectedText = selectedItem.Text;

                GetListQueue();
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    var item = (ComboBoxItem)comboBox1.Items[i];
                    if (item.Text == currentSelectedText)
                    {
                        comboBox1.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = $"Lỗi khi cập nhật thông tin: {ex.Message}";
                txtStatus.ForeColor = Color.Red;
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void videoView_Click(object sender, EventArgs e)
        {

        }

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    var number = txtNumber.Text.Trim();
                    if (string.IsNullOrEmpty(number))
                    {
                        txtStatus.Text = "Vui lòng nhập số lệnh xuất";
                        txtStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (lstCheckDo.Where(x => x == number).Count() == 1)
                    {
                        txtStatus.Text = "Đã có thông tin! Vui lòng thử lệnh xuất khác!";
                        txtStatus.ForeColor = Color.Red;
                        return;
                    }
                    var _s = new CommonService();
                    var token = _s.LoginSmoApi();
                    if (string.IsNullOrEmpty(token))
                    {
                        txtStatus.Text = "Không thể kết nối đến hệ thống SMO";
                        txtStatus.ForeColor = Color.Red;
                        return;
                    }

                    var dataDetail = _s.GetInformationNumber(number, token);
                    if (!dataDetail.STATUS)
                    {
                        txtStatus.Text = "Số lệnh xuất không tồn tại hoặc đã hết hạn! Vui lòng kiểm tra lại!";
                        txtStatus.ForeColor = Color.Red;
                        return;
                    }

                    txtStatus.Text = "Kiểm tra lệnh xuất thành công!";
                    txtStatus.ForeColor = Color.Green;

                    _lstDOSAP.Add(dataDetail);
                    lstCheckDo.Add(number);


                    AppendPanelDetail(dataDetail);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Vui lòng liện hệ đến quản trị viên hệ thống: {ex.Message}",
                            "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
