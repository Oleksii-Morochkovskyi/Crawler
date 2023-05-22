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
        public static IServiceCollection AddCrawlerDependencies(this IServiceCollection services)
        {
            services.AddScoped<UrlHelper>();
            services.AddScoped<UrlValidator>();
            services.AddScoped<Crawlers.Crawler>();
            services.AddScoped<HttpClientService>();
            services.AddScoped<ResponseTimeService>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<HtmlParser>();
            services.AddScoped<XmlCrawler>();
            services.AddHttpClient<HttpClientService>();
            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();

            return services;
        }
    }
}
