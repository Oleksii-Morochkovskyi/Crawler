using Crawler.Logic.Interfaces;

namespace ConsoleOutput.Wrappers
{
    public class ConsoleWrapper : IOutputWriter
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