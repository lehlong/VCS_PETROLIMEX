using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.BU
{
    [Table("T_BU_DETAIL_MATERIAL")]
    public class TblBuDetailMaterial : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }
        [Column("HEADER_ID", TypeName = "NVARCHAR(50)")]
        public string HeaderId { get; set; }

        [Column("MATERIAL_CODE", TypeName = "NVARCHAR(50)")]
        public string MaterialCode { get; set; }

        [Column("QUANTITY", TypeName = "DECIMAL(18,0)")]
        public decimal Quantity { get; set; }
        [Column("UNIT_CODE", TypeName = "NVARCHAR(50)")]
        public string UnitCode { get; set; }

    }
}
