using Crawler.Application.Crawlers.Interfaces;
using Crawler.Application.Interfaces;
using Crawler.Application.Services;
using Crawler.Application.Wrappers;
using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;
using Crawler.Persistence;
using Crawler.Persistence.Repositories;
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
            services.AddScoped<ResponseTimeService>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<HtmlParser>();
            services.AddScoped<XmlCrawler>();
            services.AddHttpClient<HttpClientService>();
            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            services.AddScoped<DatabaseInteractionService>();
            services.AddScoped<ConsoleWrapper>();
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