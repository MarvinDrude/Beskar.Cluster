using System.Text.Json;
using Beskar.Cluster.Configuration.Config;
using Beskar.Cluster.Configuration.Constants;
using Beskar.Cluster.Database.Common.Constants;
using Beskar.Cluster.Database.Main.Enums.System;
using Beskar.Cluster.Utilities.Randoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Beskar.Cluster.Database.Main.Entities.System;

public sealed class DbSystemConfigEntryConfiguration 
   : IEntityTypeConfiguration<DbSystemConfigEntry>
{
   public static readonly ValueConverter<DbSystemConfigEntryId, Guid> KeyConverter = new (
      id => id.Value,
      id => new DbSystemConfigEntryId(id)
   );
   
   public void Configure(EntityTypeBuilder<DbSystemConfigEntry> builder)
   {
      builder.Property(e => e.Id)
         .HasConversion(KeyConverter)
         .HasDefaultValueSql(DbConstants.DefaultIdGenerator)
         .ValueGeneratedOnAdd();

      builder.HasKey(e => e.Key);

      builder.Property(e => e.Key)
         .HasMaxLength(512);

      builder.Property(e => e.Value)
         .HasColumnType("jsonb")
         .IsRequired();
   }

   public static DbSystemConfigEntry[] DefaultEntries { get; } =
   [
      new ()
      {
         Key = ConfigurationKeys.AccountIsSignInEnabled,
         Type = SystemConfigType.Boolean,
         Value = JsonSerializer.SerializeToElement(new SystemConfigValueWrapper<bool>(true))
      },
      new ()
      {
         Key = ConfigurationKeys.AccountIsSignUpEnabled,
         Type = SystemConfigType.Boolean,
         Value = JsonSerializer.SerializeToElement(new SystemConfigValueWrapper<bool>(true))
      },
      new ()
      {
         Key = ConfigurationKeys.JwtOptions,
         Type = SystemConfigType.JwtOptions,
         Value = JsonSerializer.SerializeToElement(new SystemConfigValueWrapper<JwtOptions>(new JwtOptions
         {
            Issuer = "Beskar.Cluster",
            KeyV1 = RandomUtils.GenerateRandomBytes(JwtOptions.KeyLength),
            KeyV2 = RandomUtils.GenerateRandomBytes(JwtOptions.KeyLength),
            IsV2Enabled = false,
            ExpirationInMinutes = 20,
            SwitchedAt = DateTimeOffset.UtcNow - TimeSpan.FromDays(1),
            SwapKeysInterval = TimeSpan.FromDays(1),
            RefreshExpirationInDays = 30
         }))
      }
   ];
}