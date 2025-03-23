using VCS.DbContext.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Entities.AD
{
    [Table("T_AD_ACCOUNT")]
    public class TblAdAccount : SoftDeleteEntity
    {
        [Key]
        [Column("USER_NAME",TypeName = "VARCHAR(50)")]
        public string UserName { get; set; }

        [Column("FULL_NAME",TypeName = "NVARCHAR(255)")]
        public string FullName { get; set; }

        [Column("PASSWORD", TypeName = "VARCHAR(50)")]
        public string Password { get; set; }

        [Column("PHONE_NUMBER",TypeName = "VARCHAR(10)")]
        public string? PhoneNumber { get; set; }

        [Column("EMAIL",TypeName = "VARCHAR(255)")]
        public string? Email { get; set; }

        [Column("ADDRESS",TypeName = "NVARCHAR(255)")]
        public string? Address { get; set; }

        [Column("ACCOUNT_TYPE", TypeName = "VARCHAR(10)")]
        public string? AccountType { get; set; }

        [Column("ORG_CODE", TypeName = "VARCHAR(50)")]
        public string? OrganizeCode { get; set; }

        [Column("WAREHOUSE_CODE", TypeName = "VARCHAR(50)")]
        public string? WarehouseCode { get; set; }

        [Column("POSITION_CODE", TypeName = "VARCHAR(50)")]
        public string? PositionCode { get; set; }

        public virtual ICollection<TblAdAccount_AccountGroup> Account_AccountGroups { get; set; }

        public virtual ICollection<TblAdAccountRight> AccountRights { get; set; }

        public virtual ICollection<TblAdAccountRefreshToken> RefreshTokens { get; set; }
    

    }
}
