using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Crawler.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectCrawlerDependencies(this IServiceCollection services)
        {
            services.AddSingleton<UrlHelper>();
            services.AddSingleton<UrlValidator>();
            services.AddSingleton<Crawlers.Crawler>();
            services.AddSingleton<HttpClientService>();
            services.AddSingleton<ResponseTimeService>();
            services.AddSingleton<HtmlCrawler>();
            services.AddSingleton<HtmlParser>();
            services.AddSingleton<XmlCrawler>();
            services.AddHttpClient<HttpClientService>();
            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();

            return services;
        }
    }
}
