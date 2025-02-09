using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.BU
{
    [Table("T_BU_IMAGE")]
    public class TblBuImage : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }
        [Column("HEADER_ID", TypeName = "NVARCHAR(50)")]
        public string HeaderId { get; set; }
        [Column("PATH", TypeName = "NVARCHAR(500)")]
        public string? Path { get; set; }
        [Column("FULL_PATH", TypeName = "NVARCHAR(500)")]
        public string? FullPath { get; set; }
        [Column("IS_PLATE")]
        public bool IsPlate { get; set; }
        [Column("IN_OUT", TypeName = "NVARCHAR(50)")]
        public string? InOut { get; set; }

    }
}
