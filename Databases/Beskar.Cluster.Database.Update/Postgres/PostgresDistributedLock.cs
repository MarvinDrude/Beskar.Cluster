using System.Data;
using System.Security.Cryptography;
using System.Text;
using Beskar.Cluster.Database.Common.Enums;
using Beskar.Cluster.Database.Common.Interfaces.Contexts;
using Npgsql;

namespace Beskar.Cluster.Database.Update.Postgres;

public sealed class PostgresDistributedLock(
   IDbConnectionStringProvider provider,
   DbContextKind kind,
   string lockId) 
   : IDistributedLock
{
   private readonly DbContextKind _kind = kind;
   private readonly long _lockId = GetDeterministicHashCode(lockId);
   
   private NpgsqlConnection? _connection;
   
   public async Task<bool> TryAcquire(CancellationToken ct = default)
   {
      _connection = new NpgsqlConnection(await provider.GetConnectionString(_kind, ct));
      await _connection.OpenAsync(ct);

      await using var cmd = new NpgsqlCommand("SELECT pg_try_advisory_lock(@id)", _connection);
      cmd.Parameters.AddWithValue("id", _lockId);

      var result = (bool?)await cmd.ExecuteScalarAsync(ct);

      if (result == true)
      {
         return true;
      }

      await DisposeAsync();
      return false;
   }

   public async Task Acquire(CancellationToken ct = default)
   {
      _connection = new NpgsqlConnection(await provider.GetConnectionString(_kind, ct));
      await _connection.OpenAsync(ct);

      // pg_advisory_lock block until available
      await using var cmd = new NpgsqlCommand("SELECT pg_advisory_lock(@id)", _connection);
      cmd.Parameters.AddWithValue("id", _lockId);

      await cmd.ExecuteNonQueryAsync(ct);
   }

   public async ValueTask DisposeAsync()
   {
      if (_connection is null) return;

      try
      {
         if (_connection.State is ConnectionState.Open)
         {
            await using var cmd = new NpgsqlCommand("SELECT pg_advisory_unlock(@id)", _connection);
            cmd.Parameters.AddWithValue("id", _lockId);
            
            await cmd.ExecuteNonQueryAsync();
         }
      }
      finally
      {
         await _connection.DisposeAsync();
         _connection = null;
      }
   }
   
   private static long GetDeterministicHashCode(string input)
   {
      var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
      return BitConverter.ToInt64(hashBytes, 0);
   }
}