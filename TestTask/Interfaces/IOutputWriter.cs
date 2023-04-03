namespace Crawler.Logic.Interfaces
{
    public interface IOutputWriter
    {
        void Write(string text);
        string Read();
    }
}
