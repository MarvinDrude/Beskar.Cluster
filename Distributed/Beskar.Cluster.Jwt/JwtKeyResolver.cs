using System.Collections.Concurrent;
using Beskar.Cluster.Configuration.Config;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.IdentityModel.Tokens;

namespace Beskar.Cluster.Jwt;

public sealed class JwtKeyResolver
{
   private const string KeyV1Name = "v1-sa";
   private const string KeyV2Name = "v2-sa";
   
   public JwtOptions Options => _options ?? throw new InvalidOperationException("Jwt options not initialized");
   
   private JwtOptions? _options;
   private ConcurrentDictionary<string, SecurityKey> _keys = [];

   private string ActiveKeyId => Options.IsV2Enabled ? KeyV2Name : KeyV1Name;

   public Result<SecurityKey, StringError> GetActiveKey()
   {
      return GetKey(ActiveKeyId);
   }
   
   public Result<SecurityKey, StringError> GetKey(string keyId)
   {
      var key = _keys.GetValueOrDefault(keyId);
      if (key is null)
      {
         return new StringError($"Key {keyId} not found.");
      }

      return key;
   }
   
   public void UpdateOptions(JwtOptions options)
   {
      _options = options;
      
      _keys[KeyV1Name] = new SymmetricSecurityKey(options.KeyV1)
      {
         KeyId = KeyV1Name
      };
      
      _keys[KeyV2Name] = new SymmetricSecurityKey(options.KeyV2)
      {
         KeyId = KeyV2Name
      };
   }
}