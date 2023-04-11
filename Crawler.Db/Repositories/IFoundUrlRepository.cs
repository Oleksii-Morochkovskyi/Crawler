using Crawler.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.UrlRepository.Repositories
{
    public interface IFoundUrlRepository
    {
        Task AddFoundUrlsAsync(IEnumerable<UrlResponse> urls);
    }
}
