using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.CORE;
using DMS.CORE.Entities.BU;


namespace DMS.BUSINESS.Services.BU
{
    public interface IOrderService : IGenericService<TblBuOrder, OrderDto>
    {
        Task<IList<OrderDto>> GetAll(BaseMdFilter filter);
        //Task<byte[]> Export(BaseMdFilter filter);
    }
    public class OrderService(AppDbContext dbContext, IMapper mapper) : GenericService<TblBuOrder, OrderDto>(dbContext, mapper), IOrderService
    {
         public async Task<IList<OrderDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblBuOrders.AsQueryable();
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                return await base.GetAllMd(query, filter);
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
