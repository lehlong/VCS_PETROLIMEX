using DMS.CORE;
using DMS.CORE.Entities.BU;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2013.Word;
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
using VCS.APP.Areas.PrintStt;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.Services;

namespace VCS.APP.Areas.History
{
    public partial class History : Form
    {
        private AppDbContextForm _dbContext;
        public History(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            fromDate.Value = DateTime.Now.Date;
            toDate.Value = DateTime.Now.Date.AddDays(1).AddTicks(-1);


            List<ComboBoxItem> items = new List<ComboBoxItem>();
            items.Add(new ComboBoxItem("-", ""));
            items.Add(new ComboBoxItem("Trong hàng chờ", "01"));
            items.Add(new ComboBoxItem("Đang trong kho", "02"));
            items.Add(new ComboBoxItem("Đang lấy hàng", "03"));
            items.Add(new ComboBoxItem("Đã ra kho", "04"));
            items.Add(new ComboBoxItem("Không xử lý", "05"));

            cbStatus.DataSource = items;
            cbStatus.DisplayMember = "Text";
            cbStatus.ValueMember = "Value";
        }
        private void History_Load(object sender, EventArgs e)
        {
            SearchData();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            var data = _dbContext.TblBuHeader.Where(x => x.CreateDate >= fromDate.Value && x.CreateDate <= toDate.Value 
            && x.CompanyCode == ProfileUtilities.User.OrganizeCode 
            && x.WarehouseCode == ProfileUtilities.User.WarehouseCode)
                .OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.Stt).AsQueryable();

            if (!string.IsNullOrEmpty(txtVehicleName.Text))
            {
                data = data.Where(x => x.VehicleName.Contains(txtVehicleName.Text));
            }
            if (!string.IsNullOrEmpty(txtVehicleCode.Text))
            {
                data = data.Where(x => x.VehicleCode.Contains(txtVehicleCode.Text));
            }
            ComboBoxItem selectedItem = (ComboBoxItem)cbStatus.SelectedItem;
            string selectedValue = selectedItem == null ? "" : selectedItem.Value;
            if (!string.IsNullOrEmpty(selectedValue))
            {
                data = data.Where(x => x.StatusVehicle == selectedValue);
            }
            dataTable.Rows.Clear();
            var order = 0;
            foreach (var i in data.ToList())
            {
                var statusVehicle = i.StatusVehicle == "01" ? "Trong hàng chờ" : i.StatusVehicle== "02" ? "Đang trong kho" : i.StatusVehicle == "03" ? "Đang lấy hàng" : i.StatusVehicle == "04" ? "Đã ra kho" : i.StatusVehicle == "05" ? "Không xử lý" : "";
                var timeCheckout = i.TimeCheckout.HasValue ? i.TimeCheckout.Value.ToString("dd/MM/yyyy hh:mm:ss") : "";
                dataTable.Rows.Add(i.Id, order, i.VehicleName, i.VehicleCode, i.Stt.ToString("00"), statusVehicle, i.CreateDate.Value.ToString("dd/MM/yyyy hh:mm:ss"), timeCheckout);
                order++;
            }
        }

        private void toDate_ValueChanged(object sender, EventArgs e)
        {
            var status = ValidateSearch();
            if (status)
            {
                SearchData();
            }
            else
            {
                return;
            }
        }

        private void fromDate_ValueChanged(object sender, EventArgs e)
        {
            var status = ValidateSearch();
            if (status)
            {
                SearchData();
            }
            else
            {
                return;
            }
        }

        private void cbStatus_SelectedValueChanged(object sender, EventArgs e)
        {
            var status = ValidateSearch();
            if (status)
            {
                SearchData();
            }
            else
            {
                return;
            }
        }

        private void txtVehicleCode_TextChanged(object sender, EventArgs e)
        {
            var status = ValidateSearch();
            if (status)
            {
                SearchData();
            }
            else
            {
                return;
            }
        }
        private void txtVehicleName_TextChanged(object sender, EventArgs e)
        {
            var status = ValidateSearch();
            if (status)
            {
                SearchData();
            }
            else
            {
                return;
            }
        }
        private bool ValidateSearch()
        {
            fromDate.Value = fromDate.Value.Date;
            toDate.Value = toDate.Value.Date.AddDays(1).AddTicks(-1);
            if (toDate.Value < fromDate.Value)
            {
                CommonService.Alert("Từ ngày > Đến ngày! Vui lòng kiểm tra lại!", VCS.Areas.Alert.Alert.enumType.Error);
                return false;
            }
            return true;
        }

