using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace TestTask
{
    public class HtmlParser
    {
        private string BaseAdress { get; set; }
        private HashSet <string> UrlList;
        private HashSet<string> CheckedUrl;
        public HtmlParser(string adress)
        {
            BaseAdress = adress;
            UrlList = new HashSet<string>();
            CheckedUrl = new HashSet<string>();
        }

        public HtmlDocument GetHtml(string adress)
        {
                var httpclient = new HttpClient();
                var Html = httpclient.GetStringAsync(adress).Result;
                HtmlDocument HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(Html);
                return HtmlDoc;                    
        }
        public bool CheckUrl(string adress)
        {
            
                if (adress.Contains("http") && !adress.Contains("#"))
                    return true;
                else return false;  
        }
        public HashSet<string> ParseUrl(string adress)
        {
                CheckedUrl.Add(adress);
                var html = GetHtml(adress);
                HtmlNodeCollection nodes = html.DocumentNode.SelectNodes("//a[@href]");
                foreach (var n in nodes)
                {
                    string href = n.Attributes["href"].Value;
                    try
                    {
                        var absUrl = GetAbsoluteUrlString(BaseAdress, href);
                        if (!UrlList.Contains(absUrl) && CheckUrl(absUrl))
                        {
                            UrlList.Add(absUrl);
                            if (!CheckedUrl.Contains(absUrl) && absUrl.Contains(BaseAdress))
                            {
                                ParseUrl(absUrl);
                            }
                            else
                            {
                                CheckedUrl.Add(absUrl);

                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine($"Cant't open url {GetAbsoluteUrlString(BaseAdress, href)}");
                    }
                }
            return UrlList;
        }
        private string GetAbsoluteUrlString(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);
            return uri.ToString();
        }
        
    }
}
