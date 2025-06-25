using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SerialCaller.Libs.Common.DependencyInjectionRegister;

public interface IProjectDependencyInjectionRegister
{
    /// <summary>
    /// Configure the app by registering and configuring the wanted project dependency injections.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration from the appsettings.</param>
    /// <returns></returns>
    public IServiceCollection Configure(IServiceCollection services, IConfiguration configuration);
}
