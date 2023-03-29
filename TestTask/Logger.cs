using Crawler.Logic.Interfaces;

namespace Crawler.Logic
{
    public class Logger:ILogger
    {
        public void Write(string text)
        {
            Console.WriteLine(text);
        }

        public string Read()
        {
            return Console.ReadLine();
        }
    }
}