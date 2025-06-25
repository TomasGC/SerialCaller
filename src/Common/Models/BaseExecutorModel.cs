using SerialCaller.Libs.Common.Clients;
using SerialCaller.Libs.Common.Logger;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace SerialCaller.Libs.Common.Models;

public abstract class BaseExecutorModel<TRequest, TResponse> : IExecutorModel<TRequest>
    where TRequest : class, IRequest, new()
    where TResponse : class, IResponse, new()
{
    protected string TicketId { get; }

    public string ResultsFilePath { get; }
    public ConcurrentBag<TRequest>? Requests { get; set; } = [];

    public ConcurrentBag<Result<TRequest, TResponse>> Successes { get; set; } = [];
    public ConcurrentBag<Result<TRequest, TResponse>> Failures { get; set; } = [];

    protected BaseExecutorModel(string ticketId, string filename, string specificName)
    {
        TicketId = ticketId;

        if (!string.IsNullOrWhiteSpace(specificName))
        {
            filename = $"{filename}_{specificName}";
        }

        ResultsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @$"Data\{TicketId}\{filename}_Results.json");
    }

    public void LogFiles()
    {
        var resultObject = new
        {
            Successes,
            Failures
        };

        FileLogger.Log(ResultsFilePath, JsonSerializer.Serialize(resultObject, Constants.SERIALIZER_OPTIONS));
    }

    public void DisplayFiles()
    {
        Process.Start(Constants.NOTEPAD, ResultsFilePath);
    }
}
