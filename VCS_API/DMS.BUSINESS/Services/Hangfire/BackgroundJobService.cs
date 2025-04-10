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
        private SMSInfo _config;
        public BackgroundJobService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _config = new SMSInfo
            {
                UrlSMS = "http://ams.tinnhanthuonghieu.vn:8009/bulkapi",
                Username = "smsbrand_xangdauna",
                Password = "xd@258369",
                CpCode = "XANGDAUNA",
                ServiceId = "CtyXdauN.an"
            };
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
                    DataTable tableData = new DataTable();
                    var query = $"SELECT MaPhuongTien FROM tblLenhXuatChiTiet " +
                        $"WHERE MaPhuongTien='{vehicle}' " +
                        $"AND TrangThai= 'KT' " +
                        $"AND NgayXuat = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MaHangHoa = {i.TdhCode}";

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

                    var querye5 = $"SELECT SO_PTIEN FROM LENH_GH " +
                        $"WHERE SO_PTIEN='{vehicle}' " +
                        $"AND TRANG_THAI= '3' " +
                        $"AND NGAY_DKY = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MA_HHOA = {i.TdhE5Code}";

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
                    DataTable tableData = new DataTable();
                    var querye5 = $"SELECT SO_PTIEN FROM LENH_GH " +
                        $"WHERE SO_PTIEN='{vehicle}' " +
                        $"AND TRANG_THAI= '3' " +
                        $"AND NGAY_DKY = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND MA_HHOA = {i.TdhE5Code}";

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

                    var material = i.GoodsCode.Substring(12);
                    var query = $"SELECT so_ptien FROM BX_BangMaLenh " +
                        $"WHERE so_ptien='{vehicle}' " +
                        $"AND Trang_thai_lenh= '4' " +
                        $"AND Time_tao_lenh = '{DateTime.Now.ToString("yyyy-MM-dd")}' AND Ma_hang = {i.TdhCode}";

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

        #region Gửi tin nhắn
        public async Task SendSMSJobs()
        {
            try
            {
                _dbContext.ChangeTracker.Clear();
                var lstQueueSMS = _dbContext.TblBuSmsQueue.Where(x => x.IsSend == false).ToList();
                foreach (var s in lstQueueSMS)
                {
                    var status = await SendSMS(ConvertPhoneNumber(s.Phone.Replace(" ", "")), s.SmsContent);
                    if (status)
                    {
                        s.IsSend = true;
                        _dbContext.TblBuSmsQueue.Update(s);
                        _dbContext.SaveChanges();
                        Console.WriteLine("Gửi tin nhắn thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Lỗi không gửi được tin nhắn");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string ConvertPhoneNumber(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.StartsWith("0") && phoneNumber.Length > 1)
            {
                return "84" + phoneNumber.Substring(1);
            }
            return phoneNumber;
        }

        public async Task<bool> SendSMS(string phone, string content)
        {
            try
            {
                string soapRequest = $@"
                    <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:impl='http://impl.bulkSms.ws/'>
                       <soapenv:Header/>
                       <soapenv:Body>
                          <impl:wsCpMt>
                             <User>{_config.Username}</User>
                             <Password>{_config.Password}</Password>
                             <CPCode>{_config.CpCode}</CPCode>
                             <RequestID>1</RequestID>
                             <UserID>{phone}</UserID>
                             <ReceiverID>{phone}</ReceiverID>
                             <ServiceID>{_config.ServiceId}</ServiceID>
                             <CommandCode>bulksms</CommandCode>
                             <Content>{content}</Content>
                             <ContentType>1</ContentType>
                          </impl:wsCpMt>
                       </soapenv:Body>
                    </soapenv:Envelope>";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("SOAPAction", "wsCpMt");
                    HttpContent contentData = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                    HttpResponseMessage response = await client.PostAsync(_config.UrlSMS, contentData);
                    var res = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(res))
                    {
                        if (res.Contains("<result>1</result>"))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không gửi được SMS! Chi tiết {ex.Message}");
                return false;
            }

        }
        #endregion

    }

    public class SMSInfo
    {
        public string? UrlSMS { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? CpCode { get; set; }
        public string? ServiceId { get; set; }
    }
}
