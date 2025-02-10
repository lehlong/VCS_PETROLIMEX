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
using DMS.CORE;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DMS.CORE.Migrations;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace DMS.BUSINESS.Services.BU
{
    public interface IOrderService : IGenericService<TblBuOrder, OrderDto>
    {
        Task<List<TblBuOrder>> GetOrder(BaseFilter filter);
        Task<List<TblBuOrder>> UpdateOrderCall(OrderUpdateDto orderDto);
        Task<List<TblBuOrder>> UpdateOrderCome(OrderUpdateDto orderDto);
        Task Order(OrderDto orderDto);
        Task<bool> CheckTicket(string headerId);
    }
    public class OrderService : GenericService<TblBuOrder, OrderDto>, IOrderService
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderService(AppDbContext dbContext, IMapper mapper, IHubContext<OrderHub> hubContext)
            : base(dbContext, mapper)
        {
            _hubContext = hubContext;
        }

        public async Task<List<TblBuOrder>> GetOrder(BaseFilter filter)
        {
            try
            {
                var data = await _dbContext.TblBuOrders
                    .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                               x.WarehouseCode == filter.WarehouseCode &&
                               x.CompanyCode == filter.OrgCode)
                    .OrderBy(x =>x.IsDone).ThenBy(x => x.Stt)
                    .ToListAsync();

                await _hubContext.Clients.All.SendAsync(SignalRMethod.ORDER_LIST_CHANGED.ToString(), data);

                return data;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
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
                switch (w.Code)
                {
                    #region Kho Bến Thuỷ
                    case "2810-BT":
                        
                        using (SqlConnection con = new SqlConnection(w.Tdh))
                        {
                            SqlCommand cmd = new SqlCommand($"" +
                                $"SELECT * FROM tblLenhXuatChiTiet " +
                                $"WHERE NgayXuat='{DateTime.Now.ToString("yyyy/MM/dd")}'" +
                                $"AND MaPhuongTien ='{i.VehicleCode}' " +
                                $"AND ISNULL(ThoiGianDau, '3000-01-01') >= {i.CreateDate.Value.ToString("yyyy/MM/dd")}", con);
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
                        break;
                    #endregion
                    #region Kho Nghi Hương
                    case "2810-NH":
                        using (SqlConnection con = new SqlConnection(w.Tdh))
                        {
                            SqlCommand cmd = new SqlCommand($"SELECT 1 a FROM BX_BangMaLenh " +
                                $"WHERE Time_tao_lenh = {DateTime.Now.ToString("yyyy/MM/dd")} " +
                                $"AND SO_PTIEN = {i.VehicleCode} " +
                                $"AND ISNULL(Time_bat_dau_lenh, '3000-01-01') >= {i.CreateDate.Value.ToString("yyyy/MM/dd")} " +
                                $"UNION " +
                                $"SELECT 1 a FROM Lenh_GH WHERE NGAY_DKY = {DateTime.Now.ToString("yyyy/MM/dd")} AND SO_PTIEN = {i.VehicleCode}", con);
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
                        break;
                    #endregion
                    #region Các kho còn lại
                    default:
                        break;
                    #endregion
                }
                return tableData.Rows.Count > 0 ? true : false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
