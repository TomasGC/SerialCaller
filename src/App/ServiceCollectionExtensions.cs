using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerialCaller.App.Scenarios;
using SerialCaller.Libs.Common;
using SerialCaller.Libs.Common.DependencyInjectionRegister;
using System.Reflection;

namespace SerialCaller.App;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterProjectDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        var interfaceType = typeof(IProjectDependencyInjectionRegister);
        var assemblyNames = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().Where(n => n.Name!.Contains("SerialCaller"));

        Parallel.ForEach(assemblyNames!, Constants.PARALLEL_OPTIONS, assemblyName =>
        {
            var assembly = AppDomain.CurrentDomain.Load(assemblyName);
            var implementation = assembly.GetTypes().SingleOrDefault(type => type.IsClass && interfaceType.IsAssignableFrom(type));
            if (implementation != null)
            {
                var dependencyInjectionRegister = (IProjectDependencyInjectionRegister)Activator.CreateInstance(implementation)!;
                dependencyInjectionRegister?.Configure(services, configuration);
            }
        });

        return services.ConfigureScenarios();
    }

    private static IServiceCollection ConfigureScenarios(this IServiceCollection services)
    {
        var scenarios = typeof(IScenario).
            GetTypeInfo().Assembly.DefinedTypes
            .Where(t => typeof(IScenario).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass)
            .Select(p => p.AsType())
            .OrderBy(p => p.GetProperty("Position", BindingFlags.Public | BindingFlags.Static)!.GetValue(p));


        Parallel.ForEach(scenarios, Constants.PARALLEL_OPTIONS, scenario =>
        {
            services.AddSingleton(scenario);
        });

        return services;
    }
}
