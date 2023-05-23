using Crawler.Application.Interfaces;

namespace Crawler.Application.Wrappers
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