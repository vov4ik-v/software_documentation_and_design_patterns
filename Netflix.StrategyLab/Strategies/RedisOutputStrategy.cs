using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Netflix.StrategyLab.Strategies;

public class RedisOutputStrategy : IOutputStrategy
{
    private readonly string _connectionString;
    private readonly string _listKey;

    public RedisOutputStrategy(IConfiguration configuration)
    {
        _connectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
        _listKey = configuration["Redis:ListKey"] ?? "netflix-data-list";
    }

    public void WriteData(IEnumerable<string> data)
    {
        Console.WriteLine($"\n--- Redis Output Strategy Started (ListKey: {_listKey}) ---");

        try
        {
            var redis = ConnectionMultiplexer.Connect(_connectionString);
            var db = redis.GetDatabase();

            int count = 0;
            foreach (var item in data)
            {
                db.ListRightPush(_listKey, item);
                count++;
            }
            Console.WriteLine($"--- Redis Output Strategy Finished ({count} items pushed to Redis list) ---\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Could not write to Redis: {ex.Message}");
            Console.WriteLine("Make sure Redis is running at " + _connectionString);
        }
    }
}
