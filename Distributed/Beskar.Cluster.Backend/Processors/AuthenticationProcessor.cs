using Beskar.Cluster.Backend.Inputs;
using Beskar.Cluster.Jwt;
using Beskar.Cluster.Jwt.Extensions;
using Beskar.CodeGeneration.ObserveGenerator.Marker.Attributes;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Interfaces;
using Beskar.CodeGeneration.ProcessorGenerator.Marker.Models;
using Me.Memory.Results;

namespace Beskar.Cluster.Backend.Processors;

[Observe]
[ObserveActivity("processor.authentication")]
public sealed partial class AuthenticationProcessor(
   JwtGenerator jwtGenerator)
   : IValueAsyncProcessor<ProRequest, ProRequest>
{
   private readonly JwtGenerator _jwtGenerator = jwtGenerator;
   private const string _resultTagName = "processor.authentication.result";
   
   public ValueTask<Result<ProRequest, ProcessorError>> Execute(
      ProcessorContext context, ProRequest input, CancellationToken cancellationToken)
   {
      using var activity = ActivitySource.StartActivity();
      
      var bearerToken = input.UserProperties.RawBearerToken;
      if (bearerToken is not { Length: > 0 })
      {
         activity?.AddTag(_resultTagName, "No bearer token found.");
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
      using var activity = ActivitySource.StartActivity();
      
      var tokenResult = await _jwtGenerator.ValidateToken(token);
      if (tokenResult.Failed)
      {
         activity?.AddTag(_resultTagName, "Token validation failed.");
         return input;
      }

      var rawAccountId = tokenResult.Success.RawAccountIdentifier;
      if (rawAccountId is null)
      {
         activity?.AddTag(_resultTagName, "No account identifier found in token.");
         return input;
      }

      if (Guid.TryParse(rawAccountId, out var accountId))
      {
         activity?.AddTag(_resultTagName, "Token validation successful.");
         input.UserProperties.AccountId = accountId;
      }
      
      return input;
   }
   
   private const string _bearer = "Bearer ";
}