
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCS.DbContext.Common;

namespace VCS.DbContext.Entities.MD
{
    [Table("T_MD_WAREHOUSE")]
    public class TblMdWarehouse : SoftDeleteEntity
    {
        [Key]
        [Column("CODE", TypeName = "VARCHAR(50)")]
        public string Code { get; set; }

        [Column("NAME", TypeName = "NVARCHAR(255)")]
        public string Name { get; set; }
        [Column("ORG_CODE", TypeName = "NVARCHAR(50)")]
        public string OrgCode { get; set; }
        [Column("TGBX", TypeName = "NVARCHAR(50)")]
        public string Tgbx { get; set; }
        [Column("TDH", TypeName = "NVARCHAR(50)")]
        public string Tdh { get; set; }
        [Column("TDH_E5", TypeName = "NVARCHAR(50)")]
        public string? Tdh_e5 { get; set; }
        [Column("IS_SMS_IN")]
        public bool? Is_sms_in { get; set; }
        [Column("IS_SMS_OUT")]
        public bool? Is_sms_out { get; set; }
    }
}
