using System;

using System.Xml;


namespace TestTask
{
    public class XmlParser
    {
        private string Adress { get; set; }


        public XmlParser(string adress)
        {
            Adress = adress;
        }

        public HashSet<string> ParseUrl() //retrieves all url's from sitemap.xml
        {
            HashSet<string> url = new HashSet<string>();
            try
            {
                using (var reader = XmlReader.Create(Adress))
                {


                    while (reader.Read())
                    {
                        if (reader.Name == "loc")
                        {
                            var absUrl = GetAbsoluteUrlString(Adress, reader.ReadInnerXml());
                            url.Add(absUrl);
                        }
                    }
                    return url;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

            return url;

        }
        private string GetAbsoluteUrlString(string baseUrl, string url) //gets absolute url if it is relative
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);
            return uri.ToString();
        }
    }


}
