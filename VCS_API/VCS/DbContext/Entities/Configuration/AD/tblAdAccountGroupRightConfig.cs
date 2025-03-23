using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCS.DbContext.Entities.AD;

namespace VCS.DbContext.Configuration.AD
{
    public class TblAdAccountGroupRightConfig : IEntityTypeConfiguration<TblAdAccountGroupRight>
    {
        public void Configure(EntityTypeBuilder<TblAdAccountGroupRight> builder)
        {
            builder.HasOne<TblAdRight>(x => x.Right).WithMany(y => y.AccountGroupRights).HasForeignKey(x => x.RightId);
            builder.HasOne<TblAdAccountGroup>(x => x.AccountGroup).WithMany(y => y.ListAccountGroupRight).HasForeignKey(x => x.GroupId);
        }
    }
}
