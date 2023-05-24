
namespace Crawler.ConsoleOutput.Wrappers
{
    public class ConsoleWrapper
    {
        public virtual void Write(string text)
        {
            Console.WriteLine(text);
        }

        public virtual string Read()
        {
            return Console.ReadLine();
        }
    }
}