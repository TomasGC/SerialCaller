using SerialCaller.App.Scenarios;
using SerialCaller.Libs.Common;
using System.Collections.Concurrent;
using System.Data.Common;

namespace SerialCaller.App.CsvFileParsing;

public class CsvFileParser<TItem>(Func<string[], TItem> method, CsvFileParserModel model) 
    where TItem : class, IItem, new()
{
    private readonly Func<string[], TItem> createRequest = method;
    protected readonly CsvFileParserModel _model = model;

    public IEnumerable<TItem>? ExtractItems()
    {
        var requests = new ConcurrentQueue<TItem>();
        var rows = File.ReadAllLines(_model.CsvFilePath)!;

        Parallel.ForEach(rows.Skip(_model.RowsToSkip), Constants.PARALLEL_OPTIONS, row =>
        {
            string[] columns = row.Split(_model.Delimiter);

            if (ValidateRow(columns))
            {
                var request = createRequest(columns);
                requests.Enqueue(request!);
            }
        });

        return requests;
    }

    public string? GetCellValue(string[] columns, string propertyName)
    {
        if (!_model.ColumnsByPropertyName!.TryGetValue(propertyName, out int value))
        {
            return null;
        }
        var columnValue = columns[value];
        if (string.IsNullOrEmpty(columnValue))
        {
            return null;
        }

        return columnValue.Trim();
    }

    public decimal? GetAmountValue(string[] columns, string propertyName)
    {
        var amount = GetCellValue(columns, propertyName);
        if (string.IsNullOrEmpty(amount))
        {
            return null;
        }

        var culture = amount.Contains(',') ? Constants.CULTURE_FR : Constants.CULTURE_EN;
        return Convert.ToDecimal(amount, culture);
    }

    public bool? GetBoolValue(string[] columns, string propertyName)
    {
        var value = GetCellValue(columns, propertyName);
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        const string trueAsInt = "1";
        const string falseAsInt = "0";

        if (string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase))
        {
            return bool.Parse(value!);
        }
        else if (string.Equals(value, trueAsInt))
        {
            return true;
        }
        else if (string.Equals(value, falseAsInt))
        {
            return false;
        }

        return null;
    }

    private bool ValidateRow(string[] columns)
    {
        if (_model.Filters == null)
        {
            return true;
        }

        bool isValid = true;
        Parallel.ForEach(_model.Filters, Constants.PARALLEL_OPTIONS, (filter, loopState) =>
        {
            if (filter.AuthorizedValues == null)
            {
                loopState.Break();
                return;
            }

            if (!filter.AuthorizedValues.Any(v => columns[filter.Column].Contains(v, StringComparison.OrdinalIgnoreCase)))
            {
                isValid = false;
                loopState.Stop();
            }
        });

        return isValid;
    }
}
