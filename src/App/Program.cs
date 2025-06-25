using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerialCaller.App;
using SerialCaller.App.Tools;

IServiceCollection _serviceCollection;
IServiceProvider _serviceProvider;
IServiceScope _serviceScope;

// No need to modify
async Task Main()
{
    Initialize();
    await _serviceScope.SelectScenario();
}

void Initialize()
{
    _serviceCollection = new ServiceCollection();

    FileHandler.CheckFolder();
    var appsettingsPath = FileHandler.GetFilePath("appsettings.json");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile(appsettingsPath, false, true)
        .Build();

    _serviceCollection.RegisterProjectDependencyInjections(configuration);

    _serviceProvider = _serviceCollection.BuildServiceProvider();
    _serviceScope = _serviceProvider.CreateScope();
}

await Main();