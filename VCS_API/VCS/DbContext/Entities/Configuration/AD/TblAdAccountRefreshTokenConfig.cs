using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCS.DbContext.Entities.AD;

namespace VCS.DbContext.Configuration.AD
{
    public class TblAdAccountRefreshTokenConfig : IEntityTypeConfiguration<TblAdAccountRefreshToken>
    {
        public void Configure(EntityTypeBuilder<TblAdAccountRefreshToken> builder)
        {
            builder.HasOne(x => x.Account).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserName).IsRequired();
        }
    }
}
