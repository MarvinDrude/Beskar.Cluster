using Beskar.Cluster.Backend.Inputs;
using Beskar.Cluster.Jwt;
using Beskar.Cluster.Jwt.Extensions;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Interfaces;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Models;
using Me.Memory.Results;

namespace Beskar.Cluster.Backend.Processors;

public sealed class AuthenticationProcessor(
   JwtGenerator jwtGenerator)
   : IValueAsyncProcessor<ProRequest, ProRequest>
{
   private readonly JwtGenerator _jwtGenerator = jwtGenerator;
   
   public ValueTask<Result<ProRequest, ProcessorError>> Execute(
      ProcessorContext context, ProRequest input, CancellationToken cancellationToken)
   {
      var bearerToken = input.UserProperties.RawBearerToken;
      if (bearerToken is not { Length: > 0 })
      {
         return ValueTask.FromResult<Result<ProRequest, ProcessorError>>(input);
      }

      if (bearerToken.StartsWith(_bearer, StringComparison.InvariantCultureIgnoreCase))
      {
         bearerToken = bearerToken[_bearer.Length..];
      }

      return ValidateToken(input, bearerToken);
   }

   private async ValueTask<Result<ProRequest, ProcessorError>> ValidateToken(ProRequest input, string token)
   {
      var tokenResult = await _jwtGenerator.ValidateToken(token);
      if (tokenResult.Failed)
      {
         return input;
      }

      var rawAccountId = tokenResult.Success.RawAccountIdentifier;
      if (rawAccountId is null)
      {
         return input;
      }

      if (Guid.TryParse(rawAccountId, out var accountId))
      {
         input.UserProperties.AccountId = accountId;
      }
      
      return input;
   }
   
   private const string _bearer = "Bearer ";
}