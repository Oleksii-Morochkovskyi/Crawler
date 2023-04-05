﻿using Crawler.Logic.Crawlers;
using Crawler.Logic.Helpers;
using Crawler.Logic.Parsers;
using Crawler.Logic.Services;
using Crawler.Logic.Validators;

namespace Crawler.ConsoleOutput
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuredCrawler = new CrawlerConfiguration();

            var console = configuredCrawler.ComposeObjects();

            await console.ExecuteAsync();
        }
    }
}