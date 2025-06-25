namespace SerialCaller.APIs.OfficialJokeApi.Models;

public class Joke
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public string? Setup { get; set; }

    public string? PunchLine { get; set; }
}