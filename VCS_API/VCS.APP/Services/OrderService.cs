using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Services.HUB;
using DMS.CORE;
using DMS.CORE.Entities.BU;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VCS.APP.Services
{
    public interface IWOrderService : IGenericService<TblBuOrder, OrderDto>
    {
      //  Task<List<TblBuOrder>> GetOrder(BaseFilter filter);
     //   Task<List<TblBuOrder>> UpdateOrderCall(OrderUpdateDto orderDto);
      //  Task<List<TblBuOrder>> UpdateOrderCome(OrderUpdateDto orderDto);
    }

    public class WOrderService : GenericService<TblBuOrder, OrderDto>, IWOrderService
    {
        private readonly HubConnection _hubConnection;
       

        public WOrderService(AppDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://sso.d2s.com.vn:1000/order")
                .WithAutomaticReconnect()
                .Build();

            InitializeHubConnection();
        }

        private async Task InitializeHubConnection()
        {
            try
            {
                await _hubConnection.StartAsync();
               
            }
            catch (Exception ex)
            {
                
            }
        }

        //public async Task<List<TblBuOrder>> GetOrder(BaseFilter filter)
        //{
        //    try
        //    {
        //        var data = await _dbContext.TblBuOrders
        //            .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
        //                       x.WarehouseCode == filter.WarehouseCode &&
        //                       x.CompanyCode == filter.OrgCode)
        //            .OrderBy(x => x.Stt)
        //            .ToListAsync();

        //        if (_hubConnection.State == HubConnectionState.Connected)
        //        {
        //            await _hubConnection.InvokeAsync(SignalRMethod.ORDER_LIST_CHANGED.ToString(), data);
                
        //        }

        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return new List<TblBuOrder>();
        //    }
        //}

        //public async Task<List<TblBuOrder>> UpdateOrderCall(OrderUpdateDto orderDto)
        //{
        //    try
        //    {
        //        var orders = await _dbContext.TblBuOrders
        //            .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
        //                       x.WarehouseCode == orderDto.WarehouseCode &&
        //                       x.CompanyCode == orderDto.CompanyCode)
        //            .OrderBy(x => x.Stt)
        //            .ToListAsync();

        //        foreach (var order in orders)
        //        {
        //            order.IsCall = false;
        //        }

        //        var selectedOrder = orders.FirstOrDefault(x => x.Id == orderDto.Id);
        //        if (selectedOrder != null)
        //        {
        //            selectedOrder.IsCall = true;
        //        }

        //        await _dbContext.SaveChangesAsync();

        //        if (_hubConnection.State == HubConnectionState.Connected)
        //        {
        //            await _hubConnection.InvokeAsync(SignalRMethod.ORDER_CALL.ToString(), orders);
        //           // _logger.LogInformation("Sent updated order call via SignalR.");
        //        }

        //        return orders;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<TblBuOrder>();
        //    }
        //}

        //public async Task<List<TblBuOrder>> UpdateOrderCome(OrderUpdateDto orderDto)
        //{
        //    try
        //    {
        //        var order = await _dbContext.TblBuOrders
        //            .FirstOrDefaultAsync(x => x.Id == orderDto.Id &&
        //                                      x.CreateDate.Value.Date == DateTime.Now.Date &&
        //                                      x.WarehouseCode == orderDto.WarehouseCode &&
        //                                      x.CompanyCode == orderDto.CompanyCode);

        //        if (order != null)
        //        {
        //            order.IsCome = orderDto.IsCome;
        //            order.IsDone = orderDto.IsDone;
        //            await _dbContext.SaveChangesAsync();

        //            var updatedOrders = await _dbContext.TblBuOrders
        //                .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
        //                           x.WarehouseCode == orderDto.WarehouseCode &&
        //                           x.CompanyCode == orderDto.CompanyCode)
        //                .OrderBy(x => x.Stt)
        //                .ToListAsync();

        //            if (_hubConnection.State == HubConnectionState.Connected)
        //            {
        //                await _hubConnection.InvokeAsync(SignalRMethod.ORDER_COME.ToString(), updatedOrders);
        //            }

        //            return updatedOrders;
        //        }

        //        return new List<TblBuOrder>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<TblBuOrder>();
        //    }
        //}

        public override async Task<OrderDto> Add(IDto dto)
        {
            try
            {
                var orderDto = dto as OrderDto;
                if (orderDto == null)
                {
                    return null;
                }

                var maxOrder = await _dbContext.TblBuOrders
                    .Where(q => q.CreateDate.Value.Date == DateTime.Now.Date &&
                               q.WarehouseCode == orderDto.WarehouseCode &&
                               q.CompanyCode == orderDto.CompanyCode)
                    .MaxAsync(x => (int?)x.Order) ?? 0;

                orderDto.Order = (maxOrder + 1).ToString();

                var maxStt = await _dbContext.TblBuOrders
                    .Where(q => q.CreateDate.Value.Date == DateTime.Now.Date &&
                               q.WarehouseCode == orderDto.WarehouseCode &&
                               q.CompanyCode == orderDto.CompanyCode)
                    .MaxAsync(x => (int?)x.Stt) ?? 0;

                orderDto.Stt = maxStt + 1;
                orderDto.IsCall = false;
                orderDto.IsCome = false; 

                var result = await base.Add(dto);

                if (result != null)
                {
                    var updatedOrders = await _dbContext.TblBuOrders
                        .Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                                   x.WarehouseCode == orderDto.WarehouseCode &&
                                   x.CompanyCode == orderDto.CompanyCode)
                        .OrderBy(x => x.Stt)
                        .ToListAsync();

                    if (_hubConnection.State == HubConnectionState.Connected)
                    {
                        await _hubConnection.InvokeAsync(SignalRMethod.ORDER_LIST_CHANGED.ToString(), updatedOrders);
               
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
