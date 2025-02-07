using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_CONFIG_APP")]
    public class TblAdConfigApp : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }

        [Column("ORG_CODE", TypeName = "NVARCHAR(50)")]
        public string OrgCode { get; set; }
        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(50)")]
        public string WarehouseCode { get; set; }
        [Column("SMO_API_USERNAME", TypeName = "NVARCHAR(50)")]
        public string SmoApiUsername { get; set; }
        [Column("SMO_API_PASSWORD", TypeName = "NVARCHAR(50)")]
        public string SmoApiPassword { get; set; }
        [Column("SMO_API_URL", TypeName = "NVARCHAR(500)")]
        public string SmoApiUrl { get; set; }
        [Column("PATH_SAVE_FILE", TypeName = "NVARCHAR(500)")]
        public string PathSaveFile { get; set; }
        [Column("DETECT_API_URL", TypeName = "NVARCHAR(500)")]
        public string DetectApiUrl { get; set; }
        [Column("CONNECTION_DB", TypeName = "NVARCHAR(1000)")]
        public string ConnectionDb { get; set; }


    }
}
