using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.CORE.Entities.AD;
using DMS.CORE;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.BUSINESS.Dtos.AD;

namespace DMS.BUSINESS.Services.AD
{
    public interface IConfigDisplayService : IGenericService<TblAdConfigDisplay, ConfigDisplayDto>
    {
        Task<IList<ConfigDisplayDto>> GetAll(BaseMdFilter filter);
        Task<byte[]> Export(BaseMdFilter filter);

    }
    public class ConfigDisplayService(AppDbContext dbContext, IMapper mapper) : GenericService<TblAdConfigDisplay, ConfigDisplayDto>(dbContext, mapper), IConfigDisplayService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdConfigDisplay.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.OrgCode))
                {
                    query = query.Where(x => x.OrgCode == filter.OrgCode);
                }
                if (!string.IsNullOrWhiteSpace(filter.WarehouseCode))
                {
                    query = query.Where(x => x.WarehouseCode == filter.WarehouseCode);
                }
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                return await Paging(query, filter);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<IList<ConfigDisplayDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdConfigDisplay.AsQueryable();
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
        public async Task<byte[]> Export(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdConfigDisplay.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x => x.Name.Contains(filter.KeyWord));
                }
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                var data = await base.GetAllMd(query, filter);
                int i = 1;
                return await ExportExtension.ExportToExcel(data);
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
