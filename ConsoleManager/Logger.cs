using Crawler.Logic.Interfaces;

namespace ConsoleManager
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