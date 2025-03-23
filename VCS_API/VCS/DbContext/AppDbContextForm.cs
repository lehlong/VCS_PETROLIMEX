using Microsoft.EntityFrameworkCore;
using VCS.DbContext.Entities.AD;
using VCS.DbContext.Entities.BU;
using VCS.DbContext.Entities.MD;

namespace VCS.DbContext.Common
{
    public class AppDbContextForm : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContextForm(DbContextOptions<AppDbContextForm> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
            modelBuilder.HasSequence<int>("ORDER_SEQUENCE").StartsAt(1).IncrementsBy(1);
            base.OnModelCreating(modelBuilder);
        }

        public Func<DateTime> TimestampProvider { get; set; } = () => DateTime.Now;

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TrackChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity is IBaseEntity auditable)
                {
                    if (entry.State == EntityState.Added)
                        auditable.CreateDate = TimestampProvider();
                    else
                        auditable.UpdateDate = TimestampProvider();
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeleteEntity softDelete)
                {
                    entry.State = EntityState.Modified;
                    softDelete.IsDeleted = true;
                    softDelete.DeleteDate = TimestampProvider();
                }
            }
        }

        public IQueryable<T> GetQuery<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        #region System Manage
        public DbSet<TblAdAccount> TblAdAccount { get; set; }
        #endregion

        #region Business Unit
        public DbSet<TblBuHeader> TblBuHeader { get; set; }
        public DbSet<TblBuHeaderTgbx> TblBuHeaderTgbx { get; set; }
        public DbSet<TblBuDetailTgbx> TblBuDetailTgbx { get; set; }
        public DbSet<TblBuDetailDO> TblBuDetailDO { get; set; }
        public DbSet<TblBuDetailMaterial> TblBuDetailMaterial { get; set; }
        public DbSet<TblBuQueue> TblBuQueue { get; set; }
        public DbSet<TblBuImage> TblBuImage { get; set; }
        #endregion

        #region Master Data
        public DbSet<TblMdCamera> TblMdCamera { get; set; }
        public DbSet<TblMdGoods> TblMdGoods { get; set; }
        public DbSet<TblMdWarehouse> TblMdWarehouse { get; set; }
        public DbSet<TblMdVehicle> TblMdVehicle { get; set; }
        public DbSet<TblMdSequence> tblMdSequence { get; set; }
        #endregion
    }

}
