using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Logic.Interfaces
{
    public interface IHttpClient
    { 
        Task<string> GetStringAsync(string url);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
