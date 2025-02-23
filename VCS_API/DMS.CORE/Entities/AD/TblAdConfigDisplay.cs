using DMS.CORE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Entities.AD
{
    [Table("T_AD_CONFIG_DISPLAY")]
    public class TblAdConfigDisplay : SoftDeleteEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }

        [Column("NAME", TypeName = "NVARCHAR(500)")]
        public string Name { get; set; }

        [Column("ORG_CODE", TypeName = "NVARCHAR(50)")]
        public string? OrgCode { get; set; }
        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(50)")]
        public string? WarehouseCode { get; set; }

        [Column("C_FROM")]
        public int Cfrom { get; set; }

        [Column("C_TO")]
        public int Cto { get; set; }
        [Column("DISPLAY", TypeName = "NVARCHAR(50)")]
        public string? Display { get; set; }

    }
}
