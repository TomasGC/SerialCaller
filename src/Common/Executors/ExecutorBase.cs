using SerialCaller.Libs.Common.Clients;
using SerialCaller.Libs.Common.Models;

namespace SerialCaller.Libs.Common.Executors;

public abstract class ExecutorBase<TRequest, TResponse, TExecutorModel>(TExecutorModel model)
    where TRequest : class, IRequest, new()
    where TResponse : class, IResponse, new()
    where TExecutorModel : ExecutorModelBase<TRequest, TResponse>
{
    protected TExecutorModel Model { get; } = model;

    public async Task<TResponse?> ExecuteSingleAsync(ExecutionOptions executionOptions, TRequest? request)
    {
        var isFirstExecution = executionOptions.IsFirstExecution;
        if (isFirstExecution)
        {
            executionOptions.IsFirstExecution = false;
            Console.WriteLine(GetType().Name);
        }

        if (request != null)
        {
            Model!.Requests!.Add(request); // We record it for the logs. 
        }

        var response = await ComputeSingleAsync(request);

        if (response != null && response.Success)
        {
            var success = new Result<TRequest, TResponse>(request, response);
            Model.Successes.Add(success);
        }
        else
        {
            var failure = new Result<TRequest, TResponse>(request, response);
            Model.Failures.Add(failure);
        }

        return response;
    }

    public async Task ExecuteListAsync(ExecutionOptions executionOptions)
    {
        if (Model!.Requests == null || !Model.Requests.Any())
        {
            throw new ArgumentException("You can't use ExecuteListAsync if Model.Request is empty.");
        }

        if (executionOptions.IsParallelismRequired)
        {
            await Parallel.ForEachAsync(Model!.Requests, executionOptions.ParallelOptions, async (request, ct) =>
            {
                await ExecuteSingleAsync(executionOptions, request);
            });
        }
        else
        {
            foreach (var request in Model!.Requests)
            {
                await ExecuteSingleAsync(executionOptions, request);
            }
        }
    }

    protected abstract Task<TResponse> ComputeSingleAsync(TRequest? request);
}
