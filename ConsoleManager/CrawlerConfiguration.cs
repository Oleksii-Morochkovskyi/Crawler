using Crawler.Logic.Interfaces;
using Ninject;

namespace Crawler.ConsoleOutput
{
    public class CrawlerConfiguration
    {
        private readonly IKernel _container = new StandardKernel();

        public ConsoleProcessor ComposeObjects()
        {
            _container.Bind<IConsoleHandler>().To<ConsoleWrapper>();

            return _container.Get<ConsoleProcessor>();
        }
    }
}
