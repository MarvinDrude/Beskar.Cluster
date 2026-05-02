using System.Security.Claims;
using Beskar.Cluster.Jwt.Parameters;
using Me.Memory.Results;
using Me.Memory.Results.Errors;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Beskar.Cluster.Jwt;

public sealed class JwtGenerator(
   JwtKeyResolver keyResolver,
   TimeProvider timeProvider)
{
   private readonly TimeProvider _timeProvider = timeProvider;
   private readonly JwtKeyResolver _keyResolver = keyResolver;
   
   private readonly JsonWebTokenHandler _tokenHandler = new()
   {
      SetDefaultTimesOnTokenCreation = false
   };

   public async Task<Result<ClaimsPrincipal, StringError>> ValidateToken(string token)
   {
      try
      {
         var options = _keyResolver.Options;
         var validationParameters = new TokenValidationParameters()
         {
            ValidIssuer = options.Issuer,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidAudience = options.Audience,
            ValidateAudience = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            IssuerSigningKeyResolver = (_, _, kid, _) =>
            {
               var keyResult = _keyResolver.GetKey(kid);
               return keyResult.Failed 
                  ? [] : [keyResult.Success];
            }
         };

         var result = await _tokenHandler.ValidateTokenAsync(token, validationParameters);
         if (!result.IsValid)
         {
            return new StringError($"Invalid Token: {result.Exception.Message}");
         }
         
         return new ClaimsPrincipal(result.ClaimsIdentity);
      }
      catch (Exception err)
      {
         return new StringError(err.ToString());
      }
   }
   
   public Result<string, StringError> CreateToken(JwtAccountParameters parameters)
   {
      return CreateToken(new Dictionary<string, object>()
      {
         [ClaimTypes.NameIdentifier] = parameters.AccountId, 
      });
   }

   private Result<string, StringError> CreateToken(Dictionary<string, object> claims)
   {
      try
      {
         var now = _timeProvider.GetUtcNow().DateTime;
         var activeResult = _keyResolver.GetActiveKey();

         if (activeResult.Failed)
         {
            return activeResult.Error;
         }
         
         var options = _keyResolver.Options;
         var descriptor = new SecurityTokenDescriptor()
         {
            Issuer = options.Issuer,
            Claims = claims,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.Add(TimeSpan.FromMinutes(options.ExpirationInMinutes)),
            SigningCredentials = new SigningCredentials(activeResult.Success, SecurityAlgorithms.HmacSha512)
         };

         return _tokenHandler.CreateToken(descriptor);
      }
      catch (Exception err)
      {
         return new StringError(err.ToString());
      }
   }
}