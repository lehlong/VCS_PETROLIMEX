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
    }
    public class OrderService(AppDbContext dbContext, IMapper mapper) : GenericService<TblBuOrder, OrderDto>(dbContext, mapper), IOrderService
    {
        public async Task<List<TblBuOrder>> GetOrder(BaseFilter filter)
        {
            try
            {
                var data = await _dbContext.TblBuOrders.Where(x => x.CreateDate.Value.Date == DateTime.Now.Date &&
                x.WarehouseCode == filter.WarehouseCode &&
                x.CompanyCode == filter.OrgCode).OrderBy(x => x.Stt).ToListAsync();
                return data;
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