        private void dataTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Bitmap iconBitmap = null;

                // Xác định icon cho từng cột
                if (e.ColumnIndex == dataTable.Columns["Edit"].Index)
                {
                    iconBitmap = Properties.Resources.icons8_list_18;
                }
                else if (e.ColumnIndex == dataTable.Columns["Print"].Index)
                {
                    iconBitmap = Properties.Resources.icons8_print_18__2_;
                }
                else if (e.ColumnIndex == dataTable.Columns["Cancel"].Index)
                {
                    iconBitmap = Properties.Resources.icons8_close_18;
                }

                // Nếu là cột chứa icon, thực hiện vẽ
                if (iconBitmap != null)
                {
                    // Vẽ nền trắng khi hover
                    if (dataTable.CurrentCell != null &&
                        dataTable.CurrentCell.RowIndex == e.RowIndex &&
                        dataTable.CurrentCell.ColumnIndex == e.ColumnIndex)
                    {
                        e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
                    }
                    else
                    {
                        e.Paint(e.CellBounds, DataGridViewPaintParts.Background);
                    }

                    // Vẽ icon căn giữa 18x18
                    int iconSize = 18;
                    int x = e.CellBounds.Left + (e.CellBounds.Width - iconSize) / 2;
                    int y = e.CellBounds.Top + (e.CellBounds.Height - iconSize) / 2;
                    e.Graphics.DrawImage(iconBitmap, new Rectangle(x, y, iconSize, iconSize));

                    // Vẽ lại border sau khi vẽ icon để không mất khung
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);

                    e.Handled = true;
                }
            }
        }

        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataTable.Columns["Print"].Index && e.RowIndex >= 0)
            {
                var header = _dbContext.TblBuHeader.Find(dataTable.Rows[e.RowIndex].Cells[0].Value.ToString());
                var t = new TicketInfo
                {
                    WarehouseName = _dbContext.TblMdWarehouse.FirstOrDefault(x => x.Code == ProfileUtilities.User.WarehouseCode)?.Name,
                    Vehicle = header.VehicleCode,
                    STT = header.Stt.ToString("00"),
                    DO_Code = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == header.Id).Select(x => x.Do1Sap).Distinct().ToList(),
                };
                var f = new STT(t, t.DO_Code);
                f.ShowDialog();

            }
            if (e.ColumnIndex == dataTable.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                var f = new DetailHistory(_dbContext, dataTable.Rows[e.RowIndex].Cells[0].Value.ToString());
                f.ShowDialog();
            }
            if (e.ColumnIndex == dataTable.Columns["Cancel"].Index && e.RowIndex >= 0)
            {
                var result = MessageBox.Show("Huỷ không xử lý phương tiện này?",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var header = _dbContext.TblBuHeader.Find(dataTable.Rows[e.RowIndex].Cells[0].Value.ToString());
                        var lstDo = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == header.Id).Select(x => x.Do1Sap).ToList();
                        if (header.StatusVehicle != "01")
                        {
                            CommonService.Alert("Chỉ được huỷ phương tiện trong hàng chờ!", VCS.Areas.Alert.Alert.enumType.Error);
                            return;
                        }
                        header.StatusVehicle = "05";
                        _dbContext.TblBuHeader.Update(header);
                        _dbContext.SaveChanges();

                        if (lstDo.Count() > 0)
                        {
                            var model = new PostStatusVehicleToSMO
                            {
                                VEHICLE = header.VehicleCode,
                                TYPE = "CANCEL",
                                LIST_DO = string.Join(",", lstDo),
                                DATE_INFO = DateTime.Now,
                            };
                            CommonService.PostStatusVehicleToSMO(model);
                        }

                        CommonService.Alert("Cập nhật thông tin thành công!", VCS.Areas.Alert.Alert.enumType.Success);
                        SearchData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lấy thông tin chi tiết: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void dataTable_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == dataTable.Columns["Edit"].Index ||
                             e.ColumnIndex == dataTable.Columns["Print"].Index ||
                             e.ColumnIndex == dataTable.Columns["Cancel"].Index))
            {
                dataTable.Cursor = Cursors.Hand;
            }
            else
            {
                dataTable.Cursor = Cursors.Default;
            }
        }
    }
}
