using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Filter.BU;
using DMS.CORE;
using DMS.CORE.Entities.BU;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.BU
{
    public interface IHeaderService : IGenericService<TblBuHeader, HeaderDto>
    {
        Task<HistoryDto> GetHistoryDetail(string headerId);
        Task<PagedResponseDto> Search(HeaderFilter filter);
    }
    public class HeaderService(AppDbContext dbContext, IMapper mapper) : GenericService<TblBuHeader, HeaderDto>(dbContext, mapper), IHeaderService
    {
        public async Task<PagedResponseDto> Search(HeaderFilter filter)
        {
            try
            {
                var query = _dbContext.TblBuHeader.AsQueryable();

                // Tìm theo tên tài xế
                if (!string.IsNullOrWhiteSpace(filter.VehicleName))
                {
                    query = query.Where(x => x.VehicleName.Contains(filter.VehicleName));
                }

                // Tìm theo biển số xe
                if (!string.IsNullOrWhiteSpace(filter.VehicleCode))
                {
                    query = query.Where(x => x.VehicleCode.Contains(filter.VehicleCode));
                }

                // Tìm theo khoảng thời gian
                if (filter.FromDate.HasValue)
                {
                    query = query.Where(x => x.CreateDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(x => x.TimeCheckout <= filter.ToDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                    x.VehicleCode.Contains(filter.KeyWord) || x.Id.ToString().Contains(filter.KeyWord));
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

        public override async Task<PagedResponseDto> Search(BaseFilter baseFilter)
        {
            if (baseFilter is HeaderFilter filter)
            {
                return await Search(filter);
            }
            return await base.Search(baseFilter);
        }
        public async Task<HistoryDto> GetHistoryDetail(string headerId)
        {
            try
            {
                var header = await (from h in _dbContext.TblBuHeader
                                    join w in _dbContext.TblMdWarehouse on h.WarehouseCode equals w.Code into warehouseGroup
                                    from warehouse in warehouseGroup.DefaultIfEmpty()
                                    where h.Id == headerId
                                    select new HistoryDto
                                    {
                                        VehicleCode = h.VehicleCode,
                                        VehicleName = h.VehicleName,
                                        TimeCheckOut = h.TimeCheckout,
                                        WarehouseName = warehouse.Name,
                                        CreateDate = h.CreateDate,
                                        StatusVehicle = h.StatusVehicle,
                                        StatusProcess = h.StatusProcess,
                                        NoteIn = h.NoteIn,
                                        NoteOut = h.NoteOut,
                                        Stt = h.Stt.ToString()
                                    }).FirstOrDefaultAsync();

                if (header == null)
                {
                    Status = false;
                    MessageObject.MessageDetail = "Không tìm thấy thông tin phiếu";
                    return null;
                }

                header.ImagesIn = await _dbContext.TblBuImage
                    .Where(x => x.HeaderId == headerId && x.InOut == "in")
                    .Select(x => x.Path)
                    .ToListAsync();

                header.ImagesOut = await _dbContext.TblBuImage
                    .Where(x => x.HeaderId == headerId && x.InOut == "out")
                    .Select(x => x.Path)
                    .ToListAsync();

                header.DetailDOs = await (from d in _dbContext.TblBuDetailDO
                                          join m in _dbContext.TblBuDetailMaterial on d.Id equals m.HeaderId
                                          join g in _dbContext.TblMdGoods on m.MaterialCode equals g.Code into goodsGroup
                                          from goods in goodsGroup.DefaultIfEmpty()
                                          where d.HeaderId == headerId
                                          group new { d, m, goods } by d.Do1Sap into g
                                          select new DetailDto
                                          {
                                              Do1Sap = g.Key,
                                              Materials = g.Select(x => new MaterialDto
                                              {
                                                  MaterialCode = x.m.MaterialCode,
                                                  Quantity = x.m.Quantity,
                                                  UnitCode = x.m.UnitCode,
                                                  MaterialName = x.goods != null ? x.goods.Name : null
                                              }).ToList()
                                          }).ToListAsync();

                header.DetailTgbx = await (from t in _dbContext.TblBuDetailTgbx
                                           join g in _dbContext.TblMdGoods on ("00000000000" + t.MaHangHoa) equals g.Code into goodsGroup
                                           from goods in goodsGroup.DefaultIfEmpty()
                                           where t.HeaderId == headerId
                                           select new DetailTgbxDto
                                           {
                                               SoLenh = t.SoLenh,
                                               MaterialName = goods.Name,
                                               TongXuat = t.TongXuat,
                                               DonViTinh = t.DonViTinh
                                           }).ToListAsync();

                return header;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        private string? GetNameWarehouse(string code)
        {
            try
            {
                return _dbContext.TblMdWarehouse.Find(code)?.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string? GetNameMaterial(string materialCode)
        {
            try
            {
                return _dbContext.TblMdGoods.Find(materialCode)?.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
