using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace Netflix.StrategyLab;

public class DatasetReader
{
    static DatasetReader()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    public IEnumerable<string> ReadDataset(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Даний датасет не знайдено за шляхом: {filePath}");
        }

        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (extension == ".xlsx" || extension == ".xls")
        {
            return ReadExcel(filePath);
        }
        
        return File.ReadLines(filePath);
    }

    private IEnumerable<string> ReadExcel(string filePath)
    {
        var resultList = new List<string>();

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read())
                {
                    var values = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var val = reader.GetValue(i)?.ToString() ?? "";
                        if (val.Contains(",") || val.Contains("\"") || val.Contains("\n"))
                        {
                            val = $"\"{val.Replace("\"", "\"\"")}\"";
                        }
                        values.Add(val);
                    }
                    resultList.Add(string.Join(",", values));
                }
            }
        }

        return resultList;
    }
}
