using Microsoft.Extensions.DependencyInjection;
using SerialCaller.App.Scenarios;
using System.Diagnostics;
using System.Reflection;

namespace SerialCaller.App;

public static class ProgramMenu
{
    public static async Task SelectScenario(this IServiceScope serviceScope)
    {
        var sw = new Stopwatch();
        sw.Start();

        var message = "Which scenario do you want ?";

        var scenarios = typeof(IScenario).
        GetTypeInfo().Assembly.DefinedTypes
        .Where(t => typeof(IScenario).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass)
        .Select(p => p.AsType())
        .OrderBy(p => p.GetProperty("Position", BindingFlags.Public | BindingFlags.Static)!.GetValue(p));


        for (int i = 0; i < scenarios.Count(); ++i)
        {
            var scenario = scenarios.ElementAt(i);
            var description = scenario.GetProperty("Description", BindingFlags.Public | BindingFlags.Static)!.GetValue(scenario);
            message = @$"{message}
- {i}: {description}";
        }
        message = @$"{message}
- r/R: Return
- e/E: Exit";

        string? input = null;
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
        }

        if (string.Equals(input, "r", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Going back to previous menu.");
            return;
        }
        else if (string.Equals(input, "e", StringComparison.OrdinalIgnoreCase))
        {
            sw.Stop();
            Console.WriteLine($"Work done in {sw.Elapsed}");
            Environment.Exit(0);
        }
        else
        {
            var index = int.Parse(input);
            if (index >= 0 && index < scenarios.Count())
            {
                var scenarioType = scenarios.ElementAt(index);

                var scenarioInstance = serviceScope.ServiceProvider.GetRequiredService(scenarioType);

                if (scenarioInstance is IScenario scenario)
                {
                    await scenario.ProcessAsync(GetWorkItemId());
                }
                else
                {
                    Console.WriteLine("The requested scenario does not implement IScenario");
                }
            }
            else
            {
                Console.WriteLine($"No scenario matches this index: {index}.");
            }
        }

        sw.Stop();
        Console.WriteLine($"Work done in {sw.Elapsed}");

        await serviceScope.SelectScenario();
    }
    private static string GetWorkItemId()
    {
        string? ticketId = null;
        while (string.IsNullOrWhiteSpace(ticketId))
        {
            Console.Write("Please provide the workItemId: ");
            ticketId = Console.ReadLine();
        }

        return ticketId;
    }
}
