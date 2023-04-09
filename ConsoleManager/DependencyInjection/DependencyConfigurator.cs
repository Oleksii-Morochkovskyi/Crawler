using Crawler.Db.DependencyInjection;
using Crawler.Logic.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            builder.ConfigureLogging(logging => logging.ClearProviders());

            return builder;
        }
    }
}
