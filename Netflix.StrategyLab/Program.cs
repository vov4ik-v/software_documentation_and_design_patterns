using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Netflix.StrategyLab.Strategies;

namespace Netflix.StrategyLab;

class Program
{
    static void Main(string[] args)
    {
        var (strategyName, saved) = ConfigManager.RunSetup();

        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectRoot)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string datasetPath = configuration["DatasetPath"] ?? "Data/dataset.xlsx";

        IOutputStrategy strategy;
        try
        {
            strategy = OutputStrategyFactory.CreateStrategy(strategyName, configuration);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Strategy Error] {ex.Message}");
            return;
        }

        var reader = new DatasetReader();
        Console.WriteLine($"Reading dataset from: {datasetPath} ...");

        IEnumerable<string> rows;
        try
        {
            rows = reader.ReadDataset(datasetPath);
            Console.WriteLine($"Successfully read {rows.Count()} lines from the dataset.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Read Error] {ex.Message}");
            return;
        }

        strategy.WriteData(rows);

        Console.WriteLine("=== Processing Completed ===");
    }
}
