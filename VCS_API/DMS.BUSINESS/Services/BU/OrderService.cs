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
using DMS.CORE;
using DMS.CORE.Entities.BU;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;


namespace DMS.BUSINESS.Services.BU
{
    public interface IOrderService : IGenericService<TblBuOrder, OrderDto>
    {
        Task<IList<OrderDto>> GetAll(BaseMdFilter filter);
        //Task<byte[]> Export(BaseMdFilter filter);
    }
    public class OrderService : GenericService<TblBuOrder, OrderDto>, IOrderService
    {
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderService(
            AppDbContext dbContext,
            IMapper mapper,
            IHubContext<OrderHub> hubContext,
            IHttpContextAccessor contextAccessor)
            : base(dbContext, mapper)
        {
            _hubContext = hubContext;
            _contextAccessor = contextAccessor;
        }
        private string CurrentUser
        {
            get
            {
                return _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            }
        }
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
