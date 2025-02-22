﻿using DMS.CORE;
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
using VCS.APP.Areas.ViewAllCamera;
using VCS.APP.Areas.PrintStt;
using System.Drawing.Printing;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Util;
using System;


namespace VCS.APP.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private AppDbContextForm _dbContext;
        private List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        private Dictionary<string, MediaPlayer> _mediaPlayers = new Dictionary<string,MediaPlayer>();
        private LibVLC? _libVLC;
        private List<DOSAPDataDto> _lstDOSAP = new List<DOSAPDataDto>();
        private List<string> lstCheckDo = new List<string>();
        private List<string> lstPathImageCapture = new List<string>();
        private string IMGPATH;    
        private string PLATEPATH;
        public CheckIn(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }
        private void CheckIn_Load(object sender, EventArgs e)
        {
            InitializeLibVLC();
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
                        && x.IsIn && x.IsRecognition) // Lọc camera cổng vào
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
                txtStatus.Text = "Đang nhận diện...";
                txtStatus.ForeColor = Color.DarkGoldenrod;
                btnDetect.Enabled = false;
                var (filePath, snapshotImage) = await CommonService.TakeSnapshot(videoView.MediaPlayer);
                IMGPATH = filePath;

                //Lưu các ảnh từ camera vào thư mục
                var lstCamera = _dbContext.TblMdCamera.Where(x => x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.OrgCode == ProfileUtilities.User.OrganizeCode && x.IsIn == true).ToList();
                lstPathImageCapture = new List<string>();
                foreach (var c in lstCamera)
                {
                    byte[] imageBytes = CommonService.CaptureFrameFromRTSP(c.Rtsp);
                    var path = CommonService.SaveDetectedImage(imageBytes);
                    lstPathImageCapture.Add(path);
                }

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
                txtStatus.Text = "Nhận diện thành công!";
                txtStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                txtLicensePlate.Text = "";
                pictureBoxLicensePlate.Image = null;
                txtStatus.Text = "Lỗi không nhận diện được biển số!";
                txtStatus.ForeColor = Color.Red;
                txtLicensePlate.Text = "";
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
                int yPosition = 235;
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
                var dataGridView1 = new DataGridView
                {
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                    ColumnHeadersHeight = 35,
                    Location = new Point(18, yPosition + 35),
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicensePlate.Text))
            {
                txtStatus.Text = "Không có thông tin phương tiện! Vui lòng kiểm tra lại!";
                txtStatus.ForeColor = Color.Red;
                return;
            }
            if (txtLicensePlate.Text.Length > 8)
            {
                txtStatus.Text = "Thông tin biển số không được vượt quá 8 ký tự!";
                txtStatus.ForeColor = Color.Red;
                return;
            }
            var c = _dbContext.TblBuHeader.Where(x => x.VehicleCode == txtLicensePlate.Text && x.StatusVehicle != "04"
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).Count();
            if (c != 0)
            {
                txtStatus.Text = "Phương tiện đã có trong hàng chờ hoặc trong kho!";
                txtStatus.ForeColor = Color.Red;
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

            foreach(var i in lstPathImageCapture)
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
            await _dbContext.SaveChangesAsync();
            ReloadForm(_dbContext);
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
                if (number.Length > 10)
                {
                    txtStatus.Text = "Số lệnh xuất không vượt quá 10 ký tự";
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
            if (txtLicensePlate.Text.Length > 8)
            {
                txtStatus.Text = "Thông tin biển số không được vượt quá 8 ký tự!";
                txtStatus.ForeColor = Color.Red;
                return;
            }

            var c = _dbContext.TblBuHeader.Where(x => x.VehicleCode == txtLicensePlate.Text && x.StatusVehicle != "04"
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).Count();

            ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
            string selectedHeaderId = selectedItem.Value;

            if (c != 0 && string.IsNullOrEmpty(selectedHeaderId))
            {
                txtStatus.Text = "Phương tiện đã có trong hàng chờ hoặc trong kho!";
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

                //var orderDto = new OrderDto
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    HeaderId = headerId,
                //    VehicleCode = txtLicensePlate.Text,
                //    Name = name,
                //    Count = "0",
                //    IsActive = true,
                //    WarehouseCode = ProfileUtilities.User.WarehouseCode,
                //    CompanyCode = ProfileUtilities.User.OrganizeCode
                //};

                //if (result == null)
                //{
                //    txtStatus.Text = "Cho xe vào không thành công! Vui lòng kiểm tra lại!";
                //    txtStatus.ForeColor = Color.Red;
                //    return;
                //}
                txtStatus.Text = "Cho xe vào thành công!";
                txtStatus.ForeColor = Color.Green;
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
                WarehouseName = GetNameWarehouse(),
                Vehicle = txtLicensePlate.Text,
                STT = _stt.ToString("00"),
            };
            STT sttForm = new STT(ticketInfo, lstCheckDo);
            sttForm.ShowDialog();
            ReloadForm(_dbContext);
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
        private void ReloadForm(AppDbContextForm dbContext)
        {
            this.Controls.Clear();
            InitializeComponent();
            _dbContext = dbContext;
            ResetVarible();
            InitializeLibVLC();
            GetListCameras();
            GetListQueue();
        }
        private void ResetVarible()
        {
            _lstDOSAP = new List<DOSAPDataDto>();
            lstCheckDo = new List<string>();
            _lstCamera = new List<TblMdCamera>();
            _mediaPlayers = new Dictionary<string, MediaPlayer>();
            lstPathImageCapture = new List<string>();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ReloadForm(_dbContext);
        }

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

                if (string.IsNullOrEmpty(selectedValue))
                {
                    button3.Visible = false;
                    btnDeleteQueue.Visible = false;
                    return;
                }
                btnDeleteQueue.Visible = true;
                button3.Visible = true;

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
                if (txtLicensePlate.Text.Length > 8)
                {
                    txtStatus.Text = "Thông tin biển số không được vượt quá 8 ký tự";
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

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                _lstCamera = _dbContext.TblMdCamera
                    .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                        && x.WarehouseCode == ProfileUtilities.User.WarehouseCode
                        && x.IsIn)
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

        private void button2_Click(object sender, EventArgs e)
        {
            ReloadForm(_dbContext);
        }

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
                _dbContext.TblBuHeader.RemoveRange(_dbContext.TblBuHeader.Where(x => x.Id == ((ComboBoxItem)comboBox1.SelectedItem).Value));
                _dbContext.TblBuDetailDO.RemoveRange(_dbContext.TblBuDetailDO.Where(x => x.HeaderId == ((ComboBoxItem)comboBox1.SelectedItem).Value));
                _dbContext.TblBuImage.RemoveRange(_dbContext.TblBuImage.Where(x => x.HeaderId == ((ComboBoxItem)comboBox1.SelectedItem).Value));
                _dbContext.SaveChanges();
                ReloadForm(_dbContext);
            }
        }
    }
}
