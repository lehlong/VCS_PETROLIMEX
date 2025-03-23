
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCS.DbContext.Common;

namespace VCS.DbContext.Entities.MD
{
    [Table("T_MD_SEQUENCE")]
    public class TblMdSequence : BaseEntity
    {
        [Key]
        [Column("CODE", TypeName = "NVARCHAR(50)")]
        public string Code { get; set; }

        [Column("STT")]
        public int STT { get; set; }
        [Column("COMPANY_CODE", TypeName = "NVARCHAR(255)")]
        public string OrgCode { get; set; }
        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(255)")]
        public string WarehouseCode { get; set; }
    
    }
  
}
