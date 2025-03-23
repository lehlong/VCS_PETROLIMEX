using VCS.DbContext.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Entities.BU
{
    [Table("T_BU_DETAIL_DO")]
    public class TblBuDetailDO : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }
        [Column("HEADER_ID", TypeName = "NVARCHAR(50)")]
        public string HeaderId { get; set; }

        [Column("DO1_SAP", TypeName = "NVARCHAR(50)")]
        public string Do1Sap { get; set; }
        [Column("VEHICLE_CODE", TypeName = "NVARCHAR(50)")]
        public string VehicleCode { get; set; }

        [Column("TANK_GROUP", TypeName = "NVARCHAR(50)")]
        public string? TankGroup { get; set; }
        [Column("MODUL_TYPE", TypeName = "NVARCHAR(50)")]
        public string? ModulType { get; set; }
        [Column("CUSTOMER_CODE", TypeName = "NVARCHAR(50)")]
        public string? CustomerCode { get; set; }
        [Column("CUSTOMER_NAME", TypeName = "NVARCHAR(500)")]
        public string? CustomerName { get; set; }
        [Column("PHONE", TypeName = "NVARCHAR(50)")]
        public string? Phone { get; set; }
        [Column("EMAIL", TypeName = "NVARCHAR(50)")]
        public string? Email { get; set; }
        [Column("TAI_XE", TypeName = "NVARCHAR(50)")]
        public string? TaiXe { get; set; }
        [Column("NGUON_HANG", TypeName = "NVARCHAR(50)")]
        public string? NguonHang { get; set; }

    }
}
