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
        [Column("IS_CHECKOUT")]
        public bool IsCheckout { get; set; }
        [Column("TIME_CHECKOUT")]
        public DateTime? TimeCheckout { get; set; }

        [Column("NOTE_IN", TypeName = "NVARCHAR(500)")]
        public string? NoteIn { get; set; }

        [Column("NOTE_OUT", TypeName = "NVARCHAR(500)")]
        public string? NoteOut { get; set; }
    }
}
