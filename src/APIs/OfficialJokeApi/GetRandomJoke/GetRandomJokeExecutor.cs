using SerialCaller.Libs.Common.Clients;
using SerialCaller.Libs.Common.Executors;

namespace SerialCaller.APIs.OfficialJokeApi.GetRandomJoke;

public sealed class GetRandomJokeExecutor(SerialCallerHttpClient httpClient, GetRandomJokeExecutorModel model) : 
    ExecutorBase<GetRandomJokeRequest, GetRandomJokeResponse, GetRandomJokeExecutorModel>(model)
{
    protected override async Task<GetRandomJokeResponse> ComputeSingleAsync(GetRandomJokeRequest? request)
    {
        Console.WriteLine($"Random joke");

        return await httpClient.GetAsync<GetRandomJokeResponse>("/jokes/random");
    }
}
