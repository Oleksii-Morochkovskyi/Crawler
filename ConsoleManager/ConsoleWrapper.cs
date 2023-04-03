using Crawler.Logic.Interfaces;

namespace Crawler.ConsoleOutput
{
    public class ConsoleWrapper : IOHandler
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