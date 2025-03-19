using DMS.CORE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.Hangfire
{
    public class BackgroundJobService
    {
        private AppDbContext _dbContext;
        public BackgroundJobService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Tích hợp với tự động hoá -> kiểm tra trạng thái -> xoá xe ở đầu nếu đã xử lý
        public void UpdateOrder()
        {
            var lstPumpThroat = _dbContext.TblMdPumpThroat.ToList();
            foreach (var i in lstPumpThroat)
            {
                var w = _dbContext.TblMdWarehouse.Find(i.WarehouseCode);
                if (w == null || string.IsNullOrEmpty(i.OrderVehicle)) continue;
                if (string.IsNullOrEmpty(w.Tdh)) continue;

                string[] order = i.OrderVehicle.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var vehicle = order[0].Trim();
                var material = i.GoodsCode.Substring(11);

                DataTable tableData = new DataTable();
                var query = $"SELECT MaPhuongTien FROM tblLenhXuatChiTiet " +
                    $"WHERE MaPhuongTien='{vehicle}' " +
                    $"AND TrangThai= 'KT' " +
                    $"AND NgayXuat = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MaHangHoa = {material}";
                using (SqlConnection con = new SqlConnection(w.Tdh))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    try
                    {
                        adapter.Fill(tableData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                if (tableData.Rows.Count > 0)
                {
                   i.OrderVehicle = string.Join(",", order.Skip(1));
                    _dbContext.TblMdPumpThroat.Update(i);
                }
            }
            _dbContext.SaveChanges();
            Console.WriteLine("Cập nhật thành công!");
        }
        #endregion

    }
}
