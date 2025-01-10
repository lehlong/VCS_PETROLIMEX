using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace DMS.BUSINESS.Services.BU
{
    public interface IOrderService : IGenericService<TblBuOrder, OrderDto>
    {
        Task<List<TblBuOrder>> GetOrder(BaseFilter filter);
        Task<List<TblBuOrder>> UpdateOrderCall(string orderId, BaseFilter filter);
        Task<List<TblBuOrder>> UpdateOrderCome(string orderId, BaseFilter filter); 
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
                    .OrderBy(x => x.Stt)
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

        public async Task<List<TblBuOrder>> UpdateOrderCall(string orderId, BaseFilter filter)
        {
            try
            {
                // Lấy danh sách orders theo điều kiện
                var orders = await _dbContext.TblBuOrders
                    .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                               x.WarehouseCode == filter.WarehouseCode &&
                               x.CompanyCode == filter.OrgCode)
                    .OrderBy(x => x.Stt)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    order.IsCall = false;
                }

                var selectedOrder = orders.FirstOrDefault(x => x.Id == orderId);
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

        public async Task<List<TblBuOrder>> UpdateOrderCome(string orderId, BaseFilter filter)
        {
            try
            {
                var order = await _dbContext.TblBuOrders
                    .FirstOrDefaultAsync(x => x.Id == orderId &&
                                            x.CreateDate.Value.Date == DateTime.Now.Date &&
                                            x.WarehouseCode == filter.WarehouseCode &&
                                            x.CompanyCode == filter.OrgCode);

                if (order != null)
                {
                    order.IsCome = true;
                    await _dbContext.SaveChangesAsync();
                    var updatedOrders = await _dbContext.TblBuOrders
                        .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                                   x.WarehouseCode == filter.WarehouseCode &&
                                   x.CompanyCode == filter.OrgCode)
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

                var maxStt = await _dbContext.TblBuOrders
                    .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                               x.WarehouseCode == orderDto.WarehouseCode &&
                               x.CompanyCode == orderDto.CompanyCode)
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
    }
}
