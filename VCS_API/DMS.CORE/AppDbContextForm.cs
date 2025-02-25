using DMS.CORE.Common;
using DMS.CORE.Entities.AD;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE
{
    public class AppDbContextForm : DbContext
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

        public Func<DateTime> TimestampProvider { get; set; } = () => DateTime.UtcNow;

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
        public DbSet<TblAdConfigTemplate> TblAdConfigTemplate { get; set; }
        public DbSet<TblAdConfigApp> TblAdConfigApp { get; set; }
        public DbSet<TblAdAccount> TblAdAccount { get; set; }
        public DbSet<TblAdAccountGroup> TblAdAccountGroup { get; set; }
        public DbSet<TblAdMenu> TblAdMenu { get; set; }
        public DbSet<TblAdRight> TblAdRight { get; set; }
        public DbSet<TblAdMessage> TblAdMessage { get; set; }
        public DbSet<TblAdAccountGroupRight> TblAdAccountGroupRight { get; set; }
        public DbSet<TblAdAccountRefreshToken> TblAdAccountRefreshToken { get; set; }
        public DbSet<TblAdAppVersion> TblAdAppVersion { get; set; }
        public DbSet<TblAdAccount_AccountGroup> TblAdAccount_AccountGroup { get; set; }
        public DbSet<TblActionLog> TblActionLogs { get; set; }
        public DbSet<TblAdSystemTrace> TblAdSystemTrace { get; set; }
        public DbSet<tblAdOrganize> tblAdOrganize { get; set; }
        #endregion

        #region Business Unit
        public DbSet<TblBuHeader> TblBuHeader { get; set; }
        public DbSet<TblBuHeaderTgbx> TblBuHeaderTgbx { get; set; }
        public DbSet<TblBuDetailTgbx> TblBuDetailTgbx { get; set; }
        public DbSet<TblBuDetailDO> TblBuDetailDO { get; set; }
        public DbSet<TblBuDetailMaterial> TblBuDetailMaterial { get; set; }
        public DbSet<TblBuQueue> TblBuQueue { get; set; }
        public DbSet<TblBuImage> TblBuImage { get; set; }
        public DbSet<TblBuOrder> TblBuOrders { get; set; }
        #endregion

        #region Master Data
        public DbSet<TblMdCamera> TblMdCamera { get; set; }
        public DbSet<TblMdPumpRig> TblMdPumpRig { get; set; }
        public DbSet<TblMdPumpThroat> TblMdPumpThroat { get; set; }
        public DbSet<TblMdGoods> TblMdGoods { get; set; }
        public DbSet<TblMdWarehouse> TblMdWarehouse { get; set; }
        public DbSet<TblMdVehicle> TblMdVehicle { get; set; }

        public DbSet<TblMdCustomer> TblMdCustomer { get; set; }
        public DbSet<TblMdCustomerType> TblMdCustomerType { get; set; }
        public DbSet<TblMdArea> TblMdArea { get; set; }
        public DbSet<TblMdCurrency> tblMdCurrency { get; set; }
        public DbSet<TblMdUnit> tblMdUnit { get; set; }
        public DbSet<TblMdLocal> tblMdLocal { get; set; }
        public DbSet<TblMdPosition> TblMdPosition { get; set; }
        public DbSet<TblMdAccountType> tblMdAccountType { get; set; }
        public DbSet<TblMdSequence> tblMdSequence { get; set; }

        #endregion

        #region Common
        public DbSet<TblCmAttachment> TblBuAttachment { get; set; }
        public DbSet<TblCmModuleAttachment> TblBuModuleAttachment { get; set; }
        public DbSet<TblCmComment> TblCmComment { get; set; }
        public DbSet<TblCmModuleComment> TblCmModuleComment { get; set; }
        #endregion
    }

}
