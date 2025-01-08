using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.BU
{
    [Table("T_BU_HEADER")]
    public class TblBuHeader : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }

        [Column("VEHICLE_CODE", TypeName = "NVARCHAR(50)")]
        public string VehicleCode { get; set; }

        [Column("COMPANY_CODE", TypeName = "NVARCHAR(50)")]
        public string CompanyCode { get; set; }
        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(50)")]
        public string WarehouseCode { get; set; }
    }
}
