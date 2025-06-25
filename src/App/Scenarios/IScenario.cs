namespace SerialCaller.App.Scenarios
{
    public interface IScenario
    {
        public static abstract string Description { get; }
        public static abstract int Position { get; }

        public Task ProcessAsync(string ticketId);
    }
}
