
namespace ConsoleManager
{
    public class ConsoleProcessor
    {
        public string GetAddress()
        {
            Console.WriteLine("Enter URL: ");

            var input = Console.ReadLine();

            return input.TrimEnd('/');
        }
    }
}
