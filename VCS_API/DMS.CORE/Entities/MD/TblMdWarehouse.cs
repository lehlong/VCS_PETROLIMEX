﻿using DMS.CORE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Entities.MD
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
        public string tgbx { get; set; }
        [Column("TDH", TypeName = "NVARCHAR(50)")]
        public string tdh { get; set; }
    }
}
