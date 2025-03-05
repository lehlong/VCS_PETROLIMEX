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
        Task<bool> UpdateOrder(string headerId);
        Task<TicketModel> GetTicket(string headerId);
        List<TblBuHeaderTgbx> ConvertToHeader(DataTable dataTable, string headerId);
        List<TblBuDetailTgbx> ConvertToDetail(DataTable dataTable, string headerId);
        Task<List<string>> AsyncUploadFile(List<IFormFile> files, List<string> filePath);
        Task UpdateStatus(TblBuHeader header);
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
                var w = _dbContext.TblMdWarehouse.Find(i.WarehouseCode);
                DataTable tableData = new DataTable();
                var queryTest = $"SELECT * FROM tblLenhXuatE5 WHERE STATUS = '3' AND SoLenh = '2061976967'";
                var query = $"SELECT * FROM tblLenhXuatE5 WHERE STATUS = '3' AND MaPhuongTien = '{i.VehicleCode}' AND NgayXuat = '{DateTime.Now.ToString("yyyy-MM-dd")}'";

                using (SqlConnection con = new SqlConnection(w.Tgbx))
                {
                    SqlCommand cmd = new SqlCommand(queryTest, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    try
                    {
                        adapter.Fill(tableData);
                    }
                    catch (Exception ex)
                    {
                        this.Exception = ex;
                    }
                }
                if (tableData.Rows.Count > 0 || i.IsPrint == true)
                {
                    if (i.IsPrint == null || i.IsPrint == false)
                    {
                        var lstDetail = new List<TblBuDetailTgbx>();
                        var h = ConvertToHeader(tableData, i.Id);
                        foreach (var _h in h)
                        {
                            DataTable tblDetail = new DataTable();
                            using (SqlConnection con = new SqlConnection(w.Tgbx))
                            {
                                SqlCommand cmd = new SqlCommand($"SELECT * FROM tblLenhXuat_HangHoaE5 WHERE SoLenh = '{_h.SoLenh}'", con);
                                cmd.CommandType = CommandType.Text;
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                try
                                {
                                    adapter.Fill(tblDetail);
                                    lstDetail.AddRange(ConvertToDetail(tblDetail, i.Id));
                                }
                                catch (Exception ex)
                                {
                                    this.Exception = ex;
                                }
                            }
                        }
                        i.IsPrint = true;
                        _dbContext.TblBuHeaderTgbx.AddRange(h);
                        _dbContext.TblBuDetailTgbx.AddRange(lstDetail);
                        _dbContext.SaveChanges();
                    }
                    _dbContext.TblBuHeader.Update(i);
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    i.StatusProcess = "02";
                    i.NoteIn = "Phương tiện chưa có ticket";
                    _dbContext.TblBuHeader.Update(i);
                    _dbContext.SaveChanges();
                    return false;
                }
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
                _dbContext.TblBuHeader.Update(header);

                var lstPumpThroat = await _dbContext.TblMdPumpThroat.Where(x => x.OrgCode == header.CompanyCode && x.WarehouseCode == header.WarehouseCode).ToListAsync();

                var lstDetail = await _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == headerId).ToListAsync();
                foreach(var i in lstDetail)
                {
                    var materialCode = i.MaHangHoa.Trim() == "0201004" ? "00000000000" + i.MaHangHoa : "000000000000" + i.MaHangHoa;
                    var o = lstPumpThroat.Where(x => x.GoodsCode == materialCode).OrderBy(x => x.OrderVehicle).FirstOrDefault();
                    o.OrderVehicle = o.OrderVehicle + "," + header.VehicleCode;
                    _dbContext.TblMdPumpThroat.Update(o);

                    i.OrderName = _dbContext.TblMdPumpRig.Find(o.PumpRigCode)?.Name + ", " + o.Name;
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
                var d = new TicketModel
                {
                    CompanyName = _dbContext.tblAdOrganize.Find(i.CompanyCode).Name,
                    DateTime = $"Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}",
                    Vehicle = i.VehicleCode,
                    DriverName = tgbx?.NguoiVanChuyen,
                    PtBan = tgbx?.MaPhuongThucBan,
                    CustmerName = tgbx?.MaKhachHang,
                    ChuyenVt = tgbx?.MaTuyenDuong,
                };
                d.Detail = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == headerId).OrderBy(x => x.SoLenh).ThenBy(x => x.MaHangHoa).ToList();
                foreach (var _d in d.Detail)
                {
                    var gCode = "000000000000" + _d.MaHangHoa;
                    _d.MaHangHoa = _dbContext.TblMdGoods.Find(gCode)?.Name;
                }
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
                        MaLenh = row["MaLenh"].ToString(),
                        NgayXuat = Convert.ToDateTime(row["NgayXuat"]),
                        SoLenh = row["SoLenh"].ToString(),
                        MaDonVi = row["MaDonVi"].ToString(),
                        MaNguon = row["MaNguon"].ToString(),
                        MaKho = row["MaKho"].ToString(),
                        MaVanChuyen = row["MaVanChuyen"].ToString(),
                        MaPhuongTien = row["MaPhuongTien"].ToString(),
                        NguoiVanChuyen = row["NguoiVanChuyen"].ToString(),
                        MaPhuongThucBan = row["MaPhuongThucBan"].ToString(),
                        MaPhuongThucXuat = row["MaPhuongThucXuat"].ToString(),
                        MaKhachHang = row["MaKhachHang"].ToString(),
                        LoaiPhieu = row["LoaiPhieu"].ToString(),
                        Niem = row["Niem"].ToString(),
                        LuongGiamDinh = string.IsNullOrEmpty(row["LuongGiamDinh"].ToString()) ? 0 : Convert.ToDecimal(row["LuongGiamDinh"].ToString()),
                        NhietDoTaiTau = string.IsNullOrEmpty(row["NhietDoTaiTau"].ToString()) ? 0 : Convert.ToDecimal(row["NhietDoTaiTau"].ToString()),
                        GhiChu = row["GhiChu"].ToString(),
                        NgayHieuLuc = row["NgayHieuLuc"] != DBNull.Value ? Convert.ToDateTime(row["NgayHieuLuc"]) : null,
                        Status = row["Status"].ToString(),
                        Number = row["Number"] != DBNull.Value ? Convert.ToInt32(row["Number"]) : null,
                        SoLenhSAP = row["SoLenhSAP"].ToString(),
                        Client = row["Client"].ToString(),
                        HTTG = row["HTTG"].ToString(),
                        Approved = row["Approved"].ToString(),
                        User_Approve = row["User_Approve"].ToString(),
                        Date_Approve = row["Date_Approve"] != DBNull.Value ? Convert.ToDateTime(row["Date_Approve"]) : null,
                        EditApprove = row["EditApprove"].ToString(),
                        NhaCungCap = row["NhaCungCap"].ToString(),
                        AppDesc = row["AppDesc"].ToString(),
                        AppN30 = row["AppN30"].ToString(),
                        AppN30Date = row["AppN30Date"] != DBNull.Value ? Convert.ToDateTime(row["AppN30Date"]) : null,
                        AppN30User = row["AppN30User"].ToString(),
                        QCI_KG = string.IsNullOrEmpty(row["QCI_KG"].ToString()) ? 0 : Convert.ToDecimal(row["QCI_KG"].ToString()),
                        QCI_NhietDo = string.IsNullOrEmpty(row["QCI_NhietDo"].ToString()) ? 0 : Convert.ToDecimal(row["QCI_NhietDo"].ToString()),
                        Slog = row["Slog"].ToString(),
                        NgayHetHieuLuc = row["NgayHetHieuLuc"] != DBNull.Value ? Convert.ToDateTime(row["NgayHetHieuLuc"]) : null,
                        NgayTickKe = row["NgayTickKe"] != DBNull.Value ? Convert.ToDateTime(row["NgayTickKe"]) : null,
                        STO = row["STO"].ToString(),
                        NguoiDaiDien = row["NguoiDaiDien"].ToString(),
                        DonViCungCapVanTai = row["DonViCungCapVanTai"].ToString(),
                        UserTickKe = row["UserTickKe"].ToString(),
                        DiemTraHang = row["DiemTraHang"].ToString(),
                        Tax = row["Tax"].ToString(),
                        PaymentMethod = row["PaymentMethod"].ToString(),
                        Term = row["Term"].ToString(),
                        MaKhoNhap = row["MaKhoNhap"].ToString(),
                        SoHopDong = row["SoHopDong"].ToString(),
                        NgayHopDong = row["NgayHopDong"] != DBNull.Value ? Convert.ToDateTime(row["NgayHopDong"]) : null,
                        TyGia = string.IsNullOrEmpty(row["TyGia"].ToString()) ? 0 : Convert.ToDecimal(row["TyGia"].ToString()),
                        SoTKQNhap = row["SoTKQNhap"].ToString(),
                        SoTKQXuat = row["SoTKQXuat"].ToString(),
                        SelfShipping = row["SelfShipping"].ToString(),
                        PriceGroup = row["PriceGroup"].ToString(),
                        Inco1 = row["Inco1"].ToString(),
                        Inco2 = row["Inco2"].ToString(),
                        SoPXK = row["SoPXK"].ToString(),
                        NgayPXK = row["NgayPXK"] != DBNull.Value ? Convert.ToDateTime(row["NgayPXK"]) : null,
                        MaTuyenDuong = row["MaTuyenDuong"].ToString(),
                        Xuathanggui = row["Xuathanggui"].ToString(),
                        So_TKTN = row["So_TKTN"].ToString(),
                        So_TKTX = row["So_TKTX"].ToString(),
                        Ngay_TKTX = row["Ngay_TKTX"] != DBNull.Value ? Convert.ToDateTime(row["Ngay_TKTX"]) : null,
                        DischargePoint = row["DischargePoint"].ToString(),
                        DesDischargePoint = row["DesDischargePoint"].ToString(),
                        BSART = row["BSART"].ToString(),
                        BWART = row["BWART"].ToString(),
                        VTWEG = row["VTWEG"].ToString(),
                        TenKhoNhap = row["TenKhoNhap"].ToString(),
                        Xitec_Option = row["Xitec_Option"].ToString(),
                        SoLenhGoc = row["SoLenhGoc"].ToString(),
                        Dem = row["Dem"].ToString(),
                        DO1_MaNguon = row["DO1_MaNguon"].ToString(),
                        HDChuyen = row["HDChuyen"].ToString(),
                        BatchNum = row["BatchNum"].ToString(),
                        PriceGroupDO1 = row["PriceGroupDO1"].ToString(),
                        SOType = row["SOType"].ToString(),
                        PrcingDate = row["PrcingDate"] != DBNull.Value ? Convert.ToDateTime(row["PrcingDate"]) : null,
                        POType = row["POType"].ToString(),
                        FromSoLenh = row["FromSoLenh"].ToString(),
                        PTXuat_ID = row["PTXuat_ID"].ToString(),
                        PTIEN = row["PTIEN"].ToString(),
                        SCHUVEN = row["SCHUVEN"].ToString(),
                        DO1_SoLenh = row["DO1_SoLenh"].ToString(),
                        DO1_MaKhach = row["DO1_MaKhach"].ToString(),
                        CardNum = row["CardNum"].ToString(),
                        CardData = row["CardData"].ToString(),
                        SoBienBanMau = row["SoBienBanMau"].ToString()
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
                        LineID = row["LineID"].ToString(),
                        MaLenh = row["MaLenh"].ToString(),
                        NgayXuat = row["NgayXuat"] != DBNull.Value ? Convert.ToDateTime(row["NgayXuat"]) : null,
                        SoLenh = row["SoLenh"].ToString(),
                        TongXuat = string.IsNullOrEmpty(row["TongXuat"].ToString()) ? 0 : Convert.ToDecimal(row["TongXuat"].ToString()),
                        TongDuXuat = string.IsNullOrEmpty(row["TongDuXuat"].ToString()) ? 0 : Convert.ToDecimal(row["TongDuXuat"].ToString()),
                        MaHangHoa = row["MaHangHoa"].ToString(),
                        DonViTinh = row["DonViTinh"].ToString(),
                        BeXuat = row["BeXuat"].ToString(),
                        TableID = row["TableID"].ToString(),
                        MeterId = row["MeterId"].ToString(),
                        QCI_KG = string.IsNullOrEmpty(row["QCI_KG"].ToString()) ? 0 : Convert.ToDecimal(row["QCI_KG"].ToString()),
                        QCI_NhietDo = string.IsNullOrEmpty(row["QCI_NhietDo"].ToString()) ? 0 : Convert.ToDecimal(row["QCI_NhietDo"].ToString()),
                        QCI_TyTrong = string.IsNullOrEmpty(row["QCI_TyTrong"].ToString()) ? 0 : Convert.ToDecimal(row["QCI_TyTrong"].ToString()),
                        DonGia = string.IsNullOrEmpty(row["DonGia"].ToString()) ? 0 : Convert.ToDecimal(row["DonGia"].ToString()),
                        CurrencyKey = row["CurrencyKey"].ToString(),
                        VCF = string.IsNullOrEmpty(row["VCF"].ToString()) ? 0 : Convert.ToDecimal(row["VCF"].ToString()),
                        WCF = string.IsNullOrEmpty(row["WCF"].ToString()) ? 0 : Convert.ToDecimal(row["WCF"].ToString()),
                        NhietDo_BQGQ = string.IsNullOrEmpty(row["NhietDo_BQGQ"].ToString()) ? 0 : Convert.ToDecimal(row["NhietDo_BQGQ"].ToString()),
                        D15_BQGQ = string.IsNullOrEmpty(row["D15_BQGQ"].ToString()) ? 0 : Convert.ToDecimal(row["D15_BQGQ"].ToString()),
                        KG = string.IsNullOrEmpty(row["KG"].ToString()) ? 0 : Convert.ToDecimal(row["KG"].ToString()),
                        L15 = string.IsNullOrEmpty(row["L15"].ToString()) ? 0 : Convert.ToDecimal(row["L15"].ToString()),
                        GiaCty = string.IsNullOrEmpty(row["GiaCty"].ToString()) ? 0 : Convert.ToDecimal(row["GiaCty"].ToString()),
                        PhiVT = string.IsNullOrEmpty(row["PhiVT"].ToString()) ? 0 : Convert.ToDecimal(row["PhiVT"].ToString()),
                        TheBVMT = string.IsNullOrEmpty(row["TheBVMT"].ToString()) ? 0 : Convert.ToDecimal(row["TheBVMT"].ToString()),
                        BatchNum = row["BatchNum"].ToString(),
                        TongSoTien = string.IsNullOrEmpty(row["TongSoTien"].ToString()) ? 0 : Convert.ToDecimal(row["TongSoTien"].ToString()),
                        QCI_L15 = string.IsNullOrEmpty(row["QCI_L15"].ToString()) ? 0 : Convert.ToDecimal(row["QCI_L15"].ToString()),
                        ChietKhau = string.IsNullOrEmpty(row["ChietKhau"].ToString()) ? 0 : Convert.ToDecimal(row["ChietKhau"].ToString())
                    };
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                return new List<TblBuDetailTgbx>();
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

       
    }
}
