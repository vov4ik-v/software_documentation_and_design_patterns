namespace Netflix.StrategyLab.Strategies;

public class ConsoleOutputStrategy : IOutputStrategy
{
    public void WriteData(IEnumerable<string> data)
    {
        Console.WriteLine("\n--- Console Output Strategy Started ---");
        int count = 0;
        foreach (var item in data)
        {
            Console.WriteLine(item);
            count++;
        }
        Console.WriteLine($"--- Console Output Strategy Finished ({count} items printed) ---\n");
    }
}
