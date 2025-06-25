using SerialCaller.APIs.OfficialJokeApi;
using SerialCaller.APIs.OfficialJokeApi.GetJoke;
using SerialCaller.Libs.Common;
using static SerialCaller.App.CsvFileParsing.CsvFileParserModel;

namespace SerialCaller.App.Scenarios.Example;

public class UsingFile(OfficialJokeApiExecutorFactory officialJokeApiExecutorFactory) :
        ScenarioBase<Item>(nameof(UsingFile)), IScenario
{
    public static string Description => $"Run {nameof(UsingFile)} scenario";
    public static int Position => 0;

    private GetJokeExecutorModel? _getJokeExecutorModel;
    private readonly bool useJsonFile = false;

    protected override void InitializeItems()
    {
        if (useJsonFile)
        {
            FillFromJsonFile("Example.json");
        }
        else
        {
            var columnsByPropertyName = new Dictionary<string, int>
            {
                { "ID", 0 }
            };
            var filters = new List<Filter>
            {
                new(0, ["0", "1", "3"])
            };
            FillFromCsvFile(
                "Example.csv",
                ',',
                1,
                columnsByPropertyName,
                filters);
        }
    }

    protected override async Task ProcessScenarioAsync()
    {
        _getJokeExecutorModel = new(_ticketId)
        {
            Requests = []
        };
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Parallel.ForEach(_items!, Constants.PARALLEL_OPTIONS, item =>
        {
            _getJokeExecutorModel.Requests.Add(new() { Id = item.Id });
        });

        try
        {
            await GetJokeAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        _getJokeExecutorModel!.LogFiles();

        _getJokeExecutorModel.DisplayFiles();

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine($"Elapsed time: {elapsedMs} ms");
    }

    protected override void DisplayItem(Item item)
    {
        Console.WriteLine($"Joke ID: {item.Id}");
    }

    protected override Item MapEntryItem(string[] columns)
    {
        return new()
        {
            Id = Convert.ToInt32(_csvFileParser!.GetCellValue(columns, "ID"))
        };
    }


    private async Task GetJokeAsync()
    {
        await officialJokeApiExecutorFactory.GetJoke(_getJokeExecutorModel!).ExecuteListAsync(new(true));

        if (_getJokeExecutorModel!.Successes == null || _getJokeExecutorModel.Successes.IsEmpty)
        {
            throw new ArgumentNullException(
                nameof(_getJokeExecutorModel.Successes).ToString(),
                "Get random joke did not work for these items.");
        }
    }
}
