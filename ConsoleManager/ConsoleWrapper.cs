using Crawler.Logic.Interfaces;

namespace Crawler.ConsoleOutput
{
    public class ConsoleWrapper : IConsoleHandler
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