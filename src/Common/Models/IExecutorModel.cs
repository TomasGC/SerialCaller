using System.Collections.Concurrent;

namespace SerialCaller.Libs.Common.Models;

public interface IExecutorModel<TRequest>
    where TRequest : class, new()
{
    public ConcurrentBag<TRequest>? Requests { get; set; }
}
