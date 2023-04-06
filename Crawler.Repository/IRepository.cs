using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Logic.Models;

namespace Crawler.Repository
{
    public interface IRepository
    {
        void AddElements(IEnumerable<UrlResponse> urls);
        Task SaveChanges();
    }
}
