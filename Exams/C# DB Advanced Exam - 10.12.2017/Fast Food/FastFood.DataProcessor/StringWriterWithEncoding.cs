using System.IO;
using System.Text;

namespace FastFood.DataProcessor
{
    public class StringWriterWithEncoding : StringWriter
    {
        private Encoding encoding;

        public StringWriterWithEncoding()
        {
            this.encoding = Encoding.UTF8;
        }

        public override Encoding Encoding => this.encoding;
    }
}