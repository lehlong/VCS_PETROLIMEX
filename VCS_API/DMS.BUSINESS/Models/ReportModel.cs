using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Models
{
    public class ReportModel
    {
        public class BaoCaoChiTietXeModel
        {
            public int Hour { get; set; }
            public int XeRa { get; set; }
            public int XeVao { get; set; }
            public int XeKhongHopLe { get; set; }
        }

        public class FilterReport
        {
            public DateTime? FDate { set; get; }
            public DateTime? TDate { set; get; }
            public string? WarehouseCode { get; set; }
            public DateTime Time { get; set; }
        }

    }
    
    public class BaoCaoXeTongHop
    {
        public DateTime date { get; set; }
        public int XeRa { get; set; }
        public int XeVao { get; set; }
        public int XeKhongHopLe { get; set; }

    }
    public class BaoCaoSanPhamTongHop
    {
        public DateTime date { get; set; }
        public List<PriceGoods> priceGoods { set; get; } = new List<PriceGoods>();

    }

    public class PriceGoods
    {
        public DateTime date { get; set; }
            public string? GoodsCode { get; set; }
        public decimal? price { set; get; }
    }
}