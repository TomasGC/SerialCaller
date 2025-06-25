namespace SerialCaller.Libs.Common.Executors;

public class ExecutionOptions(bool isParallelismRequired = false)
{
    public ParallelOptions ParallelOptions { get; set; } = Constants.PARALLEL_OPTIONS;

    public bool IsParallelismRequired { get; set; } = isParallelismRequired;

    public bool IsFirstExecution { get; set; } = true;
}
