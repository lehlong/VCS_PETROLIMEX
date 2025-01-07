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

namespace VCS.APP.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private AppDbContext _dbContext;
        private List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        private Dictionary<string, LibVLCSharp.Shared.MediaPlayer> _mediaPlayers = new Dictionary<string, LibVLCSharp.Shared.MediaPlayer>();
        private LibVLCSharp.Shared.LibVLC? _libVLC;
        private List<DOSAPDataDto> _lstDOSAP = new List<DOSAPDataDto>();
        private string IMGPATH;
        private string PLATEPATH;
        private AppDbContext dbContext;

        public CheckIn(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
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
                // Xóa khỏi danh sách DOSAP
                _lstDOSAP.RemoveAll(x => x.DATA.LIST_DO.FirstOrDefault()?.DO_NUMBER == doNumber);
                
                // Lấy vị trí Y của button bị xóa
                int deletedY = deleteButton.Location.Y;
                
                // Xóa DataGridView và Button khỏi giao diện
                var gridToRemove = panel1.Controls.OfType<DataGridView>()
                    .FirstOrDefault(g => g.Location.Y == deleteButton.Location.Y + 35);
                int heightRemoved = 0;
                if (gridToRemove != null)
                {
                    heightRemoved = gridToRemove.Height + 35; // Chiều cao của grid + khoảng cách
                    panel1.Controls.Remove(gridToRemove);
                    gridToRemove.Dispose();
                }
                panel1.Controls.Remove(deleteButton);
                deleteButton.Dispose();

                // Di chuyển chỉ các DataGridView và Button phía dưới lên
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

                // Tạo nút xóa
                var deleteButton = new Button
                {
                    Size = new System.Drawing.Size(30, 30),
                    Location = new Point(797, yPosition),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(230, 230, 230), // Màu xám nhạt
                    ForeColor = Color.Black,
                    Cursor = Cursors.Hand,
                    Image = Properties.Resources.delete_icon, // Thêm icon từ Resources
                    ImageAlign = ContentAlignment.MiddleCenter // Căn giữa icon
                };
                deleteButton.FlatAppearance.BorderSize = 0;

                // Điều chỉnh kích thước icon nếu cần
                if (deleteButton.Image != null)
                {
                    deleteButton.Image = new Bitmap(deleteButton.Image, new System.Drawing.Size(16, 16));
                }

                // Xử lý sự kiện click nút xóa
                deleteButton.Click += (sender, e) =>
                {
                    var doNumber = data.DATA.LIST_DO.FirstOrDefault()?.DO_NUMBER;
                    DeleteDOSAPDetail(doNumber, deleteButton);
                };

                // Thêm nút xóa vào panel1
                panel1.Controls.Add(deleteButton);

                // Tạo và cấu hình DataGridView
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

                // Thêm DataGridView vào panel1
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
            //var _s = new CommonService();
            //var token = _s.LoginSmoApi();

            //if (_lstDOSAP.Count() == 0)
            //{
            //    DialogResult result = MessageBox.Show("Chưa có thông tin lệnh xuất! Vẫn cho xe vào?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    switch (result)
            //    {
            //        case DialogResult.No:
            //            break;
            //        case DialogResult.Yes:
            //            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            //            {
            //                txtStatus.Text = "Không có thông tin phương tiện! Vui lòng kiểm tra lại!";
            //                txtStatus.ForeColor = Color.Red;
            //                return;
            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //    return;
            //}
            //if (txtLicensePlate.Text != _lstDOSAP.FirstOrDefault().DATA.VEHICLE)
            //{
            //    txtStatus.Text = "Lưu ý! Phương tiện vào không trùng với phương tiện trong lệnh xuất!";
            //    txtStatus.ForeColor = Color.Red;
            //    return;
            //}

            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                txtStatus.Text = "Không có thông tin phương tiện! Vui lòng kiểm tra lại!";
                txtStatus.ForeColor = Color.Red;
                return;
            }
            var _s = new CommonService();
            var token = _s.LoginSmoApi();

            if (string.IsNullOrEmpty(token) || _lstDOSAP.Count() == 0)
            {
                var message = string.IsNullOrEmpty(token) ?
                    "Không thể kết nối đến hệ thống SMO!" :
                    "Chưa có thông tin lệnh xuất!";

                if (MessageBox.Show($"{message} Vẫn cho xe vào?", "Cảnh báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    txtStatus.Text = message;
                    txtStatus.ForeColor = Color.Red;
                    return;
                }
                return;
            }

            if (txtLicensePlate.Text != _lstDOSAP.FirstOrDefault().DATA.VEHICLE)
            {
                txtStatus.Text = "Lưu ý! Phương tiện vào không trùng với phương tiện trong lệnh xuất!";
                txtStatus.ForeColor = Color.Red;
                return;
            }
            var headerId = Guid.NewGuid().ToString();
            var name = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == txtLicensePlate.Text)?.OicPtrip ?? "";
            _dbContext.TblBuHeader.Add(new TblBuHeader
            {
                Id = headerId,
                VehicleCode = txtLicensePlate.Text,
            });

            foreach (var i in _lstDOSAP)
            {
                var hId = Guid.NewGuid().ToString();
                _dbContext.TblBuDetailDO.Add(new TblBuDetailDO
                {
                    Id = hId,
                    HeaderId = headerId,
                    Do1Sap = i.DATA.LIST_DO.FirstOrDefault().DO_NUMBER,
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



                AppendPanelDetail(dataDetail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vui lòng liện hệ đến quản trị viên hệ thống: {ex.Message}",
                        "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

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
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ReloadForm(_dbContext);
        }

        private void GetListQueue()
        {
            //var lstQueue = _dbContext.TblBuQueue.Where(x => x.CreateDate.Value.Year == DateTime.Now.Year
            //&& x.CreateDate.Value.Month == DateTime.Now.Month && x.CreateDate.Value.Day == DateTime.Now.Day).ToList();
            var lstQueue = _dbContext.TblBuQueue.ToList();
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
                            VEHICLE = header.VehicleCode,
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
