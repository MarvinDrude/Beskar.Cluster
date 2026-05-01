using Beskar.Cluster.Database.Main.Entities.Account;
using Beskar.Cluster.Database.Main.Entities.Projects;
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
   
   public DbSet<DbProject> Projects => Set<DbProject>();
   public DbSet<DbProjectMember> ProjectMembers => Set<DbProjectMember>();
   public DbSet<DbProjectInvitation> ProjectInvitations => Set<DbProjectInvitation>();
   public DbSet<DbProjectRole> ProjectRoles => Set<DbProjectRole>();
   public DbSet<DbProjectRolePermission> ProjectRolePermissions => Set<DbProjectRolePermission>();
   
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
      
      modelBuilder.ApplyConfiguration(new DbProjectConfiguration());
      modelBuilder.ApplyConfiguration(new DbProjectMemberConfiguration());
      modelBuilder.ApplyConfiguration(new DbProjectInvitationConfiguration());
      modelBuilder.ApplyConfiguration(new DbProjectRoleConfiguration());
      modelBuilder.ApplyConfiguration(new DbProjectRolePermissionConfiguration());
   }
}