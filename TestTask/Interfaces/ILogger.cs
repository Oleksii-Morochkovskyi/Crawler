namespace Crawler.Logic.Interfaces
{
    public interface ILogger
    {
        void Write(string text);
        string Read();
    }
}
