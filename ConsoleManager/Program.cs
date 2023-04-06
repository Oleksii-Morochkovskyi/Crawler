using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
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
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(
                services =>
                {
                    services.AddSingleton<ConsoleProcessor>();
                    services.AddSingleton<UrlHelper>();
                    services.AddSingleton<UrlValidator>();
                    services.AddSingleton<Logic.Crawlers.Crawler>();
                    services.AddSingleton<HttpClientService>();
                    services.AddSingleton<ResponseTimeService>();
                    services.AddSingleton<HtmlCrawler>();
                    services.AddSingleton<HtmlParser>();
                    services.AddSingleton<XmlCrawler>();
                    services.AddHttpClient<HttpClientService>();
                    services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
                });

            var configurator = new DependencyConfigurator();

            using var host = configurator.ComposeObjects(builder);
            
            var consoleProcessor = host.Services.GetRequiredService<ConsoleProcessor>();

            await consoleProcessor.ExecuteAsync();
        }
    }
}