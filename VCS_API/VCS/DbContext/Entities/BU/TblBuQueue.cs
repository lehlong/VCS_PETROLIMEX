using VCS.DbContext.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Entities.BU
{
    [Table("T_BU_QUEUE")]
    public class TblBuQueue : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }
        [Column("HEADER_ID", TypeName = "NVARCHAR(50)")]
        public string HeaderId { get; set; }

        [Column("VEHICLE_CODE", TypeName = "NVARCHAR(50)")]
        public string VehicleCode { get; set; }
        [Column("NAME", TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }
        [Column("C_ORDER", TypeName = "INT")]
        public int Order { get; set; }
        [Column("COUNT", TypeName = "INT")]
        public int Count { get; set; }


    }
}
