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
          //  GetHistoryData();
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
                int i = 0;
                foreach (var d in data)
                {
                    dataGridView.Rows.Add(new object[] { (i + 1).ToString(), "Test", d.VehicleCode, d.CreateDate , d.TimeCheckout, $"{d.NoteIn}\n{d.NoteOut}", "Test" });                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách lịch sử vào ra: {ex.Message}", "Lỗi",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }
        //private async void GetHistoryData()
        //{
        //    try
        //    {
        //        var data = await _dbContext.TblBuHeader.Where(x => x.WarehouseCode == ProfileUtilities.User.WarehouseCode && x.CompanyCode == ProfileUtilities.User.OrganizeCode).ToListAsync();
        //    }
        //    catch(Exception ex) 
        //    {
        //        MessageBox.Show($"Lỗi khi lấy danh sách lịch sử vào ra: {ex.Message}", "Lỗi",
        //           MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}


    }
}
