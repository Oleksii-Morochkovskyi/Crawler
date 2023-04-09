using Crawler.ConsoleOutput.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.ConsoleOutput
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configurator = new DependencyConfigurator();

            var builder = configurator.ComposeObjects();

            using var host = builder.Build();

            var consoleProcessor = host.Services.GetRequiredService<ConsoleProcessor>();

            await consoleProcessor.ExecuteAsync();
        }
    }
}