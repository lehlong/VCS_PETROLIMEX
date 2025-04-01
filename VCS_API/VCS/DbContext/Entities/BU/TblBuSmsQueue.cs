using VCS.DbContext.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Entities.BU
{
    [Table("T_BU_SMS_QUEUE")]
    public class TblBuSmsQueue : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }

        [Column("PHONE", TypeName = "NVARCHAR(50)")]
        public string Phone { get; set; }

        [Column("SMS_CONTENT", TypeName = "NVARCHAR(500)")]
        public string SmsContent { get; set; }

        [Column("IS_SEND")]
        public bool IsSend { get; set; }
        [Column("COUNT")]
        public int Count { get; set; }


    }
}
