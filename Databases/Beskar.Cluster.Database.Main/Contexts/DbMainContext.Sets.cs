using Beskar.Cluster.Database.Main.Entities.Account;
using Beskar.Cluster.Database.Main.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Beskar.Cluster.Database.Main.Contexts;

public sealed partial class DbMainContext
{
   public DbSet<DbSystemConfigEntry> SystemConfigEntries => Set<DbSystemConfigEntry>();
   
   public DbSet<DbAccount> Accounts => Set<DbAccount>();
   public DbSet<DbAccountDevice> AccountDevices => Set<DbAccountDevice>();
   public DbSet<DbAccountMultiFactor> AccountMultiFactors => Set<DbAccountMultiFactor>();
   public DbSet<DbAccountSecurityToken> AccountSecurityTokens => Set<DbAccountSecurityToken>();
   public DbSet<DbAccountExternal> AccountExternals => Set<DbAccountExternal>();
   public DbSet<DbAccountBackupCode> AccountBackupCodes => Set<DbAccountBackupCode>();
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      
      modelBuilder.ApplyConfiguration(new DbSystemConfigEntryConfiguration());
      
      modelBuilder.ApplyConfiguration(new DbAccountConfiguration());
      modelBuilder.ApplyConfiguration(new DbAccountDeviceConfiguration());
      modelBuilder.ApplyConfiguration(new DbAccountMultiFactorConfiguration());
      modelBuilder.ApplyConfiguration(new DbAccountSecurityTokenConfiguration());
      modelBuilder.ApplyConfiguration(new DbAccountExternalConfiguration());
      modelBuilder.ApplyConfiguration(new DbAccountBackupCodeConfiguration());
   }
}