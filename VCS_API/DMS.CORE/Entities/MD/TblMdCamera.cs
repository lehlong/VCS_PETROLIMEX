using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    [Table("T_MD_CAMERA")]
    public class TblMdCamera : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }

        [Column("ORG_CODE", TypeName = "NVARCHAR(50)")]
        public string? OrgCode { get; set; }

        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(50)")]
        public string? WarehouseCode { get; set; }

        [Column("IP", TypeName = "NVARCHAR(50)")]
        public string? Ip { get; set; }

        [Column("USER_NAME", TypeName = "NVARCHAR(50)")]
        public string? Username { get; set; }

        [Column("PASSWORD", TypeName = "NVARCHAR(50)")]
        public string? Password { get; set; }

        [Column("RTSP", TypeName = "NVARCHAR(50)")]
        public string? Rtsp { get; set; }

        [Column("STREAM", TypeName = "NVARCHAR(50)")]
        public string? Stream { get; set; }

        [Column("IS_IN")]
        public bool? IsIn { get; set; }

        [Column("IS_OUT")]
        public bool? IsOut { get; set; }

        [Column("IS_RECOGNITION")]
        public bool? IsRecognition { get; set; }

    }
}
