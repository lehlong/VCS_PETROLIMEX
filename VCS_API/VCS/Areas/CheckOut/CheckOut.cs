using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DocumentFormat.OpenXml;
using ICSharpCode.SharpZipLib.Zip;
using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.Areas.ViewAllCamera;
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
        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
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
            var player = viewStream.MediaPlayer;
            if (player == null || !player.IsPlaying)
            {
                CommonService.Alert("Không thể nhận diện khi camera không hoạt động!", Alert.Alert.enumType.Error);
                return;
            }

            // Tạo đường dẫn và tên file trước
            string snapshotDir = Path.Combine(Global.PathSaveFile, DateTime.Now.ToString("yyyy/MM/dd"));
            string snapshotPath = Path.Combine(snapshotDir, $"{Guid.NewGuid()}.jpg");
            string cropedPath = Path.Combine(snapshotDir, $"{Guid.NewGuid()}.jpg");
            Directory.CreateDirectory(snapshotDir);

            // Chụp ảnh từ camera
            player.TakeSnapshot(0, snapshotPath, 640, 480);

            if (!File.Exists(snapshotPath))
            {
                CommonService.Alert("Không thể chụp ảnh!", Alert.Alert.enumType.Error);
                return;
            }

            // Hiển thị ảnh chụp ở UI và chuẩn bị client trước khi chạy task
            using var image = Image.FromFile(snapshotPath);
            pictureBoxVehicle.Image = new Bitmap(image);

            // Chuẩn bị HTTP client với headers trước khi thực hiện task
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Bắt đầu xử lý nhận diện ở luồng nền
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                // Chạy task nhận diện với timeout
                var detectTask = Task.Run(async () =>
                {
                    try
                    {
                        // Gửi ảnh nhận diện - sử dụng FileStream thay vì đọc toàn bộ file vào bộ nhớ
                        using var fileStream = new FileStream(snapshotPath, FileMode.Open, FileAccess.Read);
                        using var streamContent = new StreamContent(fileStream);
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                        using var form = new MultipartFormDataContent
                {
                    { streamContent, "file", Path.GetFileName(snapshotPath) }
                };

                        // Sử dụng timeout để tránh chờ quá lâu
                        var response = await client.PostAsync(Global.DetectApiUrl, form, cts.Token);
                        if (!response.IsSuccessStatusCode)
                        {
                            return (false, "Hệ thống nhận diện lỗi hoặc chưa khởi động!", null, null, null, new List<string>());
                        }

                        // Đọc và xử lý response
                        var responseString = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JObject.Parse(responseString);
                        var detection = jsonResponse["data"]?.FirstOrDefault();

                        if (detection == null ||
                            string.IsNullOrEmpty(detection["text"]?.ToString()) ||
                            string.IsNullOrEmpty(detection["image_base64"]?.ToString()))
                        {
                            return (false, "Không nhận diện được phương tiện!", null, null, null, new List<string>());
                        }

                        var licensePlate = detection["text"].ToString();
                        var base64Image = detection["image_base64"].ToString();

                        // Lưu ảnh đã cắt
                        var plateImage = CommonService.Base64ToImage(base64Image);
                        using (var tempBitmap = new Bitmap(plateImage))
                        {
                            tempBitmap.Save(cropedPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        plateImage.Dispose();

                        // Lấy thông tin tên xe từ database
                        var vehicleInfo = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == licensePlate);
                        var vehicleName = vehicleInfo?.OicPbatch + vehicleInfo?.OicPtrip ?? "";

                        // Chụp ảnh từ các camera khác song song
                        var lstCamera = Global.lstCamera.Where(x => x.IsIn == true && x.Code != CameraDetect.Code).ToList();
                        List<string> capturedPaths = new List<string>();

                        // Sử dụng parallel để chụp đồng thời từ nhiều camera
                        Parallel.ForEach(lstCamera, c =>
                        {
                            try
                            {
                                byte[] imageBytes = CommonService.CaptureFrameFromRTSP(c.Rtsp);
                                var path = CommonService.SaveDetectedImage(imageBytes);
                                lock (capturedPaths)
                                {
                                    capturedPaths.Add(path);
                                }
                            }
                            catch
                            {
                                // Bỏ qua lỗi từ camera phụ để không ảnh hưởng đến quy trình chính
                            }
                        });

                        return (true, "Nhận diện phương tiện thành công!", licensePlate, vehicleName, base64Image, capturedPaths);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                        return (false, "Lỗi không nhận diện được biển số!", null, null, null, new List<string>());
                    }
                }, cts.Token);

                // Đặt timeout 10 giây cho API call
                var timeoutTask = Task.Delay(10000, cts.Token);
                var completedTask = await Task.WhenAny(detectTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    cts.Cancel();
                    CommonService.Alert("Quá thời gian nhận diện, vui lòng thử lại!", Alert.Alert.enumType.Error);
                    return;
                }

                var result = await detectTask;
                if (result.Item1)
                {
                    if (!string.IsNullOrEmpty(result.Item3))
                    {
                        var i = _dbContext.TblBuHeader.Where(x =>
                            x.VehicleCode == result.Item3 &&
                            x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
                            x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
                            x.StatusVehicle != "04").ToList();
                        if (i.Count() == 1)
                        {
                            selectVehicle.SelectedValue = i.FirstOrDefault().Id;
                        }
                    }
                    pictureBoxLicensePlate.Image = CommonService.Base64ToImage(result.Item5);
                    IMGPATH = snapshotPath;
                    PLATEPATH = cropedPath;
                    lstPathImageCapture = result.Item6;
                    CommonService.Alert(result.Item2, Alert.Alert.enumType.Success);
                }
                else
                {
                    txtLicensePlate.Text = "";
                    pictureBoxLicensePlate.Image = null;
                    CommonService.Alert(result.Item2, Alert.Alert.enumType.Error);
                }
            }
            catch (Exception ex)
            {
                txtLicensePlate.Text = "";
                pictureBoxLicensePlate.Image = null;
                CommonService.Alert("Lỗi không nhận diện được biển số!", Alert.Alert.enumType.Error);
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                cts.Dispose();
            }
        }
        //private async void btnDetect_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        btnDetect.Enabled = false;
        //        var (filePath, snapshotImage) = CommonService.TakeSnapshot(viewStream.MediaPlayer);
        //        IMGPATH = filePath;

        //        if (!string.IsNullOrEmpty(filePath))
        //        {
        //            pictureBoxVehicle.Image = snapshotImage;

        //            var (licensePlate, croppedImage, savedImagePath) = await CommonService.DetectLicensePlateAsync(filePath);
        //            PLATEPATH = savedImagePath;
        //            if (!string.IsNullOrEmpty(licensePlate))
        //            {
        //                var i = _dbContext.TblBuHeader.Where(x =>
        //                    x.VehicleCode == licensePlate &&
        //                    x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
        //                    x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
        //                    x.IsCheckout == false).ToList();
        //                var name = i.FirstOrDefault().VehicleName;
        //                if (i.Count() == 1)
        //                {
        //                    selectVehicle.SelectedValue = i.FirstOrDefault().Id;
        //                }

        //                txtLicensePlate.Text = licensePlate;
        //                txtVehicleName.Text = name;
        //                pictureBoxLicensePlate.Image = croppedImage;
        //            }
        //        }
        //        CommonService.Alert("Nhận diện biển số thành công!", Alert.Alert.enumType.Success);

        //        //Lưu các ảnh từ camera vào thư mục
        //        var lstCamera = Global.lstCamera.Where(x => x.IsIn == true && x.Code != CameraDetect.Code).ToList();
        //        lstPathImageCapture = new List<string>();
        //        foreach (var c in lstCamera)
        //        {
        //            byte[] imageBytes = CommonService.CaptureFrameFromRTSP(c.Rtsp);
        //            var path = CommonService.SaveDetectedImage(imageBytes);
        //            lstPathImageCapture.Add(path);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonService.Alert($"Lỗi không nhận diện được biển số!", Alert.Alert.enumType.Error);
        //        txtLicensePlate.Text = "";
        //    }
        //    finally
        //    {
        //        btnDetect.Enabled = true;
        //    }
        //}
        #endregion

        #region Xử lý phương tiện chưa ra
        private void GetListQueue()
        {
            var lstQueue = _dbContext.TblBuHeader.Where(x =>
            x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
            x.WarehouseCode == ProfileUtilities.User.WarehouseCode &&
            x.StatusVehicle != "04" && x.StatusVehicle != "01").ToList();
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
                txtVehicleName.Text = detail.VehicleName;

                panelCheckIn.Controls.Clear();
                foreach (var doSap in detail.ListDOSAP)
                {
                    AppendPanelDetailCheckIn(doSap);
                }



                //_lstDOSAP.Clear();
                //panelCheckIn.Controls.OfType<DataGridView>().ToList()
                //    .ForEach(x => { x.Dispose(); panelCheckIn.Controls.Remove(x); });
                //panelCheckIn.Controls.OfType<Button>()
                //    .Where(x => x.Size.Width == 30)
                //    .ToList()
                //    .ForEach(x => { x.Dispose(); panelCheckIn.Controls.Remove(x); });

                //_lstDOSAP.AddRange(detail.ListDOSAP);

                // Kiểm tra trạng thái xe

                //var headerTgbx = _dbContext.TblBuHeaderTgbx.Where(x => x.HeaderId == selectedValue).ToList();
                //var detailTgbx = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == selectedValue).ToList();
                //var lstDo = detailTgbx.Select(x => x.SoLenh).Distinct().ToList();
                //var vehicle = _dbContext.TblBuHeader.Find(selectedValue);
                //if (vehicle.StatusProcess == "02" || vehicle.StatusProcess == "05" || lstDo.Count() == 0)
                //{
                //    CommonService.Alert($"Phương tiện không có ticket hoặc không xử lý!", Alert.Alert.enumType.Error);
                //    this.isHasInvoice = true; // Bỏ qua kiểm tra hóa đơn
                //    txtNoteOut.Text = "Phương tiện không có ticket hoặc không xử lý";
                //    return;
                //}

                //foreach (var doSap in lstDo)
                //{
                //    var lstData = detailTgbx.Where(x => x.SoLenh == doSap).ToList();
                //    AppendPanelDetail(lstData, headerTgbx.FirstOrDefault().MaPhuongTien);
                //}
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
            //try
            //{
            //    int yPosition = 6;
            //    var existingGrids = panelCheckIn.Controls.OfType<DataGridView>().ToList();
            //    if (existingGrids.Any())
            //    {
            //        yPosition = existingGrids.Last().Bottom + 6;
            //    }

            //    var res = CommonService.CheckInvoice(data.FirstOrDefault().SoLenh);
            //    var text = res.STATUS ? $"SỐ LỆNH XUẤT {data.FirstOrDefault().SoLenh} đã có hoá đơn" : $"SỐ LỆNH XUẤT {data.FirstOrDefault().SoLenh} chưa có hoá đơn";

            //    // CREATE LABEL
            //    var titleLabel = new Label
            //    {
            //        Text = text,
            //        Font = new Font("Segoe UI", 12, FontStyle.Bold),
            //        AutoSize = true,
            //        Location = new Point(10, yPosition),
            //    };
            //    titleLabel.ForeColor = res.STATUS ? Color.Green : Color.Red;

            //    // DATA GRID VIEW
            //    var dataGridView1 = new DataGridView
            //    {
            //        BackgroundColor = Color.White,
            //        BorderStyle = BorderStyle.None,
            //        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            //        ColumnHeadersHeight = 35,
            //        Location = new Point(0, yPosition + 35),
            //        Name = $"dataGridView_{panelCheckIn.Controls.Count + 1}",
            //        ReadOnly = true,
            //        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            //        AllowUserToAddRows = false,
            //        AllowUserToResizeRows = false,
            //        RowHeadersVisible = false,
            //        SelectionMode = DataGridViewSelectionMode.RowHeaderSelect,
            //        DefaultCellStyle = new DataGridViewCellStyle
            //        {
            //            SelectionBackColor = Color.Transparent,
            //            SelectionForeColor = Color.Black,
            //            Padding = new Padding(5),
            //            Font = new Font("Segoe UI", 12, FontStyle.Regular)
            //        },
            //        RowTemplate = { Height = 35 }
            //    };

            //    // HEADER STYLE
            //    dataGridView1.EnableHeadersVisualStyles = false;
            //    dataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            //    {
            //        BackColor = Color.FromArgb(52, 58, 64),
            //        ForeColor = Color.White,
            //        Font = new Font("Segoe UI", 12, FontStyle.Regular),
            //        Alignment = DataGridViewContentAlignment.MiddleCenter
            //    };
            //    dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            //    dataGridView1.GridColor = Color.Gray;

            //    // CREATE DATA TABLE
            //    DataTable dataTable = new DataTable();
            //    dataTable.Columns.Add("SỐ LỆNH XUẤT", typeof(string));
            //    dataTable.Columns.Add("PHƯƠNG TIỆN", typeof(string));
            //    dataTable.Columns.Add("MẶT HÀNG", typeof(string));
            //    dataTable.Columns.Add("SỐ LƯỢNG (ĐVT)", typeof(string));

            //    foreach (var item in data)
            //    {
            //        var materials = _dbContext.TblMdGoods.Find("000000000000" + item.MaHangHoa);
            //        string materialName = materials?.Name ?? "Unknown";
            //        dataTable.Rows.Add(data.FirstOrDefault().SoLenh, vehicleCode, materialName, $"{item.TongDuXuat} ({item.DonViTinh})");
            //    }

            //    dataGridView1.DataSource = dataTable;

            //    // CENTER ALIGNMENT
            //    foreach (DataGridViewColumn col in dataGridView1.Columns)
            //    {
            //        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    }

            //    // ADJUST GRID HEIGHT
            //    int totalHeight = dataGridView1.ColumnHeadersHeight + (dataTable.Rows.Count * dataGridView1.RowTemplate.Height) + 20;
            //    dataGridView1.Size = new Size(809, totalHeight);

            //    // ADD TO PANEL
            //    panelCheckIn.Controls.Add(titleLabel);
            //    panelCheckIn.Controls.Add(dataGridView1);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Lỗi hệ thống: {ex.Message}\nVui lòng liên hệ quản trị viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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
       
    }
}
