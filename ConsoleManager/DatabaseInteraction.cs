using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Logic.Models;
using Crawler.UrlRepository.Entities;
using Crawler.UrlRepository.Repositories;

namespace Crawler.ConsoleOutput
{
    public class DatabaseInteraction
    {
        public DatabaseInteraction(FoundUrlRepository foundUrlRep, InitialUrlRepository initialUrlRep)
        {
            _foundUrlRep = foundUrlRep;
            _initialUrlRep = initialUrlRep;
        }

        private readonly FoundUrlRepository _foundUrlRep;
        private readonly InitialUrlRepository _initialUrlRep;

        public async Task AddUrlsAsync(IEnumerable<UrlResponse> urls, string baseUrl)
        {
            await _foundUrlRep.AddFoundUrlsAsync(urls);

            await _initialUrlRep.AddInitialUrlAsync(baseUrl);
        }
    }
}
