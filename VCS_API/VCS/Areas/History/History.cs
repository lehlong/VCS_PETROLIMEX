using System.Data;
using VCS.APP.Areas.PrintStt;
using VCS.APP.Services;
using VCS.APP.Utilities;
using VCS.DbContext.Common;
using VCS.Services;

namespace VCS.APP.Areas.History
{
    public partial class History : Form
    {
        private readonly AppDbContextForm _dbContext;
        public History(AppDbContextForm dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            fromDate.Value = DateTime.Now.Date;
            toDate.Value = DateTime.Now.Date.AddDays(1).AddTicks(-1);
            cbStatus.DataSource = new List<ComboBoxItem>
            {
                new ComboBoxItem("-", ""),
                new ComboBoxItem("Trong hàng chờ", "01"),
                new ComboBoxItem("Đang trong kho", "02"),
                new ComboBoxItem("Đang lấy hàng", "03"),
                new ComboBoxItem("Đã ra kho", "04"),
                new ComboBoxItem("Không xử lý", "05")
            };
            cbStatus.DisplayMember = "Text";
            cbStatus.ValueMember = "Value";
        }

        private void History_Load(object sender, EventArgs e) => SearchData();
        private void btnSearch_Click(object sender, EventArgs e) => SearchData();
        private void fromDate_ValueChanged(object sender, EventArgs e) => ValidateSearchAndSearch();
        private void toDate_ValueChanged(object sender, EventArgs e) => ValidateSearchAndSearch();
        private void cbStatus_SelectedValueChanged(object sender, EventArgs e) => ValidateSearchAndSearch();
        private void txtVehicleCode_TextChanged(object sender, EventArgs e) => ValidateSearchAndSearch();
        private void txtVehicleName_TextChanged(object sender, EventArgs e) => ValidateSearchAndSearch();

        private void ValidateSearchAndSearch()
        {
            if (ValidateSearch()) SearchData();
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
            if ((toDate.Value - fromDate.Value).TotalDays > 30)
            {
                CommonService.Alert("Chỉ được phép truy vấn tối đa 30 ngày!", VCS.Areas.Alert.Alert.enumType.Error);
                return false;
            }
            return true;
        }

        private void SearchData()
        {
            var data = _dbContext.TblBuHeader.Where(x => x.CreateDate >= fromDate.Value && x.CreateDate <= toDate.Value &&
                                                         x.CompanyCode == ProfileUtilities.User.OrganizeCode &&
                                                         x.WarehouseCode == ProfileUtilities.User.WarehouseCode);
            if (!string.IsNullOrEmpty(txtVehicleName.Text))
                data = data.Where(x => x.VehicleName.Contains(txtVehicleName.Text));
            if (!string.IsNullOrEmpty(txtVehicleCode.Text))
                data = data.Where(x => x.VehicleCode.Contains(txtVehicleCode.Text));

            string selectedValue = ((ComboBoxItem)cbStatus.SelectedItem)?.Value;
            if (!string.IsNullOrEmpty(selectedValue))
                data = data.Where(x => x.StatusVehicle == selectedValue);

            data = data.OrderByDescending(x => x.CreateDate).ThenByDescending(x => x.Stt);

            dataTable.Rows.Clear();
            int order = 0;
            foreach (var i in data.ToList())
            {
                string statusVehicle = i.StatusVehicle switch
                {
                    "01" => "Trong hàng chờ",
                    "02" => "Đang trong kho",
                    "03" => "Đang lấy hàng",
                    "04" => "Đã ra kho",
                    "05" => "Không xử lý",
                    _ => ""
                };
                string timeCheckout = i.TimeCheckout?.ToString("dd/MM/yyyy hh:mm:ss") ?? "";
                dataTable.Rows.Add(i.Id, order++, i.VehicleName, i.VehicleCode, i.Stt.ToString("00"), statusVehicle, i.CreateDate?.ToString("dd/MM/yyyy hh:mm:ss"), timeCheckout);
            }
        }

        private void dataTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var icons = new Dictionary<string, Bitmap>
            {
                {"Edit", Properties.Resources.icons8_list_18},
                {"Print", Properties.Resources.icons8_print_18__2_},
                {"Cancel", Properties.Resources.icons8_close_18}
            };
            if (icons.TryGetValue(dataTable.Columns[e.ColumnIndex].Name, out var iconBitmap))
            {
                if (dataTable.CurrentCell?.RowIndex == e.RowIndex && dataTable.CurrentCell.ColumnIndex == e.ColumnIndex)
                    e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
                else
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background);
                e.Graphics.DrawImage(iconBitmap, new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - 18) / 2, e.CellBounds.Top + (e.CellBounds.Height - 18) / 2, 18, 18));
                e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                e.Handled = true;
            }
        }

        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var id = dataTable.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (e.ColumnIndex == dataTable.Columns["Print"].Index)
            {
                var header = _dbContext.TblBuHeader.Find(id);
                var t = new TicketInfo
                {
                    WarehouseName = _dbContext.TblMdWarehouse.FirstOrDefault(x => x.Code == ProfileUtilities.User.WarehouseCode)?.Name,
                    Vehicle = header.VehicleCode,
                    STT = header.Stt.ToString("00"),
                    DO_Code = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == header.Id).Select(x => x.Do1Sap).Distinct().ToList()
                };
                new STT(t, t.DO_Code).ShowDialog();
            }
            else if (e.ColumnIndex == dataTable.Columns["Edit"].Index)
            {
                new DetailHistory(_dbContext, id).ShowDialog();
            }
            else if (e.ColumnIndex == dataTable.Columns["Cancel"].Index)
            {
                if (MessageBox.Show("Huỷ không xử lý phương tiện này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var header = _dbContext.TblBuHeader.Find(id);
                    if (header.StatusVehicle != "01")
                    {
                        CommonService.Alert("Chỉ được huỷ phương tiện trong hàng chờ!", VCS.Areas.Alert.Alert.enumType.Error);
                        return;
                    }
                    header.StatusVehicle = "05";
                    _dbContext.TblBuHeader.Update(header);
                    _dbContext.SaveChanges();
                    CommonService.Alert("Cập nhật thông tin thành công!", VCS.Areas.Alert.Alert.enumType.Success);
                    SearchData();
                }
            }
        }
        private void dataTable_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == dataTable.Columns["Edit"].Index ||
                                     e.ColumnIndex == dataTable.Columns["Print"].Index ||
                                     e.ColumnIndex == dataTable.Columns["Cancel"].Index))
                dataTable.Cursor = Cursors.Hand;
            else
                dataTable.Cursor = Cursors.Default;
        }
    }
}
