using SerialCaller.Libs.Common.Clients;
using SerialCaller.Libs.Common.Models;

namespace SerialCaller.APIs.OfficialJokeApi;

/// <summary>
/// This one is optional, you could have the ExecutorModels directly inherit from ExecutorModelBase.
/// It is necessary if you have specific data needed for the API you call such as an API Key or ID & Password or any other thing.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="ticketId"></param>
/// <param name="resultFilename"></param>
/// <param name="specificName"></param>
public abstract class BaseOfficialJokeApiExecutorModel<TRequest, TResponse>(
    string ticketId,
    string resultFilename,
    string specificName) : 
        BaseExecutorModel<TRequest, TResponse>(ticketId, resultFilename, specificName)
            where TRequest : class, IRequest, new()
            where TResponse : class, IResponse, new()
{
}
