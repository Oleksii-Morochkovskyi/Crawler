using Crawler.Logic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawler.ConsoleOutput.DependencyInjection
{
    public class ConsoleOutputDI
    {
        public IHostBuilder InjectDependencies(IHostBuilder builder)
        {
            builder.ConfigureServices(
                services =>
                {
                    services.AddSingleton<ConsoleProcessor>();
                    services.AddScoped<IConsoleHandler, ConsoleWrapper>();
                });

            return builder;
        }
    }
}
