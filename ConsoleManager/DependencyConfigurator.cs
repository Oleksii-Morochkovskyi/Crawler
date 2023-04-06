using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Crawler.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace Crawler.ConsoleOutput
{
    public class DependencyConfigurator
    {
        public IHost ComposeObjects(IHostBuilder builder)
        {
            builder.ConfigureServices(
                services =>
                {
                    services.AddScoped<IConsoleHandler, ConsoleWrapper>();
                    services.AddScoped<IRepository, DbHandler>();
                });

            var host = builder.Build();
            return host;
        }
    }
}
