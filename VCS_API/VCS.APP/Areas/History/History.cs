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
                    dataGridView.Rows.Add(new object[] { i.ToString(), _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == d.VehicleCode.ToString())?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == d.VehicleCode.ToString())?.OicPtrip ?? "", d.VehicleCode, d.CreateDate, d.TimeCheckout, d.NoteIn,d.NoteOut, "Test", d.Id });
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
    }
}
