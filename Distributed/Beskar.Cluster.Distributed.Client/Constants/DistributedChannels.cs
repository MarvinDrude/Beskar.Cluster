using StackExchange.Redis;

namespace Beskar.Cluster.Distributed.Client.Constants;

public static class DistributedChannels
{
   public const string RefreshSystemConfig = "RefreshSystemConfig";
   public static readonly RedisChannel RefreshSystemConfigChannel = new (RefreshSystemConfig, RedisChannel.PatternMode.Literal);
}