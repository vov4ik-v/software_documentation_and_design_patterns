namespace Netflix.StrategyLab.Strategies;

public interface IOutputStrategy
{
    void WriteData(IEnumerable<string> data);
}
