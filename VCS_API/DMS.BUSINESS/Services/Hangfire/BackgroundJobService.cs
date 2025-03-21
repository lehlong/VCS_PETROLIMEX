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
                if (w == null) continue;
                if (string.IsNullOrEmpty(i.OrderVehicle)) continue;

                string[] order = i.OrderVehicle.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var vehicle = order[0].Trim();

                #region Kho Bến Thuỷ
                if (w.Code == "2810-BT")
                {
                    var material = i.GoodsCode == "000000000000201004" ? i.GoodsCode.Substring(11) : i.GoodsCode.Substring(12);
                    DataTable tableData = new DataTable();
                    var query = $"SELECT MaPhuongTien FROM tblLenhXuatChiTiet " +
                        $"WHERE MaPhuongTien='{vehicle}' " +
                        $"AND TrangThai= 'KT' " +
                        $"AND NgayXuat = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MaHangHoa = {material}";

                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    var materiale5 = i.GoodsCode.Substring(11);
                    var querye5 = $"SELECT SO_PTIEN FROM LENH_GH " +
                        $"WHERE SO_PTIEN='{vehicle}' " +
                        $"AND TRANG_THAI= '3' " +
                        $"AND NGAY_DKY = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MA_HHOA = {materiale5}";

                    try
                    {
                        using (SqlConnection con = new SqlConnection(w.Tdh_e5))
                        {
                            SqlCommand cmd = new SqlCommand(querye5, con);
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }



                    if (tableData.Rows.Count > 0)
                    {
                        i.OrderVehicle = string.Join(",", order.Skip(1));
                        _dbContext.TblMdPumpThroat.Update(i);
                    }
                }
                #endregion

                #region Kho Nghi Hương
                if (w.Code == "2810-NH")
                {
                    var materiale5 = i.GoodsCode == "000000000000201004" ? i.GoodsCode.Substring(11) : i.GoodsCode.Substring(12);
                    DataTable tableData = new DataTable();
                    var querye5 = $"SELECT SO_PTIEN FROM LENH_GH " +
                        $"WHERE SO_PTIEN='{vehicle}' " +
                        $"AND TRANG_THAI= '3' " +
                        $"AND NGAY_DKY = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MA_HHOA = {materiale5}";

                    try
                    {
                        using (SqlConnection con = new SqlConnection(w.Tdh_e5))
                        {
                            SqlCommand cmd = new SqlCommand(querye5, con);
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    var material =i.GoodsCode.Substring(12);
                    var query = $"SELECT so_ptien FROM LENH_GH " +
                        $"WHERE so_ptien='{vehicle}' " +
                        $"AND Trang_thai_lenh= '4' " +
                        $"AND Time_tao_lenh = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND Ma_hang = {material}";

                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }




                    if (tableData.Rows.Count > 0)
                    {
                        i.OrderVehicle = string.Join(",", order.Skip(1));
                        _dbContext.TblMdPumpThroat.Update(i);
                    }
                }
                #endregion
            }
            _dbContext.SaveChanges();
            Console.WriteLine("Cập nhật thành công!");
        }
        #endregion

    }
}
