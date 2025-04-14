using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Services.HUB;
using DMS.BUSINESS.Services.MD;
using DMS.COMMON.Common.Class;
using DMS.CORE;
using DMS.CORE.Common;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DMS.CORE.Migrations;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using MathNet.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record.Chart;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static DMS.BUSINESS.Models.ReportModel;


namespace DMS.BUSINESS.Services.BU
{
    public interface IOrderService : IGenericService<TblBuOrder, OrderDto>
    {
        Task<List<TblBuHeader>> GetOrder(BaseFilter filter);
        Task<List<ArrangePumpNozzleModel>> ArrangePumpNozzle(BaseFilter filter);
        Task<List<TblBuHeader>> GetOrderDisplay(BaseFilter filter);
        Task<List<TblBuOrder>> UpdateOrderCall(OrderUpdateDto orderDto);
        Task<List<TblBuOrder>> UpdateOrderCome(OrderUpdateDto orderDto);
        Task Order(OrderDto orderDto);
        Task<bool> CheckTicket(string headerId);
        Task<bool> ReCheckTicket(string headerId);
        Task<bool> UpdateOrder(string headerId);
        Task<TicketModel> GetTicket(string headerId);
        List<TblBuHeaderTgbx> ConvertToHeader(DataTable dataTable, string headerId);
        List<TblBuDetailTgbx> ConvertToDetail(DataTable dataTable, string headerId);
        Task<List<string>> AsyncUploadFile(List<IFormFile> files, List<string> filePath);
        Task UpdateStatus(TblBuHeader header);
        Task<List<BaoCaoChiTietXeModel>> BaoCaoXeChiTiet(FilterReport filter);
        Task<byte[]> ExportExcelBaoCaoXeChiTiet(FilterReport filter);
    }
    public class OrderService : GenericService<TblBuOrder, OrderDto>, IOrderService
    {
        private readonly IHubContext<OrderHub> _hubContext;
        public OrderService(AppDbContext dbContext, IMapper mapper, IHubContext<OrderHub> hubContext)
            : base(dbContext, mapper)
        {
            _hubContext = hubContext;
        }
        public async Task<List<TblBuHeader>> GetOrder(BaseFilter filter)
        {
            try
            {
                var data = await _dbContext.TblBuHeader.Where(x => x.StatusVehicle != "01" && x.StatusVehicle != "04"
                && x.CompanyCode == filter.OrgCode && x.WarehouseCode == filter.WarehouseCode && x.CreateDate.Value.Date == DateTime.Now.Date
                ).OrderBy(x => x.Stt).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<List<ArrangePumpNozzleModel>> ArrangePumpNozzle(BaseFilter filter)
        {
            try
            {
                var data = new List<ArrangePumpNozzleModel>();
                var _pt = await _dbContext.TblMdPumpThroat
                    .Where(x => x.WarehouseCode == filter.WarehouseCode && x.OrgCode == filter.OrgCode)
                    .OrderBy(x => x.PumpRigCode).ThenBy(x => x.Code)
                    .ToListAsync();

                var pumpRigCodes = _pt.Select(pt => pt.PumpRigCode).Distinct().ToList();
                var goodsCodes = _pt.Select(pt => pt.GoodsCode).Distinct().ToList();

                var pumpRigs = await _dbContext.TblMdPumpRig.Where(pr => pumpRigCodes.Contains(pr.Code)).ToListAsync();
                var goods = await _dbContext.TblMdGoods.Where(g => goodsCodes.Contains(g.Code)).ToListAsync();

                var pumpRigLookup = pumpRigs.ToDictionary(pr => pr.Code, pr => pr.Name);
                var goodsLookup = goods.ToDictionary(g => g.Code, g => g.Name);

                foreach (var pt in _pt)
                {
                    data.Add(new ArrangePumpNozzleModel
                    {
                        CompanyCode = filter.OrgCode,
                        WarehouseCode = filter.WarehouseCode,
                        PumpRigCode = pt.PumpRigCode,
                        PumpRigName = pumpRigLookup.GetValueOrDefault(pt.PumpRigCode),
                        PumpThroatCode = pt.Code,
                        PumpThroatName = pt.Name,
                        MaterialCode = pt.GoodsCode,
                        MaterialName = goodsLookup.GetValueOrDefault(pt.GoodsCode),
                        Order = string.IsNullOrEmpty(pt.OrderVehicle) ? new List<string>() : pt.OrderVehicle.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    });
                }
                var d = _dbContext.TblAdConfigDisplay.Find(filter.DisplayId);
                if (d == null)
                {
                    return data.OrderBy(x => x.PumpRigCode).ThenBy(x => x.PumpThroatCode).ToList();
                }
                else
                {
                    return data.Skip(d.Cfrom).Take(d.Cto - d.Cfrom).OrderBy(x => x.PumpRigCode).ThenBy(x => x.PumpThroatCode).ToList();
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return new List<ArrangePumpNozzleModel>();
            }
        }
        public async Task<List<TblBuHeader>> GetOrderDisplay(BaseFilter filter)
        {
            try
            {
                var display = _dbContext.TblAdConfigDisplay.Find(filter.DisplayId);

                var data = await _dbContext.TblBuHeader.Where(x => x.StatusVehicle == "02"
                && x.CompanyCode == filter.OrgCode && x.WarehouseCode == filter.WarehouseCode && x.CreateDate.Value.Date == DateTime.Now.Date
                ).OrderBy(x => x.Stt).ToListAsync();

                return data.Skip(display.Cfrom).Take(display.Cto - display.Cfrom).ToList();

            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task UpdateStatus(TblBuHeader header)
        {
            try
            {
                if (header.StatusProcess == "01")
                {
                    header.IsVoice = true;
                    var lst = await _dbContext.TblBuHeader.Where(x => x.Id != header.Id
                                && x.CompanyCode == header.CompanyCode && x.WarehouseCode == header.WarehouseCode
                                && x.CreateDate.Value.Date == DateTime.Now.Date).OrderBy(x => x.Stt).ToListAsync();
                    foreach (var i in lst)
                    {
                        i.IsVoice = false;
                        _dbContext.TblBuHeader.Update(i);
                    }
                }
                else
                {
                    header.IsVoice = false;
                }


                _dbContext.TblBuHeader.Update(header);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }
        public async Task<List<TblBuOrder>> UpdateOrderCall(OrderUpdateDto orderDto)
        {
            try
            {
                // Lấy danh sách orders theo điều kiện
                var orders = await _dbContext.TblBuOrders
                    .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                               x.WarehouseCode == orderDto.WarehouseCode &&
                               x.CompanyCode == orderDto.CompanyCode)
                    .OrderBy(x => x.Stt)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    order.IsCall = false;
                }

                var selectedOrder = orders.FirstOrDefault(x => x.Id == orderDto.Id);
                if (selectedOrder != null)
                {
                    selectedOrder.IsCall = true;
                }

                await _dbContext.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync(SignalRMethod.ORDER_CALL.ToString(), orders);

                return orders;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task<List<TblBuOrder>> UpdateOrderCome(OrderUpdateDto orderDto)
        {
            try
            {
                var order = await _dbContext.TblBuOrders
                    .FirstOrDefaultAsync(x => x.Id == orderDto.Id &&
                                            x.CreateDate.Value.Date == DateTime.Now.Date &&
                                            x.WarehouseCode == orderDto.WarehouseCode &&
                                            x.CompanyCode == orderDto.CompanyCode);

                if (order != null)
                {
                    order.IsCome = orderDto.IsCome;
                    order.IsDone = orderDto.IsDone;
                    await _dbContext.SaveChangesAsync();
                    var updatedOrders = await _dbContext.TblBuOrders
                        .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                                   x.WarehouseCode == orderDto.WarehouseCode &&
                                   x.CompanyCode == orderDto.CompanyCode)
                        .OrderBy(x => x.Stt)
                        .ToListAsync();

                    // Gửi thông báo qua SignalR
                    await _hubContext.Clients.All.SendAsync(SignalRMethod.ORDER_COME.ToString(), updatedOrders);

                    return updatedOrders;
                }

                return null;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public override async Task<OrderDto> Add(IDto dto)
        {
            try
            {
                var orderDto = dto as OrderDto;
                if (orderDto == null)
                {
                    Status = false;
                    MessageObject.Code = "0001";
                    return null;
                }

                // Tính Order mới
                var maxOrder = await _dbContext.TblBuOrders
                    .Where(q => q.CreateDate.Value.Date == DateTime.Now.Date &&
                               q.WarehouseCode == orderDto.WarehouseCode &&
                               q.CompanyCode == orderDto.CompanyCode)
                    .MaxAsync(x => (int?)x.Order) ?? 0;

                orderDto.Order = (maxOrder + 1).ToString();

                // Tính STT mới
                var maxStt = await _dbContext.TblBuOrders
                    .Where(q => q.CreateDate.Value.Date == DateTime.Now.Date &&
                               q.WarehouseCode == orderDto.WarehouseCode &&
                               q.CompanyCode == orderDto.CompanyCode)
                    .MaxAsync(x => (int?)x.Stt) ?? 0;

                orderDto.Stt = maxStt + 1;

                var result = await base.Add(dto);

                if (result != null)
                {
                    var updatedOrders = await _dbContext.TblBuOrders
                        .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                                   x.WarehouseCode == orderDto.WarehouseCode &&
                                   x.CompanyCode == orderDto.CompanyCode)
                        .OrderBy(x => x.Stt)
                        .ToListAsync();

                    // Gửi thông báo realtime qua SignalR
                    await _hubContext.Clients.All.SendAsync(SignalRMethod.ORDER_LIST_CHANGED.ToString(), updatedOrders);
                }

                return result;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        public async Task Order(OrderDto orderDto)
        {
            try
            {
                var w = _dbContext.TblMdWarehouse.Find(orderDto.WarehouseCode);
                if (w == null)
                {
                    Status = false;
                    MessageObject.MessageDetail = "Lỗi hệ thống! Vui lòng liên hệ với quản trị viên!";
                    return;
                }
                if (string.IsNullOrEmpty(w.Tgbx) || string.IsNullOrEmpty(w.Tdh) || string.IsNullOrEmpty(w.Tdh_e5))
                {
                    MessageObject.MessageDetail = "Chưa cấu hình đủ thông tin kết nối hệ thống tại kho! Vui lòng kiểm tra lại!";
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }
        public async Task<bool> CheckTicket(string headerId)
        {
            try
            {
                var i = _dbContext.TblBuHeader.Find(headerId);
                if (i == null) return false;

                var w = _dbContext.TblMdWarehouse.Find(i.WarehouseCode);
                if (w == null) return false;

                var query = $"SELECT * FROM tblLenhXuatE5 WHERE Status = '3' AND MaPhuongTien = '{i.VehicleCode}' AND NgayXuat = '{DateTime.Now:yyyy-MM-dd}'";
                DataTable tableData = new DataTable();

                using (SqlConnection con = new SqlConnection(w.Tgbx))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            await Task.Run(() => adapter.Fill(tableData));
                        }
                    }
                }

                if (tableData.Rows.Count > 0 || i.IsPrint == true)
                {
                    if (!i.IsPrint.HasValue || !i.IsPrint.Value)
                    {
                        var lstDetail = new List<TblBuDetailTgbx>();
                        var lstDetailTicket = new List<TblBuTgbxTicket>();
                        var h = ConvertToHeader(tableData, i.Id);

                        foreach (var _h in h)
                        {
                            var queryDetail = $"SELECT * FROM tblLenhXuat_HangHoaE5 WHERE SoLenh = '{_h.SoLenh}'";
                            DataTable tblDetail = new DataTable();

                            using (SqlConnection conDetail = new SqlConnection(w.Tgbx))
                            {
                                await conDetail.OpenAsync();
                                using (SqlCommand cmdDetail = new SqlCommand(queryDetail, conDetail))
                                {
                                    cmdDetail.CommandType = CommandType.Text;
                                    using (SqlDataAdapter adapterDetail = new SqlDataAdapter(cmdDetail))
                                    {
                                        await Task.Run(() => adapterDetail.Fill(tblDetail));
                                    }
                                }
                            }

                            if (tblDetail.Rows.Count > 0)
                            {
                                var d = ConvertToDetail(tblDetail, i.Id);
                                lstDetail.AddRange(d);

                                foreach (var _d in d)
                                {
                                    var queryDetail2 = $"SELECT * FROM tblLenhXuatChiTietE5 WHERE TableID = '{_d.TableID}'";
                                    DataTable tblDetail2 = new DataTable();

                                    using (SqlConnection conDetail2 = new SqlConnection(w.Tgbx))
                                    {
                                        await conDetail2.OpenAsync();
                                        using (SqlCommand cmdDetail2 = new SqlCommand(queryDetail2, conDetail2))
                                        {
                                            cmdDetail2.CommandType = CommandType.Text;
                                            using (SqlDataAdapter adapterDetail2 = new SqlDataAdapter(cmdDetail2))
                                            {
                                                await Task.Run(() => adapterDetail2.Fill(tblDetail2)); 
                                            }
                                        }
                                    }

                                    if (tblDetail2.Rows.Count > 0)
                                    {
                                        var d2 = ConvertToDetailTicket(tblDetail2, i.Id);
                                        lstDetailTicket.AddRange(d2);
                                    }
                                }
                            }
                        }

                        i.IsPrint = true;
                        _dbContext.TblBuHeaderTgbx.AddRange(h);
                        _dbContext.TblBuDetailTgbx.AddRange(lstDetail);
                        _dbContext.TblBuTgbxTicket.AddRange(lstDetailTicket);
                        await _dbContext.SaveChangesAsync();
                    }

                    _dbContext.TblBuHeader.Update(i);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    i.StatusProcess = "02";
                    i.NoteIn = "Phương tiện chưa có ticket";
                    _dbContext.TblBuHeader.Update(i);
                    await _dbContext.SaveChangesAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ReCheckTicket(string headerId)
        {
            try
            {
                var i = _dbContext.TblBuHeader.Find(headerId);
                if (i == null) return false;

                i.IsPrint = false;
                _dbContext.TblBuHeader.Update(i);

                await _dbContext.TblBuHeaderTgbx.Where(x => x.HeaderId == headerId).ExecuteDeleteAsync();
                await _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == headerId).ExecuteDeleteAsync();
                await _dbContext.TblBuTgbxTicket.Where(x => x.HeaderId == headerId).ExecuteDeleteAsync();
                var lstOrder = await _dbContext.TblMdPumpThroat.ToListAsync();
                foreach (var o in lstOrder)
                {
                    if (!string.IsNullOrEmpty(o.OrderVehicle))
                    {
                        o.OrderVehicle = o.OrderVehicle.Replace(i.VehicleCode, "");
                        _dbContext.TblMdPumpThroat.Update(o);
                    }
                }
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UpdateOrder(string headerId)
        {
            try
            {
                var header = _dbContext.TblBuHeader.Find(headerId);
                header.StatusVehicle = "03";
                header.StatusProcess = "03";
                header.NoteIn = "";
                header.NoteOut = "";
                _dbContext.TblBuHeader.Update(header);

                var lstPumpThroat = await _dbContext.TblMdPumpThroat.Where(x => x.OrgCode == header.CompanyCode && x.WarehouseCode == header.WarehouseCode).ToListAsync();

                var lstDetail = await _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == headerId).ToListAsync();
                foreach(var i in lstDetail)
                {
                    var o = lstPumpThroat.Where(x => x.GoodsCode.Contains(i.MaHangHoa))
                      .OrderBy(x => x.OrderVehicle.Length)
                      .FirstOrDefault();
                    o.OrderVehicle = o.OrderVehicle + "," + header.VehicleCode;
                    _dbContext.TblMdPumpThroat.Update(o);

                    i.OrderName = o.Name;
                    _dbContext.TblBuDetailTgbx.Update(i);
                }
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<TicketModel> GetTicket(string headerId)
        {
            try
            {
                var i = _dbContext.TblBuHeader.Find(headerId);
                var tgbx = _dbContext.TblBuHeaderTgbx.FirstOrDefault(x => x.HeaderId == headerId);
                var w = _dbContext.TblMdWarehouse.Find(i.WarehouseCode);

                #region Lấy thông tin khách hàng
                var query = $"SELECT * FROM tblKhachHang WHERE MaKhachHang = '{tgbx.MaKhachHang}'";
                DataTable tbl = new DataTable();

                using (SqlConnection con = new SqlConnection(w.Tgbx))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            await Task.Run(() => adapter.Fill(tbl));
                        }
                    }
                }
                string tenKhachHang = string.Empty;

                if (tbl.Rows.Count > 0)
                {
                    tenKhachHang = tbl.Rows[0]["TenKhachHang"]?.ToString();
                }
                #endregion

                #region Lấy số ticket
                var query1 = $"SELECT * FROM tblTichke WHERE SoLenh = '{tgbx.SoLenh}'";
                DataTable tbl1 = new DataTable();

                using (SqlConnection con = new SqlConnection(w.Tgbx))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query1, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            await Task.Run(() => adapter.Fill(tbl1));
                        }
                    }
                }
                string soTicket = string.Empty;

                if (tbl.Rows.Count > 0)
                {
                    soTicket = tbl1.Rows[0]["SoTichKe"]?.ToString();
                }
                #endregion
                var d = new TicketModel
                {
                    CompanyName = _dbContext.tblAdOrganize.Find(i.CompanyCode).Name,
                    DateTime = $"Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}",
                    Vehicle = i.VehicleCode,
                    DriverName = tgbx?.NguoiVanChuyen,
                    PtBan = $"{tgbx?.MaPhuongThucBan} - {_dbContext.TblMdSalesMethod.Find(tgbx?.MaPhuongThucBan)?.Name}",
                    CustmerName = $"{tgbx?.MaKhachHang} - {tenKhachHang} - {tgbx.DiemTraHang}",
                    ChuyenVt = tgbx?.LoaiPhieu,
                    TicketNumber = soTicket
                };
                var tgbxDetail = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == headerId).OrderBy(x => x.SoLenh).ThenBy(x => x.MaHangHoa).ToList();
                foreach (var _d in tgbxDetail)
                {
                    var gCode = "00000000000" + _d.MaHangHoa;
                    var detail = _dbContext.TblBuTgbxTicket.Where(x => x.TableId == _d.TableID).OrderBy(x => x.MaNgan).ToList();
                    foreach (var (t, index) in detail.Select((t, index) => (t, index)))
                    {
                        var _t = new DetailTicket
                        {
                            SoLenh = _d.SoLenh,
                            MaTdh = t.MaTuDongHoa,
                            HangHoa = _dbContext.TblMdGoods.Find(gCode)?.Name,
                            MaBe = _d.BeXuat,
                            Ngan = t.MaNgan,
                            DungTich = t.SoLuongDuXuat,
                            NhietDo = t.NhietDo,
                            GianHong = _d.OrderName,
                            Rowspan = (index == 0) ? detail.Count : 0
                        };
                        d.Details.Add(_t);
                    }
                }
                d.Details = d.Details.OrderBy(x =>x.Ngan).ToList();
                return d;

            }
            catch (Exception ex)
            {
                return new TicketModel();
            }
        }
        public List<TblBuHeaderTgbx> ConvertToHeader(DataTable dataTable, string headerId)
        {
            try
            {
                List<TblBuHeaderTgbx> list = new List<TblBuHeaderTgbx>();
                foreach (DataRow row in dataTable.Rows)
                {
                    TblBuHeaderTgbx model = new TblBuHeaderTgbx
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = headerId,
                        MaLenh = row["MaLenh"] != DBNull.Value ? row["MaLenh"].ToString() : string.Empty,
                        SoLenh = row["SoLenh"] != DBNull.Value ? row["SoLenh"].ToString() : string.Empty,
                        MaDonVi = row["MaDonVi"] != DBNull.Value ? row["MaDonVi"].ToString() : string.Empty,
                        MaNguon = row["MaNguon"] != DBNull.Value ? row["MaNguon"].ToString() : string.Empty,
                        DiemTraHang = row["DiemTraHang"] != DBNull.Value ? row["DiemTraHang"].ToString() : string.Empty,
                        LoaiPhieu = row["LoaiPhieu"] != DBNull.Value ? row["LoaiPhieu"].ToString() : string.Empty,
                        MaKho = row["MaKho"] != DBNull.Value ? row["MaKho"].ToString() : string.Empty,
                        MaVanChuyen = row["MaVanChuyen"] != DBNull.Value ? row["MaVanChuyen"].ToString() : string.Empty,
                        MaPhuongTien = row["MaPhuongTien"] != DBNull.Value ? row["MaPhuongTien"].ToString() : string.Empty,
                        NguoiVanChuyen = row["NguoiVanChuyen"] != DBNull.Value ? row["NguoiVanChuyen"].ToString() : string.Empty,
                        MaPhuongThucBan = row["MaPhuongThucBan"] != DBNull.Value ? row["MaPhuongThucBan"].ToString() : string.Empty,
                        MaPhuongThucXuat = row["MaPhuongThucXuat"] != DBNull.Value ? row["MaPhuongThucXuat"].ToString() : string.Empty,
                        MaKhachHang = row["MaKhachHang"] != DBNull.Value ? row["MaKhachHang"].ToString() : string.Empty,
                        NgayXuat = row["NgayXuat"] != DBNull.Value ? Convert.ToDateTime(row["NgayXuat"]) : (DateTime?)null,
                        GhiChu = row["GhiChu"] != DBNull.Value ? row["GhiChu"].ToString().Length <= 1000 ? row["GhiChu"].ToString() : row["GhiChu"].ToString().Substring(0, 1000) : string.Empty,
                        Niem = row["Niem"] != DBNull.Value ? row["Niem"].ToString().Length <= 300 ? row["Niem"].ToString() : row["Niem"].ToString().Substring(0, 300) : string.Empty,
                        QCI_KG = row["QCI_KG"] != DBNull.Value ? Convert.ToDecimal(row["QCI_KG"]) : (decimal?)null,
                        QCI_NhietDo = row["QCI_NhietDo"] != DBNull.Value ? Convert.ToDecimal(row["QCI_NhietDo"]) : (decimal?)null,
                        NgayHetHieuLuc = row["NgayHetHieuLuc"] != DBNull.Value ? Convert.ToDateTime(row["NgayHetHieuLuc"]) : (DateTime?)null,
                        Number = row["Number"] != DBNull.Value ? Convert.ToInt32(row["Number"]) : 0,
                        Status = row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty,
                        CreateDate = row["CreateDate"] != DBNull.Value ? Convert.ToDateTime(row["CreateDate"]) : (DateTime?)null,
                        UpdateDate = row["UpdateDate"] != DBNull.Value ? Convert.ToDateTime(row["UpdateDate"]) : (DateTime?)null,
                        SoLenhSAP = row["SoLenhSAP"] != DBNull.Value ? row["SoLenhSAP"].ToString() : string.Empty,
                        Client = row["Client"] != DBNull.Value ? row["Client"].ToString() : string.Empty,
                        HTTG = row["HTTG"] != DBNull.Value ? row["HTTG"].ToString() : string.Empty,
                        Approved = row["Approved"] != DBNull.Value ? row["Approved"].ToString() : string.Empty,
                        Date_Approve = row["Date_Approve"] != DBNull.Value ? Convert.ToDateTime(row["Date_Approve"]) : (DateTime?)null,
                        User_Approve = row["User_Approve"] != DBNull.Value ? row["User_Approve"].ToString() : string.Empty,
                        EditApprove = row["EditApprove"] != DBNull.Value ? row["EditApprove"].ToString() : string.Empty,
                        NhaCungCap = row["NhaCungCap"] != DBNull.Value ? row["NhaCungCap"].ToString() : string.Empty,
                        AppDesc = row["AppDesc"] != DBNull.Value ? row["AppDesc"].ToString() : string.Empty,
                        AppN30Date = row["AppN30Date"] != DBNull.Value ? Convert.ToDateTime(row["AppN30Date"]) : (DateTime?)null,
                        AppN30User = row["AppN30User"] != DBNull.Value ? row["AppN30User"].ToString() : string.Empty,
                        SoBienBanMau = row["SoBienBanMau"] != DBNull.Value ? row["SoBienBanMau"].ToString() : string.Empty
                    };
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                return new List<TblBuHeaderTgbx>();
            }
        }

        public List<TblBuDetailTgbx> ConvertToDetail(DataTable dataTable, string headerId)
        {
            try
            {
                List<TblBuDetailTgbx> list = new List<TblBuDetailTgbx>();
                foreach (DataRow row in dataTable.Rows)
                {
                    TblBuDetailTgbx model = new TblBuDetailTgbx
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = headerId,
                        LineID = row["LineID"] != DBNull.Value ? row["LineID"].ToString() : string.Empty,
                        MaLenh = row["MaLenh"] != DBNull.Value ? row["MaLenh"].ToString() : string.Empty,
                        NgayXuat = row["NgayXuat"] != DBNull.Value ? Convert.ToDateTime(row["NgayXuat"]) : (DateTime?)null,
                        SoLenh = row["SoLenh"] != DBNull.Value ? row["SoLenh"].ToString() : string.Empty,
                        TongXuat = row["TongXuat"] != DBNull.Value && !string.IsNullOrEmpty(row["TongXuat"].ToString()) ? Convert.ToDecimal(row["TongXuat"]) : 0,
                        TongDuXuat = row["TongDuXuat"] != DBNull.Value && !string.IsNullOrEmpty(row["TongDuXuat"].ToString()) ? Convert.ToDecimal(row["TongDuXuat"]) : 0,
                        MaHangHoa = row["MaHangHoa"] != DBNull.Value ? row["MaHangHoa"].ToString() : string.Empty,
                        DonViTinh = row["DonViTinh"] != DBNull.Value ? row["DonViTinh"].ToString() : string.Empty,
                        BeXuat = row["BeXuat"] != DBNull.Value ? row["BeXuat"].ToString() : string.Empty,
                        TableID = row["TableID"] != DBNull.Value ? row["TableID"].ToString() : string.Empty,
                        MeterId = row["MeterId"] != DBNull.Value ? row["MeterId"].ToString() : string.Empty,
                        CreateDate = row["CreateDate"] != DBNull.Value ? Convert.ToDateTime(row["CreateDate"]) : (DateTime?)null,
                        UpdateDate = row["UpdateDate"] != DBNull.Value ? Convert.ToDateTime(row["UpdateDate"]) : (DateTime?)null,
                        QCI_KG = row["QCI_KG"] != DBNull.Value ? Convert.ToDecimal(row["QCI_KG"]) : 0,
                        QCI_NhietDo = row["QCI_NhietDo"] != DBNull.Value ? Convert.ToDecimal(row["QCI_NhietDo"]) : 0,
                        QCI_TyTrong = row["QCI_TyTrong"] != DBNull.Value ? Convert.ToDecimal(row["QCI_TyTrong"]) : 0,
                        DonGia = row["DonGia"] != DBNull.Value ? Convert.ToDecimal(row["DonGia"]) : 0,
                        CurrencyKey = row["CurrencyKey"] != DBNull.Value ? row["CurrencyKey"].ToString() : string.Empty,
                        VCF = row["VCF"] != DBNull.Value ? Convert.ToDecimal(row["VCF"]) : 0,
                        WCF = row["WCF"] != DBNull.Value ? Convert.ToDecimal(row["WCF"]) : 0,
                        NhietDo_BQGQ = row["NhietDo_BQGQ"] != DBNull.Value ? Convert.ToDecimal(row["NhietDo_BQGQ"]) : 0,
                        D15_BQGQ = row["D15_BQGQ"] != DBNull.Value ? Convert.ToDecimal(row["D15_BQGQ"]) : 0,
                        KG = row["KG"] != DBNull.Value ? Convert.ToDecimal(row["KG"]) : 0,
                        L15 = row["L15"] != DBNull.Value ? Convert.ToDecimal(row["L15"]) : 0,
                        GiaCty = row["GiaCty"] != DBNull.Value ? Convert.ToDecimal(row["GiaCty"]) : 0,
                        PhiVT = row["PhiVT"] != DBNull.Value ? Convert.ToDecimal(row["PhiVT"]) : 0,
                        TheBVMT = row["TheBVMT"] != DBNull.Value ? Convert.ToDecimal(row["TheBVMT"]) : 0,
                        BatchNum = row["BatchNum"] != DBNull.Value ? row["BatchNum"].ToString() : string.Empty,
                        TongSoTien = row["TongSoTien"] != DBNull.Value ? Convert.ToDecimal(row["TongSoTien"]) : 0,
                        QCI_L15 = row["QCI_L15"] != DBNull.Value ? Convert.ToDecimal(row["QCI_L15"]) : 0,
                        ChietKhau = row["ChietKhau"] != DBNull.Value ? Convert.ToDecimal(row["ChietKhau"]) : 0
                    };
                    list.Add(model);
                }
                return list;
            }
            catch (Exception)
            {
                return new List<TblBuDetailTgbx>();
            }
        }

        public List<TblBuTgbxTicket> ConvertToDetailTicket(DataTable dt, string headerId)
        {
            try
            {
                var list = new List<TblBuTgbxTicket>();

                foreach (DataRow row in dt.Rows)
                {
                    var model = new TblBuTgbxTicket
                    {
                        Id = Guid.NewGuid().ToString(),
                        HeaderId = headerId,
                        MaNgan = row["MaNgan"] != DBNull.Value ? row["MaNgan"].ToString() : string.Empty,
                        MaLenh = row["MaLenh"] != DBNull.Value ? row["MaLenh"].ToString() : string.Empty,
                        NgayXuat = row["NgayXuat"] != DBNull.Value ? Convert.ToDateTime(row["NgayXuat"]) : (DateTime?)null,
                        LineID = row["LineID"] != DBNull.Value ? row["LineID"].ToString() : string.Empty,
                        SoLuongDuXuat = row["SoLuongDuXuat"] != DBNull.Value ? Convert.ToDecimal(row["SoLuongDuXuat"]) : 0,
                        SoLuongThucXuat = row["SoLuongThucXuat"] != DBNull.Value ? Convert.ToDecimal(row["SoLuongThucXuat"]) : 0,
                        ThoiGianDau = row["ThoiGianDau"] != DBNull.Value ? Convert.ToDateTime(row["ThoiGianDau"]) : (DateTime?)null,
                        ThoiGianCuoi = row["ThoiGianCuoi"] != DBNull.Value ? Convert.ToDateTime(row["ThoiGianCuoi"]) : (DateTime?)null,
                        SlLlkebd = row["Sl_llkebd"] != DBNull.Value ? Convert.ToDecimal(row["Sl_llkebd"]) : 0,
                        SlLlkekt = row["Sl_llkekt"] != DBNull.Value ? Convert.ToDecimal(row["Sl_llkekt"]) : 0,
                        HeSoK = row["HeSo_k"] != DBNull.Value ? Convert.ToDecimal(row["HeSo_k"]) : 0,
                        NhietDo = row["NhietDo"] != DBNull.Value ? Convert.ToDecimal(row["NhietDo"]) : 0,
                        TyTrong15 = row["TyTrong_15"] != DBNull.Value ? Convert.ToDecimal(row["TyTrong_15"]) : 0,
                        MaDanXuat = row["MaDanXuat"] != DBNull.Value ? row["MaDanXuat"].ToString() : string.Empty,
                        MaLoi = row["MaLoi"] != DBNull.Value ? row["MaLoi"].ToString() : string.Empty,
                        TrangThai = row["TrangThai"] != DBNull.Value ? row["TrangThai"].ToString() : string.Empty,
                        MaLuuLuongKe = row["MaLuuLuongKe"] != DBNull.Value ? row["MaLuuLuongKe"].ToString() : string.Empty,
                        MaEntry = row["MaEntry"] != DBNull.Value ? Convert.ToDecimal(row["MaEntry"]) : 0,
                        MaLo = row["MaLo"] != DBNull.Value ? Convert.ToDecimal(row["MaLo"]) : 0,
                        GhiChu = row["GhiChu"] != DBNull.Value ? row["GhiChu"].ToString() : string.Empty,
                        Status = row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty,
                        ERate = row["ERate"] != DBNull.Value ? row["ERate"].ToString() : string.Empty,
                        GV = row["GV"] != DBNull.Value ? Convert.ToSingle(row["GV"]) : 0,
                        GST = row["GST"] != DBNull.Value ? Convert.ToSingle(row["GST"]) : 0,
                        GvTotalStart = row["GVTOTAL_START"] != DBNull.Value ? Convert.ToSingle(row["GVTOTAL_START"]) : 0,
                        GvTotalEnd = row["GVTOTAL_END"] != DBNull.Value ? Convert.ToSingle(row["GVTOTAL_END"]) : 0,
                        GstTotalStart = row["GSTTOTAL_START"] != DBNull.Value ? Convert.ToSingle(row["GSTTOTAL_START"]) : 0,
                        GstTotalEnd = row["GSTTOTAL_END"] != DBNull.Value ? Convert.ToSingle(row["GSTTOTAL_END"]) : 0,
                        KF = row["KF"] != DBNull.Value ? Convert.ToSingle(row["KF"]) : 0,
                        KfE = row["KF_E"] != DBNull.Value ? Convert.ToSingle(row["KF_E"]) : 0,
                        TyTrong = row["TY_TRONG"] != DBNull.Value ? Convert.ToSingle(row["TY_TRONG"]) : 0,
                        AvgMf = row["AVG_MF"] != DBNull.Value ? Convert.ToSingle(row["AVG_MF"]) : 0,
                        AvgMfE = row["AVG_MF_E"] != DBNull.Value ? Convert.ToSingle(row["AVG_MF_E"]) : 0,
                        AvgCtl = row["AVG_CTL"] != DBNull.Value ? Convert.ToSingle(row["AVG_CTL"]) : 0,
                        AvgCtlE = row["AVG_CTL_E"] != DBNull.Value ? Convert.ToSingle(row["AVG_CTL_E"]) : 0,
                        AvgCtlBase = row["AVG_CTL_BASE"] != DBNull.Value ? Convert.ToSingle(row["AVG_CTL_BASE"]) : 0,
                        RtdOffset = row["RTD_OFFSET"] != DBNull.Value ? Convert.ToSingle(row["RTD_OFFSET"]) : 0,
                        GvE = row["GV_E"] != DBNull.Value ? Convert.ToSingle(row["GV_E"]) : 0,
                        GstE = row["GST_E"] != DBNull.Value ? Convert.ToSingle(row["GST_E"]) : 0,
                        GvTotalEStart = row["GVTOTAL_E_START"] != DBNull.Value ? Convert.ToSingle(row["GVTOTAL_E_START"]) : 0,
                        GvTotalEEnd = row["GVTOTAL_E_END"] != DBNull.Value ? Convert.ToSingle(row["GVTOTAL_E_END"]) : 0,
                        GstTotalEStart = row["GSTTOTAL_E_START"] != DBNull.Value ? Convert.ToSingle(row["GSTTOTAL_E_START"]) : 0,
                        GstTotalEEnd = row["GSTTOTAL_E_END"] != DBNull.Value ? Convert.ToSingle(row["GSTTOTAL_E_END"]) : 0,
                        GvBase = row["GV_BASE"] != DBNull.Value ? Convert.ToSingle(row["GV_BASE"]) : 0,
                        GstBase = row["GST_BASE"] != DBNull.Value ? Convert.ToSingle(row["GST_BASE"]) : 0,
                        GvTotalBaseStart = row["GVTOTAL_BASE_START"] != DBNull.Value ? Convert.ToSingle(row["GVTOTAL_BASE_START"]) : 0,
                        GvTotalBaseEnd = row["GVTOTAL_BASE_END"] != DBNull.Value ? Convert.ToSingle(row["GVTOTAL_BASE_END"]) : 0,
                        GstTotalBaseStart = row["GSTTOTAL_BASE_START"] != DBNull.Value ? Convert.ToSingle(row["GSTTOTAL_BASE_START"]) : 0,
                        GstTotalBaseEnd = row["GSTTOTAL_BASE_END"] != DBNull.Value ? Convert.ToSingle(row["GSTTOTAL_BASE_END"]) : 0,
                        TyleTte = row["TYLE_TTE"] != DBNull.Value ? Convert.ToSingle(row["TYLE_TTE"]) : 0,
                        VPreset = row["V_PRESET"] != DBNull.Value ? Convert.ToSingle(row["V_PRESET"]) : 0,
                        TylePreset = row["TYLE_PRESET"] != DBNull.Value ? Convert.ToSingle(row["TYLE_PRESET"]) : 0,
                        TyTrongBase = row["TYTRONG_BASE"] != DBNull.Value ? Convert.ToSingle(row["TYTRONG_BASE"]) : 0,
                        TyTrongE = row["TYTRONG_E"] != DBNull.Value ? Convert.ToSingle(row["TYTRONG_E"]) : 0,
                        NgayDky = row["NGAY_DKY"] != DBNull.Value ? Convert.ToDateTime(row["NGAY_DKY"]) : (DateTime?)null,
                        NgayBd = row["NGAY_BD"] != DBNull.Value ? Convert.ToDateTime(row["NGAY_BD"]) : (DateTime?)null,
                        NgayKt = row["NGAY_KT"] != DBNull.Value ? Convert.ToDateTime(row["NGAY_KT"]) : (DateTime?)null,
                        SoCtu = row["SO_CTU"] != DBNull.Value ? row["SO_CTU"].ToString() : string.Empty,
                        MaLenhNum = row["MA_LENH"] != DBNull.Value ? Convert.ToDecimal(row["MA_LENH"]) : 0,
                        CardData1 = row["CARD_DATA"] != DBNull.Value ? row["CARD_DATA"].ToString() : string.Empty,
                        MaNganByte = row["MA_NGAN"] != DBNull.Value ? Convert.ToByte(row["MA_NGAN"]) : (byte?)null,
                        MaHhoa = row["MA_HHOA"] != DBNull.Value ? row["MA_HHOA"].ToString() : string.Empty,
                        MaHong = row["MA_HONG"] != DBNull.Value ? Convert.ToByte(row["MA_HONG"]) : (byte?)null,
                        MaKho = row["MA_KHO"] != DBNull.Value ? row["MA_KHO"].ToString() : string.Empty,
                        NhietDoTb = row["NHIET_DOTB"] != DBNull.Value ? Convert.ToSingle(row["NHIET_DOTB"]) : 0,
                        TrangThaiByte = row["TRANG_THAI"] != DBNull.Value ? Convert.ToByte(row["TRANG_THAI"]) : (byte?)null,
                        SoPtien = row["SO_PTIEN"] != DBNull.Value ? row["SO_PTIEN"].ToString() : string.Empty,
                        LaiXe = row["LAI_XE"] != DBNull.Value ? row["LAI_XE"].ToString() : string.Empty,
                        TyTrongTb = row["TY_TRONGTB"] != DBNull.Value ? Convert.ToSingle(row["TY_TRONGTB"]) : 0,
                        TyTrongTbBase = row["TY_TRONGTB_BASE"] != DBNull.Value ? Convert.ToSingle(row["TY_TRONGTB_BASE"]) : 0,
                        TyTrongTbE = row["TY_TRONGTB_E"] != DBNull.Value ? Convert.ToSingle(row["TY_TRONGTB_E"]) : 0,
                        Mass = row["MASS"] != DBNull.Value ? Convert.ToSingle(row["MASS"]) : 0,
                        MassBase = row["MASS_BASE"] != DBNull.Value ? Convert.ToSingle(row["MASS_BASE"]) : 0,
                        MassE = row["MASS_E"] != DBNull.Value ? Convert.ToSingle(row["MASS_E"]) : 0,
                        MassTotalStart = row["MASSTOTAL_START"] != DBNull.Value ? Convert.ToSingle(row["MASSTOTAL_START"]) : 0,
                        MassTotalEnd = row["MASSTOTAL_END"] != DBNull.Value ? Convert.ToSingle(row["MASSTOTAL_END"]) : 0,
                        MassTotalBaseStart = row["MASSTOTAL_BASE_START"] != DBNull.Value ? Convert.ToSingle(row["MASSTOTAL_BASE_START"]) : 0,
                        MassTotalBaseEnd = row["MASSTOTAL_BASE_END"] != DBNull.Value ? Convert.ToSingle(row["MASSTOTAL_BASE_END"]) : 0,
                        MassTotalEStart = row["MASSTOTAL_E_START"] != DBNull.Value ? Convert.ToSingle(row["MASSTOTAL_E_START"]) : 0,
                        MassTotalEEnd = row["MASSTOTAL_E_END"] != DBNull.Value ? Convert.ToSingle(row["MASSTOTAL_E_END"]) : 0,
                        CreateByName = row["Createby"] != DBNull.Value ? row["Createby"].ToString() : string.Empty,
                        UpdateByName = row["UpdatedBy"] != DBNull.Value ? row["UpdatedBy"].ToString() : string.Empty,
                        CreateDateTime = row["CreateDate"] != DBNull.Value ? Convert.ToDateTime(row["CreateDate"]) : (DateTime?)null,
                        UpdateDateTime = row["UpdateDate"] != DBNull.Value ? Convert.ToDateTime(row["UpdateDate"]) : (DateTime?)null,
                        DungTichNgan = row["DungTichNgan"] != DBNull.Value ? Convert.ToInt32(row["DungTichNgan"]) : 0,
                        TableId = row["TableID"] != DBNull.Value ? row["TableID"].ToString() : string.Empty,
                        MaTuDongHoa = row["MaTuDongHoa"] != DBNull.Value ? row["MaTuDongHoa"].ToString() : string.Empty,
                        RowId = row["Row_id"] != DBNull.Value ? Convert.ToInt32(row["Row_id"]) : 0,
                        PhuongTien = row["PhuongTien"] != DBNull.Value ? row["PhuongTien"].ToString() : string.Empty,
                        RecordStatus = row["Record_Status"] != DBNull.Value ? row["Record_Status"].ToString() : string.Empty,
                        DoCreate = row["DO_CREATE"] != DBNull.Value ? Convert.ToDateTime(row["DO_CREATE"]) : (DateTime?)null,
                        SoTt = row["SO_TT"] != DBNull.Value ? Convert.ToInt32(row["SO_TT"]) : 0,
                        FlagTankLine = row["FlagTankLine"] != DBNull.Value ? row["FlagTankLine"].ToString() : string.Empty,
                        GstTdh = row["GST_TDH"] != DBNull.Value ? Convert.ToDecimal(row["GST_TDH"]) : 0,
                        L15 = row["L15"] != DBNull.Value ? Convert.ToDecimal(row["L15"]) : 0,
                        Kg = row["KG"] != DBNull.Value ? Convert.ToDecimal(row["KG"]) : 0,
                        BqgqNhietDo = row["BQGQ_NhietDo"] != DBNull.Value ? Convert.ToDecimal(row["BQGQ_NhietDo"]) : 0,
                        BqgqD15 = row["BQGQ_D15"] != DBNull.Value ? Convert.ToDecimal(row["BQGQ_D15"]) : 0,
                        Vcf = row["VCF"] != DBNull.Value ? Convert.ToDecimal(row["VCF"]) : 0,
                        Wcf = row["WCF"] != DBNull.Value ? Convert.ToDecimal(row["WCF"]) : 0,
                        CardNum = row["CardNum"] != DBNull.Value ? row["CardNum"].ToString() : string.Empty,
                        CardData2 = row["CardData"] != DBNull.Value ? row["CardData"].ToString() : string.Empty
                    };

                    list.Add(model);
                }

                return list;
            }
            catch (Exception)
            {
                return new List<TblBuTgbxTicket>();
            }
        }



        public async Task<List<string>> AsyncUploadFile(List<IFormFile> files, List<string> filePath)
        {
            try
            {

                var SouceFileimage = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;

                List<string> savedFiles = new List<string>();
                foreach (var file in files)
                {
                    var path = filePath.FirstOrDefault(x => Path.GetFileName(x) == file.FileName);
                    string[] parts = path.Split(Path.DirectorySeparatorChar);
                    string year = parts[^4];
                    string month = parts[^3];
                    string day = parts[^2];
                    var fileaddress = Path.Combine(SouceFileimage, "AttachImageVCS", year, month, day);
                    if (!Directory.Exists(fileaddress))
                    {
                        Directory.CreateDirectory(fileaddress);
                    }
                    if (file.Length > 0)
                    {
                        string filePathSave = Path.Combine(fileaddress, file.FileName);
                        using (var stream = new FileStream(filePathSave, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        savedFiles.Add(filePathSave);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Status = false;
                return null;
            }
        }

        public async Task<List<BaoCaoChiTietXeModel>> BaoCaoXeChiTiet(FilterReport filter)
        {
            try
            {
                var query = _dbContext.TblBuHeader.AsQueryable();

                if (!string.IsNullOrEmpty(filter.WarehouseCode))
                {
                    query = query.Where(x => x.WarehouseCode == filter.WarehouseCode);
                }

                query = query.Where(x => x.TimeCheckout.HasValue && x.TimeCheckout.Value.Date == filter.Time.Date);



                var filtered = await query
                    .Where(x => x.TimeCheckout != null)
                    .ToListAsync();

                // Group theo giờ và tính số lượng các loại xe
                var result = filtered
                    .GroupBy(x => x.TimeCheckout.Value.Hour)
                    .Select(g => new BaoCaoChiTietXeModel
                    {
                        Hour = g.Key,
                        XeVao = g.Count(x => x.StatusVehicle == "02"), // Đếm các xe vào (StatusVehicle = "02")
                        XeRa = g.Count(x => x.StatusVehicle == "04"),  // Đếm các xe ra (StatusVehicle = "04")
                        XeKhongHopLe = g.Count(x => x.StatusProcess == "05") // Đếm các xe không hợp lệ (StatusProcess = "05")
                    })
                    .OrderBy(x => x.Hour)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                return new List<BaoCaoChiTietXeModel>();  // Return an empty list in case of an error
            }
        }
        public async Task<byte[]> ExportExcelBaoCaoXeChiTiet(FilterReport filter)
        {
            try
            {
                var data = await BaoCaoXeChiTiet(filter);

                byte[] fileBytes;

                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Báo cáo xe chi tiết");

                // Font in đậm cho header
                IFont boldFont = workbook.CreateFont();
                boldFont.IsBold = true;

                // Style cho header (border + in đậm + căn giữa)
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.BorderTop = BorderStyle.Thin;
                headerStyle.BorderBottom = BorderStyle.Thin;
                headerStyle.BorderLeft = BorderStyle.Thin;
                headerStyle.BorderRight = BorderStyle.Thin;
                headerStyle.Alignment = HorizontalAlignment.Center;
                headerStyle.VerticalAlignment = VerticalAlignment.Center;
                headerStyle.SetFont(boldFont);

                // Style cho cell thường (có border + căn giữa)
                ICellStyle borderStyle = workbook.CreateCellStyle();
                borderStyle.BorderTop = BorderStyle.Thin;
                borderStyle.BorderBottom = BorderStyle.Thin;
                borderStyle.BorderLeft = BorderStyle.Thin;
                borderStyle.BorderRight = BorderStyle.Thin;
                borderStyle.Alignment = HorizontalAlignment.Center;
                borderStyle.VerticalAlignment = VerticalAlignment.Center;

                // Tạo dòng tiêu đề
                var header = sheet.CreateRow(0);
                string[] titles = { "STT", "Giờ", "Xe vào", "Xe ra", "Xe không hợp lệ" };

                for (int i = 0; i < titles.Length; i++)
                {
                    var cell = header.CreateCell(i);
                    cell.SetCellValue(titles[i]);
                    cell.CellStyle = headerStyle;
                }

                // Dữ liệu
                int rowIndex = 1;
                foreach (var item in data)
                {
                    var row = sheet.CreateRow(rowIndex);

                    var cell0 = row.CreateCell(0);
                    cell0.SetCellValue(rowIndex);
                    cell0.CellStyle = borderStyle;

                    var cell1 = row.CreateCell(1);
                    cell1.SetCellValue(item.Hour);
                    cell1.CellStyle = borderStyle;

                    var cell2 = row.CreateCell(2);
                    cell2.SetCellValue(item.XeVao);
                    cell2.CellStyle = borderStyle;

                    var cell3 = row.CreateCell(3);
                    cell3.SetCellValue(item.XeRa);
                    cell3.CellStyle = borderStyle;

                    var cell4 = row.CreateCell(4);
                    cell4.SetCellValue(item.XeKhongHopLe);
                    cell4.CellStyle = borderStyle;

                    rowIndex++;
                }

                // Auto size các cột
                for (int i = 0; i < titles.Length; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms, true);
                    fileBytes = ms.ToArray();
                }

                return await Task.FromResult(fileBytes);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }


    }
}
