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
using Microsoft.Extensions.Http.Logging;
using Microsoft.Graph.Models;

namespace Crawler.ConsoleOutput
{
    public class DependencyConfigurator
    {
        // private readonly IKernel _container = new StandardKernel();

        public ConsoleProcessor ComposeObjects()
        {
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(
                services =>
                {
                    services.AddSingleton<ConsoleProcessor>();
                    services.AddScoped<IConsoleHandler, ConsoleWrapper>();
                    services.AddSingleton<UrlHelper>();
                    services.AddSingleton<UrlValidator>();
                    services.AddSingleton<Logic.Crawlers.Crawler>();
                    services.AddScoped<HttpClientService>();
                    services.AddSingleton<ResponseTimeService>();
                    services.AddSingleton<HtmlCrawler>();
                    services.AddSingleton<HtmlParser>();
                    services.AddSingleton<XmlCrawler>();
                    services.AddHttpClient();
                    services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
                });

            using var host = builder.Build();
            return host.Services.GetService<ConsoleProcessor>();
        }
    }
}
