using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
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
        Task <HistoryDto> GetHistoryDetail (string headerId);
    }
    public class HeaderService(AppDbContext dbContext, IMapper mapper) : GenericService<TblBuHeader, HeaderDto>(dbContext, mapper), IHeaderService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblBuHeader.AsQueryable();
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
        public async Task<HistoryDto> GetHistoryDetail(string headerId)
        {
            try
            {
                var header = await _dbContext.TblBuHeader
                    .Where(x => x.Id == headerId)
                    .Select(x => new HistoryDto
                    {
                        VehicleCode = x.VehicleCode,
                        CreateDate = x.CreateDate,
                        StatusVehicle = x.StatusVehicle,
                        StatusProcess = x.StatusProcess,
                        NoteIn = x.NoteIn,
                        NoteOut = x.NoteOut,
                        Stt = x.Stt.ToString()
                    })
                    .FirstOrDefaultAsync();

                if (header == null)
                {
                    Status = false;
                    MessageObject.MessageDetail = "Không tìm thấy thông tin phiếu";
                    return null;
                }
                header.ImagesIn = await _dbContext.TblBuImage
                    .Where(x => x.HeaderId == headerId && x.InOut == "in")
                    .Select(x => x.FullPath)
                    .ToListAsync();
                header.ImagesOut = await _dbContext.TblBuImage
                    .Where(x => x.HeaderId == headerId && x.InOut == "out")
                    .Select(x => x.FullPath)
                    .ToListAsync();
                var lstDO = await _dbContext.TblBuDetailDO
                    .Where(x => x.HeaderId == headerId)
                    .Select(d => new DetailDto
                    {
                        Do1Sap = d.Do1Sap,
                        Materials = _dbContext.TblBuDetailMaterial
                            .Where(m => m.HeaderId == d.Id)
                            .Select(m => new MaterialDto
                            {
                                MaterialCode = m.MaterialCode,
                                Quantity = m.Quantity,
                                UnitCode = m.UnitCode,
                                // MaterialName = _dbContext.TblMdGoods.FirstOrDefaultAsync(m.MaterialCode) != null ? _dbContext.TblMdGoods.FirstOrDefaultAsync(m.MaterialCode).Name : null 
                            }).ToList()
                    }).ToListAsync();

                header.DetailDOs = lstDO;
                var lstDOOUT = await _dbContext.TblBuDetailTgbx
                    .Where(x => x.HeaderId == headerId)
                    .Select(x => new DetailTgbxDto
                    {
                        SoLenh = x.SoLenh,
                        // MaterialName = _dbContext.TblMdGoods.Find("000000000000" + x.MaHangHoa) != null ? _dbContext.TblMdGoods.Find("000000000000" + x.MaHangHoa).Name : null,
                        TongXuat = x.TongXuat,
                        DonViTinh = x.DonViTinh
                    }).ToListAsync();

                header.DetailTgbx = lstDOOUT;
                var totalCount = 1;
                return header;
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
