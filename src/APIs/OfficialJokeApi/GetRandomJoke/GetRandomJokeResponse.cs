using SerialCaller.APIs.OfficialJokeApi.Models;
using SerialCaller.Libs.Common.Clients;

namespace SerialCaller.APIs.OfficialJokeApi.GetRandomJoke;

public class GetRandomJokeResponse : Joke, IResponse
{
    public string? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public bool Success => ErrorCode == null;
}