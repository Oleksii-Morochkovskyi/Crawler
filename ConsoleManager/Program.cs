using Crawler.ConsoleOutput.DependencyInjection;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

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