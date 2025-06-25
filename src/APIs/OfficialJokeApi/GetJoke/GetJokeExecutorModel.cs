namespace SerialCaller.APIs.OfficialJokeApi.GetJoke;

public sealed class GetJokeExecutorModel(string ticketId, string specificName = "") : 
    BaseOfficialJokeApiExecutorModel<GetJokeRequest, GetJokeResponse>(ticketId, "OfficialJokeApi_GetJoke", specificName)
{
}
