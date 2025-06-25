namespace SerialCaller.Libs.Common.Models;

public sealed class Result<TRequest, TResponse>(TRequest? request, TResponse? response)
    where TRequest : class, new()
    where TResponse : class, new()
{
    public TRequest? Request { get; set; } = request;
    public TResponse? Response { get; set; } = response;
}
