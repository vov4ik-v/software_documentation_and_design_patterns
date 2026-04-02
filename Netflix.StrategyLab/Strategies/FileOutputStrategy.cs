using Microsoft.Extensions.Configuration;

namespace Netflix.StrategyLab.Strategies;

public class FileOutputStrategy(IConfiguration configuration) : IOutputStrategy
{
    private readonly string _filePath = configuration["OutputPath"] ?? "Data/output.txt";

    public void WriteData(IEnumerable<string> data)
    {
        Console.WriteLine($"\n--- File Output Strategy Started (Saving to {_filePath}) ---");
        var dirInfo = new FileInfo(_filePath).Directory;
        if (dirInfo is { Exists: false })
        {
            dirInfo.Create();
        }

        int count = 0;
        using var writer = new StreamWriter(_filePath);
        foreach (var item in data)
        {
            writer.WriteLine(item);
            count++;
        }
        
        Console.WriteLine($"--- File Output Strategy Finished ({count} items saved) ---\n");
    }
}
