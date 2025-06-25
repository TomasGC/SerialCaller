using SerialCaller.Libs.Common.Clients;
using SerialCaller.Libs.Common.Executors;

namespace SerialCaller.APIs.OfficialJokeApi.GetJoke;

public sealed class GetJokeExecutor(SerialCallerHttpClient httpClient, GetJokeExecutorModel model) : 
    ExecutorBase<GetJokeRequest, GetJokeResponse, GetJokeExecutorModel>(model)
{
    protected override async Task<GetJokeResponse> ComputeSingleAsync(GetJokeRequest? request)
    {
        Console.WriteLine($"Joke Id {request!.Id}");

        return await httpClient.GetAsync<GetJokeResponse>($"jokes/{request.Id}");
    }
}
