using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.BU;
using DMS.BUSINESS.Filter.BU;
using DMS.BUSINESS.Models;
using DMS.CORE;
using DMS.CORE.Entities.BU;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DMS.BUSINESS.Models.ReportModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DMS.BUSINESS.Services.BU
{
    public interface IHeaderService : IGenericService<TblBuHeader, HeaderDto>
    {
        Task<HistoryDto> GetHistoryDetail(string headerId);
        Task<PagedResponseDto> Search(HeaderFilter filter);
        Task<List<BaoCaoXeTongHop>> getBaoCaoXeTongHop(FilterReport filter);
        Task<List<BaoCaoSanPhamTongHop>> getBaoCaoSanPhamTongHop(FilterReport filter);
        Task<byte[]> ExportExcelBaoCaoXeTongHop(FilterReport filter);
        Task<byte[]> ExportExcelBaoCaoSanPhamTongHop(FilterReport filter);
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
                    query = query.Where(x => x.CreateDate <= filter.ToDate.Value);
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

        public async Task<List<BaoCaoXeTongHop>> getBaoCaoXeTongHop(FilterReport filter)
        {
            var lstHeader = await _dbContext.TblBuHeader.ToListAsync();
            if (filter.WarehouseCode != null)
            {
                lstHeader = lstHeader.Where(x => x.WarehouseCode == filter.WarehouseCode).ToList();
            }
            if (filter.FDate.HasValue)
            {
                lstHeader = lstHeader.Where(x => x.CreateDate <= filter.FDate).ToList();
            }
            if (filter.TDate.HasValue)
            {
                lstHeader = lstHeader.Where(x => x.CreateDate >= filter.TDate).ToList();
            }
            var groupedHeaders = lstHeader
                .GroupBy(header => header.CreateDate.Value.Date) // Nhóm theo ngày tạo
                .Select(g => new BaoCaoXeTongHop
                {
                    date = (DateTime)g.Key,
                    XeVao = g.Count(x => x.StatusVehicle == "01" || x.StatusVehicle == "02" || x.StatusVehicle == "03"),
                    XeRa = g.Count(x => x.StatusVehicle == "04"),
                    XeKhongHopLe = g.Count(x => x.StatusProcess == "05"),
                }).OrderByDescending(g => g.date)
                .ToList();
            return groupedHeaders;
        }

        public async Task<List<BaoCaoSanPhamTongHop>> getBaoCaoSanPhamTongHop(FilterReport filter)
        {
            var dateNow = DateTime.Today;
            var lstHeader = await _dbContext.TblBuHeader.ToListAsync();
            var lstGoods = await _dbContext.TblMdGoods.ToListAsync();
            var lstTgbx = await _dbContext.TblBuDetailTgbx.ToListAsync();

            if (filter.WarehouseCode != null)
            {
                lstHeader = lstHeader.Where(x => x.WarehouseCode == filter.WarehouseCode).ToList();
            }
            if (filter.FDate.HasValue)
            {
                lstTgbx = lstTgbx.Where(x => x.NgayXuat <= filter.FDate).ToList();
            }
            else if (filter.TDate.HasValue)
            {
                lstTgbx = lstTgbx.Where(x => x.NgayXuat >= filter.TDate).ToList();
            }
            else
            {
                lstTgbx = lstTgbx.Where(x => x.NgayXuat == dateNow).ToList();

            }
            var data = new List<BaoCaoSanPhamTongHop>();

            foreach (var h in lstHeader)
            {
                var report = new BaoCaoSanPhamTongHop();
                DateTime date = DateTime.Now;
                foreach (var g in lstGoods)
                {
                    var groupedData = lstTgbx
                       .Where(x => "000000000000" + x.MaHangHoa == g.Code && x.HeaderId == h.Id)
                       .GroupBy(x => x.NgayXuat.Value.Date) 
                       .Select(group => new PriceGoods
                       {
                           date = group.Key,
                           GoodsCode = g.Code.ToString(),
                           price = group.Sum(x => x.TongXuat)
                       })
                       .ToList();
                    
                        if (groupedData.Any())

                        {
                            report.priceGoods.AddRange(groupedData);
                            date = groupedData.First().date;
                    }
                }
                if (report.priceGoods.Any())
                {
                    report.date = date; 
                    data.Add(report);
                }
            }

            var report1 = new BaoCaoSanPhamTongHop
            {
                priceGoods = data.SelectMany(d => d.priceGoods) 
                                .GroupBy(pg => pg.GoodsCode) 
                                .Select(g => new PriceGoods()
                                {
                                    GoodsCode = g.Key,
                                    price = g.Sum(pg => pg.price) 
                                }).ToList()
            };
            data.Add(report1);

            return data;
        }
        public async Task<byte[]> ExportExcelBaoCaoXeTongHop(FilterReport filter)
        {
            try
            {
                var data = await getBaoCaoXeTongHop(filter);

                byte[] fileBytes;

                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Báo cáo xe tổng hợp");

                // Font in đậm cho header
                IFont boldFont = workbook.CreateFont();
                boldFont.IsBold = true;

                // Style cho header (border + in đậm + căn giữa)
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.BorderTop = BorderStyle.Thin;
                headerStyle.BorderBottom = BorderStyle.Thin;
                headerStyle.BorderLeft = BorderStyle.Thin;
                headerStyle.BorderRight = BorderStyle.Thin;
                headerStyle.Alignment = HorizontalAlignment.Center;
                headerStyle.VerticalAlignment = VerticalAlignment.Center;
                headerStyle.SetFont(boldFont);

                // Style cho cell thường (có border + căn giữa)
                ICellStyle borderStyle = workbook.CreateCellStyle();
                borderStyle.BorderTop = BorderStyle.Thin;
                borderStyle.BorderBottom = BorderStyle.Thin;
                borderStyle.BorderLeft = BorderStyle.Thin;
                borderStyle.BorderRight = BorderStyle.Thin;
                borderStyle.Alignment = HorizontalAlignment.Center;
                borderStyle.VerticalAlignment = VerticalAlignment.Center;

                // Tạo dòng tiêu đề
                var header = sheet.CreateRow(0);
                string[] titles = { "STT", "Ngày", "Xe vào", "Xe ra", "Xe không hợp lệ" };

                for (int i = 0; i < titles.Length; i++)
                {
                    var cell = header.CreateCell(i);
                    cell.SetCellValue(titles[i]);
                    cell.CellStyle = headerStyle;
                }

                // Dữ liệu
                int rowIndex = 1;
                foreach (var item in data)
                {
                    var row = sheet.CreateRow(rowIndex);

                    var cell0 = row.CreateCell(0);
                    cell0.SetCellValue(rowIndex);
                    cell0.CellStyle = borderStyle;

                    var cell1 = row.CreateCell(1);
                    cell1.SetCellValue(item.date.ToString("dd/MM/yyyy"));
                    cell1.CellStyle = borderStyle;

                    var cell2 = row.CreateCell(2);
                    cell2.SetCellValue(item.XeVao);
                    cell2.CellStyle = borderStyle;

                    var cell3 = row.CreateCell(3);
                    cell3.SetCellValue(item.XeRa);
                    cell3.CellStyle = borderStyle;

                    var cell4 = row.CreateCell(4);
                    cell4.SetCellValue(item.XeKhongHopLe);
                    cell4.CellStyle = borderStyle;

                    rowIndex++;
                }

                // Auto size các cột
                for (int i = 0; i < titles.Length; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms, true);
                    fileBytes = ms.ToArray();
                }

                return await Task.FromResult(fileBytes);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }


        public async Task<byte[]> ExportExcelBaoCaoSanPhamTongHop(FilterReport filter)
        {
            try
            {
                var data = await getBaoCaoSanPhamTongHop(filter);

                byte[] fileBytes;

                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Báo cáo xe tổng hợp");

                // Font in đậm cho header
                IFont boldFont = workbook.CreateFont();
                boldFont.IsBold = true;

                // Style cho header (border + in đậm + căn giữa)
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.BorderTop = BorderStyle.Thin;
                headerStyle.BorderBottom = BorderStyle.Thin;
                headerStyle.BorderLeft = BorderStyle.Thin;
                headerStyle.BorderRight = BorderStyle.Thin;
                headerStyle.Alignment = HorizontalAlignment.Center;
                headerStyle.VerticalAlignment = VerticalAlignment.Center;
                headerStyle.SetFont(boldFont);

                // Style cho cell thường (có border + căn giữa)
                ICellStyle borderStyle = workbook.CreateCellStyle();
                borderStyle.BorderTop = BorderStyle.Thin;
                borderStyle.BorderBottom = BorderStyle.Thin;
                borderStyle.BorderLeft = BorderStyle.Thin;
                borderStyle.BorderRight = BorderStyle.Thin;
                borderStyle.Alignment = HorizontalAlignment.Center;
                borderStyle.VerticalAlignment = VerticalAlignment.Center;

                // Tạo dòng tiêu đề
                var header = sheet.CreateRow(0);
                var lstGoods = await _dbContext.TblMdGoods.OrderBy(x => x.CreateDate).ToListAsync();
                var stastCell = 0;

                var cell0 = header.CreateCell(stastCell++);
                cell0.SetCellValue("Stt");
                cell0.CellStyle = headerStyle;
                sheet.AutoSizeColumn(stastCell);

                var cell1 = header.CreateCell(stastCell++);
                cell1.SetCellValue("Ngày");
                cell1.CellStyle = headerStyle;
                sheet.AutoSizeColumn(stastCell);

                foreach (var i in lstGoods)
                {
                    var cell = header.CreateCell(stastCell++);
                    cell.SetCellValue(i.Name);
                    cell.CellStyle = headerStyle;
                    sheet.AutoSizeColumn(stastCell);
                }

                var rowIndex = 1;
                foreach (var d in data)
                {
                    stastCell = 0;
                    var body = sheet.CreateRow(rowIndex++);

                    var cellBd0 = body.CreateCell(stastCell++);
                    cellBd0.SetCellValue(stastCell+1);
                    cellBd0.CellStyle = borderStyle;

                    var cellBd1 = body.CreateCell(stastCell++);
                    cellBd1.SetCellValue(d.date != DateTime.MinValue ? d.date.ToString("dd/MM/yyyy") : "Tổng");
                    cellBd1.CellStyle = borderStyle;

                    foreach (var i in lstGoods)
                    {
                        var cellBd2 = body.CreateCell(stastCell++);
                        cellBd2.SetCellValue((double)(d.priceGoods.Where(x => x.GoodsCode == i.Code).Sum(x => x.price) ?? 0));
                        cellBd2.CellStyle = borderStyle;

                    }
                }
                for (int i = 0; i < lstGoods.Count + 2; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms, true);
                    fileBytes = ms.ToArray();
                }

                return await Task.FromResult(fileBytes);
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
