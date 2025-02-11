using DMS.CORE;
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
using VCS.APP.Utilities;

namespace VCS.APP.Areas.History
{
    public partial class History : Form
    {
        private AppDbContext _dbContext;
        public History(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            dataGridView.CellContentClick += dataGridView_CellContentClick;

        }

        private void fromDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void History_Load(object sender, EventArgs e)
        {
            try
            {
                var data = _dbContext.TblBuHeader.Where(x => x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).ToList();
                int i = 1;
               
                foreach (var d in data)
                {
                    dataGridView.Rows.Add(new object[] { 
                        i.ToString(),
                        _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == d.VehicleCode.ToString())?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == d.VehicleCode.ToString())?.OicPtrip ?? "",
                        d.VehicleCode,
                        d.CreateDate,
                        d.TimeCheckout,
                        d.NoteIn,
                        d.NoteOut,
                        _dbContext.TblBuOrders.Where(x => x.HeaderId == d.Id).Select(x => x.Stt).FirstOrDefault(),
                        d.Id });
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách lịch sử vào ra: {ex.Message}", "Lỗi",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                object cellValue = row.Cells[8].Value;
                DetailHistory detail = new DetailHistory(_dbContext, cellValue.ToString());
                detail.ShowDialog();
            }
        }
        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["RePrint"].Index && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background); 
                Image icon = Properties.Resources.icons8_print_18__1_;
                int iconSize = 18; 
                int iconX = e.CellBounds.Left + (e.CellBounds.Width - iconSize) / 2;
                int iconY = e.CellBounds.Top + (e.CellBounds.Height - iconSize) / 2;
                e.Graphics.DrawImage(icon, new Rectangle(iconX, iconY, iconSize, iconSize));
                e.Handled = true; 
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dataGridView.Columns["RePrint"].Index && e.RowIndex >= 0)
            {
                var headerId = dataGridView.Rows[e.RowIndex].Cells["Id"].Value;
                if (headerId != null)
                {
                    string id = headerId.ToString();
                    var data = _dbContext.TblBuHeader.Where(x => x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode && x.Id == id).FirstOrDefault();
                    var lstDO = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == id).Select(x => x.Do1Sap).ToList();
                    var ticketInfo = new TicketInfo
                    {
                        WarehouseName = GetNameWarehouse(),
                        Vehicle = data.VehicleCode,                       
                        STT = _dbContext.TblBuOrders.Where(x => x.HeaderId == id).Select(x => x.Stt).FirstOrDefault().ToString("00"),
                    };
                    STT sttForm = new STT(ticketInfo, lstDO);
                    sttForm.ShowDialog();
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


    }
}
