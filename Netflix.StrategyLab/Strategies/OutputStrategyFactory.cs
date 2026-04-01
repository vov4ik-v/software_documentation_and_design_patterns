using System;
using Microsoft.Extensions.Configuration;

namespace Netflix.StrategyLab.Strategies;

public static class OutputStrategyFactory
{
    public static IOutputStrategy CreateStrategy(IConfiguration configuration)
    {
        var strategyName = configuration["OutputStrategy"] ?? "Console";

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
