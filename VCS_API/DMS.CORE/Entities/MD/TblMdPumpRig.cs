using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_PUMP_RIG")]
    public class TblMdPumpRig : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "NVARCHAR(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }
        [Column("ORG_CODE", TypeName = "NVARCHAR(255)")]
        public string OrgCode { get; set; }
        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(255)")]
        public string WarehouseCode { get; set; }
     



    }
}
