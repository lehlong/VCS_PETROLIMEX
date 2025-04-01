using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCS.DbContext.Common;

namespace VCS.DbContext.Entities.AD
{
    [Table("T_AD_SMS_CONFIG")]
    public class TblAdSmsConfig : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "nvarchar(50)")]
        public string Id { get; set; }

        [Column("SMS_IN", TypeName = "nvarchar(1000)")]
        public string? SmsIn { get; set; }

        [Column("SMS_OUT", TypeName = "nvarchar(1000)")]
        public string? SmsOut { get; set; }
    }
}
