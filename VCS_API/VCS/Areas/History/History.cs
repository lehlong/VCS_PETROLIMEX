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
            var data = _dbContext.TblBuHeader.Where(x => x.CreateDate >= fromDate.Value && x.CreateDate <= toDate.Value).OrderByDescending(x => x.CreateDate).ThenByDescending(x => x.Stt).AsQueryable();
            if (!string.IsNullOrEmpty(txtVehicleName.Text))
            {
                data = data.Where(x => x.VehicleName.Contains(txtVehicleName.Text));
            }
            if (!string.IsNullOrEmpty(txtVehicleCode.Text))
            {
                data = data.Where(x => x.VehicleCode.Contains(txtVehicleCode.Text));
            }
            dataTable.Rows.Clear();

            var order = 0;
            foreach (var i in data.ToList())
            {
                var timeCheckout = i.TimeCheckout.HasValue ? i.TimeCheckout.Value.ToString("dd/MM/yyyy hh:mm:ss") : "";
                dataTable.Rows.Add(i.Id, order, i.VehicleName, i.VehicleCode, i.Stt.ToString("00"), "", i.CreateDate.Value.ToString("dd/MM/yyyy hh:mm:ss"), timeCheckout);
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
            if (e.ColumnIndex == dataTable.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                Bitmap iconBitmap = Properties.Resources.icons8_details_18;
                int iconSize = 18;
                int x = e.CellBounds.Left + (e.CellBounds.Width - iconSize) / 2;
                int y = e.CellBounds.Top + (e.CellBounds.Height - iconSize) / 2;
                e.Graphics.DrawImage(iconBitmap, new Rectangle(x, y, iconSize, iconSize));
                e.Handled = true;
            }
            if (e.ColumnIndex == dataTable.Columns["Print"].Index && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                Bitmap iconBitmap = Properties.Resources.icons8_print_18__1_;
                int iconSize = 18;
                int x = e.CellBounds.Left + (e.CellBounds.Width - iconSize) / 2;
                int y = e.CellBounds.Top + (e.CellBounds.Height - iconSize) / 2;
                e.Graphics.DrawImage(iconBitmap, new Rectangle(x, y, iconSize, iconSize));
                e.Handled = true;
            }
        }

        private void dataTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataTable.Columns["Print"].Index && e.RowIndex >= 0)
            {
                MessageBox.Show(dataTable.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            if (e.ColumnIndex == dataTable.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                var f = new DetailHistory(_dbContext, dataTable.Rows[e.RowIndex].Cells[0].Value.ToString());
                f.ShowDialog();
            }
        }
    }
}
