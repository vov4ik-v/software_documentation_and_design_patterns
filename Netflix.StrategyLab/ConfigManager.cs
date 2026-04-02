using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netflix.StrategyLab;

public class AppConfig
{
    [JsonPropertyName("DatasetPath")]
    public string DatasetPath { get; set; } = "Data/dataset.xlsx";

    [JsonPropertyName("OutputStrategy")]
    public string OutputStrategy { get; set; } = "Console";

    [JsonPropertyName("OutputPath")]
    public string OutputPath { get; set; } = "Data/output.txt";

    [JsonPropertyName("Kafka")]
    public KafkaConfig Kafka { get; set; } = new();

    [JsonPropertyName("Redis")]
    public RedisConfig Redis { get; set; } = new();
}

public class KafkaConfig
{
    [JsonPropertyName("BootstrapServers")]
    public string BootstrapServers { get; set; } = "localhost:9092";

    [JsonPropertyName("Topic")]
    public string Topic { get; set; } = "netflix-data";
}

public class RedisConfig
{
    [JsonPropertyName("ConnectionString")]
    public string ConnectionString { get; set; } = "localhost:6379";

    [JsonPropertyName("ListKey")]
    public string ListKey { get; set; } = "netflix-data-list";
}

public static class ConfigManager
{
    private static readonly string[] AvailableStrategies = ["Console", "File", "Kafka", "Redis"];

    private static string ProjectRoot => Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
    private static string ConfigPath => Path.GetFullPath(Path.Combine(ProjectRoot, "appsettings.json"));

    public static (string strategy, bool saved) RunSetup()
    {
        Console.WriteLine("=== Netflix.StrategyLab — Configuration ===\n");

        if (!File.Exists(ConfigPath))
        {
            return HandleNoConfig();
        }

        if (!TryParseConfig(out var config))
        {
            return HandleBrokenConfig();
        }

        return HandleValidConfig(config!);
    }

    private static (string strategy, bool saved) HandleNoConfig()
    {
        Console.WriteLine("[!] Config file (appsettings.json) not found.\n");
        Console.WriteLine("  1. Create config file and save settings");
        Console.WriteLine("  2. One-shot run (pick strategy without saving)\n");

        var choice = ReadChoice(1, 2);

        var strategy = PickStrategy();

        if (choice == 1)
        {
            SaveConfig(strategy);
            Console.WriteLine($"\n[OK] Config saved with strategy: {strategy}\n");
            return (strategy, true);
        }

        SaveConfig("Console");
        Console.WriteLine($"\n[OK] Running one-shot with strategy: {strategy}\n");
        return (strategy, false);
    }

    private static (string strategy, bool saved) HandleBrokenConfig()
    {
        Console.WriteLine("[!] Config file found, but it is malformed or has invalid format.\n");
        Console.WriteLine("  The config will be overwritten with default settings.");
        Console.WriteLine("  Please select a strategy:\n");

        var strategy = PickStrategy();
        SaveConfig(strategy);
        Console.WriteLine($"\n[OK] Config overwritten and saved with strategy: {strategy}\n");
        return (strategy, true);
    }

    private static (string strategy, bool saved) HandleValidConfig(AppConfig config)
    {
        Console.WriteLine($"[OK] Config file found. Current strategy: {config.OutputStrategy}\n");
        Console.WriteLine($"  1. Continue with saved strategy ({config.OutputStrategy})");
        Console.WriteLine("  2. Pick a different strategy (and save)");
        Console.WriteLine("  3. Pick a different strategy (one-shot, don't save)\n");

        var choice = ReadChoice(1, 3);

        if (choice == 1)
        {
            return (config.OutputStrategy, true);
        }

        var strategy = PickStrategy();

        if (choice == 2)
        {
            config.OutputStrategy = strategy;
            SaveConfig(strategy);
            Console.WriteLine($"\n[OK] Config updated with strategy: {strategy}\n");
            return (strategy, true);
        }

        Console.WriteLine($"\n[OK] Running one-shot with strategy: {strategy}\n");
        return (strategy, false);
    }

    private static string PickStrategy()
    {
        Console.WriteLine("\n  Available strategies:");
        for (int i = 0; i < AvailableStrategies.Length; i++)
        {
            Console.WriteLine($"    {i + 1}. {AvailableStrategies[i]}");
        }
        Console.Write("\n  Select strategy: ");

        var choice = ReadChoice(1, AvailableStrategies.Length);
        return AvailableStrategies[choice - 1];
    }

    private static int ReadChoice(int min, int max)
    {
        while (true)
        {
            Console.Write("  > ");
            var input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out var num) && num >= min && num <= max)
            {
                return num;
            }

            Console.WriteLine($"  Please enter a number from {min} to {max}.");
        }
    }

    private static void SaveConfig(string strategy)
    {
        var config = new AppConfig { OutputStrategy = strategy };

        if (File.Exists(ConfigPath) && TryParseConfig(out var existing))
        {
            existing!.OutputStrategy = strategy;
            config = existing;
        }

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(ConfigPath, json);
    }

    private static bool TryParseConfig(out AppConfig? config)
    {
        config = null;
        try
        {
            var json = File.ReadAllText(ConfigPath);
            config = JsonSerializer.Deserialize<AppConfig>(json);

            if (config == null) return false;

            var validStrategies = AvailableStrategies.Select(s => s.ToLowerInvariant()).ToArray();
            if (!validStrategies.Contains(config.OutputStrategy.ToLowerInvariant()))
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
