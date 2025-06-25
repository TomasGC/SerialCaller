using SerialCaller.App.CsvFileParsing;
using SerialCaller.App.Tools;
using SerialCaller.Libs.Common;
using static SerialCaller.App.CsvFileParsing.CsvFileParserModel;

namespace SerialCaller.App.Scenarios;
public abstract class ScenarioBase(string scenarioName)
{
    protected string _ticketId = string.Empty;

    protected readonly string _scenarioName = scenarioName;

    public virtual async Task ProcessAsync(string ticketId)
    {
        _ticketId = ticketId;

        Console.Write($"If you want to continue please write 'yes/YES': ");
        var answer = Console.ReadLine();
        if (!string.Equals(answer, "yes", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await ProcessScenarioAsync();
    }

    protected abstract Task ProcessScenarioAsync();
}

public abstract class ScenarioBase<TItem>(string scenarioName) : ScenarioBase(scenarioName)
        where TItem : class, IItem, new()
{
    protected IEnumerable<TItem>? _items;
    protected CsvFileParser<TItem>? _csvFileParser;

    public override async Task ProcessAsync(string ticketId)
    {
        InitializeItems();
        if (_items == null || !_items.Any())
        {
            throw new ArgumentNullException(nameof(IEnumerable<IItem>).ToString(), "No items found for this request.");
        }
        Console.Write($"Do you want to display all {_items.Count()} items found ? 'yes/y/YES/Y' or 'no/n/NO/N' (default yes): ");
        var toDisplay = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(toDisplay)
            || string.Equals(toDisplay, "yes", StringComparison.OrdinalIgnoreCase)
            || string.Equals(toDisplay, "y", StringComparison.OrdinalIgnoreCase))
        {
            Parallel.ForEach(_items, Constants.PARALLEL_OPTIONS, DisplayItem);
        }

        await base.ProcessAsync(ticketId);
    }

    protected abstract void InitializeItems();

    protected abstract void DisplayItem(TItem item);

    protected abstract TItem MapEntryItem(string[] columns);

    protected void FillFromCsvFile(
        string filename,
        char delimiter,
        int rowsToSkip,
        Dictionary<string, int> columnsByPropertyName,
        IEnumerable<Filter>? filters = null)
    {
        var filePath = FileHandler.GetFilePath(filename);
        var model = new CsvFileParserModel(_ticketId, filePath)
        {
            Delimiter = delimiter,
            RowsToSkip = rowsToSkip,
            ColumnsByPropertyName = columnsByPropertyName,
            Filters = filters
        };
        _csvFileParser = new CsvFileParser<TItem>(MapEntryItem, model);
        _items = _csvFileParser.ExtractItems();
    }

    protected void FillFromJsonFile(string filename)
    {
        var filePath = FileHandler.GetFilePath(filename);
        var json = File.ReadAllText(filePath);
        _items = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TItem>>(json);
    }
}
