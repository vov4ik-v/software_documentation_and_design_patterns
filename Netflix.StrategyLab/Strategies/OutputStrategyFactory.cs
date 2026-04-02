using System;
using Microsoft.Extensions.Configuration;

namespace Netflix.StrategyLab.Strategies;

public static class OutputStrategyFactory
{
    public static IOutputStrategy CreateStrategy(string strategyName, IConfiguration configuration)
    {
        return strategyName.ToLowerInvariant() switch
        {
            "console" => new ConsoleOutputStrategy(),
            "file" => new FileOutputStrategy(configuration),
            "kafka" => new KafkaOutputStrategy(configuration),
            "redis" => new RedisOutputStrategy(configuration),
            _ => throw new ArgumentException($"Unknown strategy: {strategyName}")
        };
    }
}
