using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.ConsoleOutput
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration connectionConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var configurator = new DependencyConfigurator();

            var builder = configurator.ConfigureHost(connectionConfiguration);

            using var host = builder.Build();

            var consoleProcessor = host.Services.GetRequiredService<ConsoleProcessor>();

            await consoleProcessor.ExecuteAsync();
        }
    }
}