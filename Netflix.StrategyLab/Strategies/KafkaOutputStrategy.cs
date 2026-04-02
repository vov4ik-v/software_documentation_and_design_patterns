using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;

namespace Netflix.StrategyLab.Strategies;

public class KafkaOutputStrategy(IConfiguration configuration) : IOutputStrategy
{
    private readonly string _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
    private readonly string _topic = configuration["Kafka:Topic"] ?? "netflix-data";

    public void WriteData(IEnumerable<string> data)
    {
        Console.WriteLine($"\n--- Kafka Output Strategy Started (Topic: {_topic}) ---");

        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        try
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            int count = 0;
            foreach (var item in data)
            {
                producer.Produce(_topic, new Message<Null, string> { Value = item });
                count++;
            }
            producer.Flush(TimeSpan.FromSeconds(10));
            Console.WriteLine($"--- Kafka Output Strategy Finished ({count} messages produced) ---\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Could not write to Kafka: {ex.Message}");
            Console.WriteLine("Make sure Kafka broker is running at " + _bootstrapServers);
        }
    }
}
