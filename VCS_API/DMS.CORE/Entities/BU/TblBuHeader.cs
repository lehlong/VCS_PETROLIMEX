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


    }
}
