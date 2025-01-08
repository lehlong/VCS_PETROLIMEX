using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.BU
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

    }
}
