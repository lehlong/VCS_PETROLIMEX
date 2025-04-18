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
            public string? WarehouseCode { get; set; }
            public DateTime Time { get; set; }
        }
    }
}