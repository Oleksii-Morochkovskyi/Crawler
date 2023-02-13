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

                            url.Add(reader.ReadInnerXml());
                        }
                    }
                    return url;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                
            }

            return url;

        }
    }
   

}
