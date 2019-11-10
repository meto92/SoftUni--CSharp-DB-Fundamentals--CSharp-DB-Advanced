using P02_DatabaseFirst.IO.Interfaces;
using System.IO;
using System.Text;

namespace P02_DatabaseFirst.IO
{
    public class FileWriter : IWriter
    {
        private const string FilePath = "result.txt";

        private readonly StringBuilder sb;

        public FileWriter()
        {
            this.sb = new StringBuilder();
        }

        public void Append(string text) => this.sb.Append(text);

        public void AppendLine(string text) => this.sb.AppendLine(text);

        public void Flush()
        {
            File.WriteAllText(FilePath, this.sb.ToString());

            this.sb.Clear();
        }
    }
}