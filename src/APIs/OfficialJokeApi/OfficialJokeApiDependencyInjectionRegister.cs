using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerialCaller.Libs.Common.DependencyInjectionRegister;

namespace SerialCaller.APIs.OfficialJokeApi;

public class OfficialJokeApiDependencyInjectionRegister : IProjectDependencyInjectionRegister
{
    public IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSingleton<OfficialJokeApiExecutorFactory>(sp =>
        {
            var baseAddress = configuration.GetValue<string>("OfficialJokeApiSettings:BaseAddress");
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress!)
            };
            return new(new(httpClient));
        });
    }
}
