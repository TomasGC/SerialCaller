using SerialCaller.Libs.Common.Clients;

namespace SerialCaller.APIs.OfficialJokeApi.GetJoke;

public class GetJokeRequest : IRequest
{
    public int Id { get; set; }
}