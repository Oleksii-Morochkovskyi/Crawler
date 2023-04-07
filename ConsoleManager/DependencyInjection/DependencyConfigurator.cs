using Crawler.Db.DependencyInjection;
using Crawler.Db.Repository;
using Crawler.Logic.Crawlers;
using Crawler.Logic.DependencyInjection;
using Crawler.Logic.Helpers;
using Crawler.Logic.Interfaces;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace Crawler.ConsoleOutput.DependencyInjection
{
    public class DependencyConfigurator
    {
        public IHostBuilder ComposeObjects()
        {
            var builder = Host.CreateDefaultBuilder();

            var consoleOutputDI = new ConsoleOutputDI();
            var crawlerLogicDI = new CrawlerLogicDI();
            var databaseDI = new DatabaseDI();

            builder = consoleOutputDI.InjectDependencies(builder);
            builder = crawlerLogicDI.InjectDependencies(builder);
            builder = databaseDI.InjectDependencies(builder);

            return builder;
        }
    }
}
