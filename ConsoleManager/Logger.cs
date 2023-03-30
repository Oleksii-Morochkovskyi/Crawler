using Crawler.Logic.Interfaces;

namespace ConsoleOutput
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