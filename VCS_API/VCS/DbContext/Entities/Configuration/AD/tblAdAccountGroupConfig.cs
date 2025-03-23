using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCS.DbContext.Entities.AD;

namespace VCS.DbContext.Configuration.AD
{
    public class TblAdAccountGroupConfig : IEntityTypeConfiguration<TblAdAccountGroup>
    {
        public void Configure(EntityTypeBuilder<TblAdAccountGroup> builder)
        {
            builder.HasMany(x => x.Account_AccountGroups).WithOne(y => y.AccountGroup).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
