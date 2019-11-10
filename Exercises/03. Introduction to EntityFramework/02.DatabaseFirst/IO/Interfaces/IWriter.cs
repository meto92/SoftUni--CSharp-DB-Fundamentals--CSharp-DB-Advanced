namespace P02_DatabaseFirst.IO.Interfaces
{
    public interface IWriter
    {
        void Append(string text);

        void AppendLine(string text);

        void Flush();
    }
}