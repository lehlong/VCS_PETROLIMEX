using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.COMMON.Common.Class
{
    public class ArrangePumpNozzleModel
    {
        public string? CompanyCode { get; set; }
        public string? WarehouseCode { get; set; }
        public string? PumpRigCode { get; set; }
        public string? PumpRigName { get; set; }
        public string? PumpThroatCode { get; set; }
        public string? PumpThroatName { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public List<string>? Order { get; set; }
    }
}
