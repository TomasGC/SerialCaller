using SerialCaller.APIs.OfficialJokeApi;
using SerialCaller.APIs.OfficialJokeApi.GetJoke;
using SerialCaller.APIs.OfficialJokeApi.GetRandomJoke;

namespace SerialCaller.App.Scenarios.Example2;

public class ChainCalls(OfficialJokeApiExecutorFactory officialJokeApiExecutorFactory) :
        ScenarioBase(nameof(ChainCalls)), IScenario
{
    public static string Description => $"Run {nameof(ChainCalls)} scenario";
    public static int Position => 1;

    private GetRandomJokeExecutorModel? _getRandomJokeExecutorModel;
    private GetJokeExecutorModel? _getJokeExecutorModel;

    protected override async Task ProcessScenarioAsync()
    {     
        _getRandomJokeExecutorModel = new(_ticketId);
        _getJokeExecutorModel = new(_ticketId);

        var watch = System.Diagnostics.Stopwatch.StartNew();
        await Parallel.ForAsync(0, 5, async (i, ct) =>
        {
            try
            {
                var randomJoke = await GetRandomJokeAsync();
                if (_getRandomJokeExecutorModel.Failures.IsEmpty && !_getRandomJokeExecutorModel.Successes.IsEmpty)
                {
                    await GetJokeAsync(randomJoke);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        });

        _getJokeExecutorModel!.LogFiles();
        _getRandomJokeExecutorModel!.LogFiles();

        _getJokeExecutorModel.DisplayFiles();
        _getRandomJokeExecutorModel.DisplayFiles();

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine($"Elapsed time: {elapsedMs} ms");
    }

    private async Task<GetRandomJokeResponse> GetRandomJokeAsync()
    {
        var response = await officialJokeApiExecutorFactory.GetRandomJoke(_getRandomJokeExecutorModel!).ExecuteSingleAsync(new(), null);

        if (response == null || _getRandomJokeExecutorModel!.Successes == null || _getRandomJokeExecutorModel.Successes.IsEmpty)
        {
            throw new ArgumentNullException(
                nameof(_getRandomJokeExecutorModel.Successes).ToString(),
                "Get random joke did not work for these items.");
        }

        return response;
    }


    private async Task GetJokeAsync(GetRandomJokeResponse randomJoke)
    {
        var getJokeRequest = new GetJokeRequest
        {
            Id = randomJoke.Id
        };

        await officialJokeApiExecutorFactory.GetJoke(_getJokeExecutorModel!).ExecuteSingleAsync(new(), getJokeRequest);

        if (_getJokeExecutorModel!.Successes == null || _getJokeExecutorModel.Successes.IsEmpty)
        {
            throw new ArgumentNullException(
                nameof(_getJokeExecutorModel.Successes).ToString(),
                $"Get joke with id {randomJoke!.Id} did not work for these items.");
        }
    }
}
