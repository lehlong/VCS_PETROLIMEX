using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_VEHICLE")]
    public class TblMdVehicle : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "NVARCHAR(10)")]
        public string Code { get; set; }

        [Column("TRANSUNIT_CODE", TypeName = "NVARCHAR(4)")]
        public string? TransunitCode { get; set; }

        [Column("UNIT", TypeName = "NVARCHAR(3)")]
        public string? Unit { get; set; }

        [Column("OIC_PBATCH", TypeName = "NVARCHAR(16)")]
        public string? OicPbatch { get; set; }

        [Column("OIC_PTRIP", TypeName = "NVARCHAR(16)")]
        public string? OicPtrip { get; set; }

        [Column("CAPACITY", TypeName = "DECIMAL(18,0)")]
        public decimal? Capacity { get; set; }

        [Column("TRANSMODE_CODE", TypeName = "NVARCHAR(2)")]
        public string? TransmodeCode { get; set; }


    }
}
