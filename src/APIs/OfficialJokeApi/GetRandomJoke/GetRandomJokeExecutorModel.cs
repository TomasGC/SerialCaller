namespace SerialCaller.APIs.OfficialJokeApi.GetRandomJoke;

public sealed class GetRandomJokeExecutorModel(string ticketId, string specificName = "") : 
    BaseOfficialJokeApiExecutorModel<GetRandomJokeRequest, GetRandomJokeResponse>(ticketId, "OfficialJokeApi_GetRandomJoke", specificName)
{
}
