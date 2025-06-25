using SerialCaller.APIs.OfficialJokeApi.GetJoke;
using SerialCaller.APIs.OfficialJokeApi.GetRandomJoke;
using SerialCaller.Libs.Common.Clients;

namespace SerialCaller.APIs.OfficialJokeApi;

public class OfficialJokeApiExecutorFactory(SerialCallerHttpClient httpClient)
{
    public GetJokeExecutor GetJoke(GetJokeExecutorModel model) =>  new(httpClient, model);
    public GetRandomJokeExecutor GetRandomJoke(GetRandomJokeExecutorModel model) =>  new(httpClient, model);
}
