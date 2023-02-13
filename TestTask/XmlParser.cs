using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace TestTask
{
    public class XmlParser
    {
        private string Adress { get; set; }
        

        public XmlParser(string adress)
        {
            Adress = adress;
        }

        public HashSet<string> ParseUrl()
        {
            using (var reader = XmlReader.Create(Adress))
            {

                HashSet<string> url = new HashSet<string>();
                while (reader.Read())
                {
                    if (reader.Name == "loc")
                    {

                        url.Add(reader.ReadInnerXml());
                    }
                }
                return url;
            }


            

        }
    }
   

}
