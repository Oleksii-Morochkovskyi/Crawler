using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Net;

namespace TestTask
{
    public class ListManager
    {
        public IEnumerable<string> CompareResult(HashSet<string> first, HashSet<string> second)
        {
            IEnumerable<string> result;
            result = first.Except(second);
            return result;
        }
        public void Print(IEnumerable<string> list)
        {
            foreach (string el in list) Console.WriteLine(el);
        }
        public IEnumerable<string> MergeUrls(HashSet<string> first, HashSet<string> second)
        {
            IEnumerable<string> result = first.Union(second);
            return result;
        }
        public void GetResponseTime(IEnumerable<string> urlList)
        {
            Dictionary<string, long> urlResponse = new Dictionary<string, long>();
            Console.WriteLine("URL\t\t\tTiming (ms)");
            HttpClient httpClient = new HttpClient();
            foreach(var el in urlList)
            {
                var timer = Stopwatch.StartNew();
                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(el).Result;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"\nCant access url: {el}\n" + e.Message);
                    timer.Stop();
                    continue;
                }
               
                timer.Stop();
                urlResponse.Add(el, timer.ElapsedMilliseconds);
                
            }
            var sortedDict = urlResponse.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (var el in sortedDict)
            {
                Console.WriteLine($"{el.Key} {el.Value} ms");
            }
        }
    }
}
