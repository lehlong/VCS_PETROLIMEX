using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.MD
{
    public interface ISequenceService : IGenericService<TblMdSequence, SequenceDto>
    {
        Task<IList<SequenceDto>> GetAll(BaseMdFilter filter);
        Task<byte[]> Export(BaseMdFilter filter);
    }

    public class SequenceService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdSequence, SequenceDto>(dbContext, mapper), ISequenceService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.tblMdSequence.AsQueryable();
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

        public async Task<IList<SequenceDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.tblMdSequence.AsQueryable();
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
                var query = _dbContext.tblMdSequence.AsQueryable();
                //if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                //{
                //    query = query.Where(x => x.Name.Contains(filter.KeyWord));
                //}
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                var data = await base.GetAllMd(query, filter);
                int i = 1;
                data.ForEach(x =>
                {
                    x.OrdinalNumber = i++;
                });
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
