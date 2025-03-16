using Common;

namespace DMS.BUSINESS.Filter.BU
{
    public class HeaderFilter : BaseFilter
    {
        public string? VehicleName { get; set; }
        public string? VehicleCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
} 