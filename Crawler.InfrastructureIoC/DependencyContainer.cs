using Crawler.Application.Crawlers.Interfaces;
using Crawler.Application.Helpers;
using Crawler.Application.Interfaces;
using Crawler.Application.Parsers.Interfaces;
using Crawler.Application.Services;
using Crawler.Application.Services.Interfaces;
using Crawler.Application.Validators;
using Crawler.Application.Wrappers;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Persistence;
using Crawler.Persistence.Repositories;
using Crawler.Utils.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Crawler.InfrastructureIoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<UrlHelper>();
            services.AddScoped<UrlValidator>();
            services.AddScoped<ICrawler, Logic.Crawlers.Crawler>();
            services.AddScoped<HttpClientService>();
            services.AddScoped<IResponseTimeService, ResponseTimeService>();
            services.AddScoped<IHtmlCrawler, HtmlCrawler>();
            services.AddScoped<IHtmlParser, HtmlParser>();
            services.AddScoped<IXmlCrawler, XmlCrawler>();
            services.AddHttpClient<HttpClientService>();
            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            services.AddScoped<DatabaseInteractionService>();
            services.AddScoped<ModelMapper>();
            services.AddScoped<IConsoleHandler, ConsoleWrapper>();

            return services;
        }

        public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UrlDatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IInitialUrlRepository, InitialUrlRepository>();
            services.AddScoped<IFoundUrlRepository, FoundUrlRepository>();

            return services;
        }
    }
}