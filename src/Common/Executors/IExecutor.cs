namespace SerialCaller.Libs.Common.Executors;

public interface IExecutor
{
    public Task ExecuteSingleAsync(ExecutionOptions executionOptions);

    public Task ExecuteListAsync(ExecutionOptions executionOptions);
}
