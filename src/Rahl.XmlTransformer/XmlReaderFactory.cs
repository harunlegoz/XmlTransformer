using System.IO;
using System.Xml;

namespace Rahl.XmlTransformer
{
    public static class XmlReaderFactory
    {
        public static XmlReader Create(TextReader reader)
        {
            var settings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse,
                Async = true,
                IgnoreWhitespace = true
            };

            var xmlReader = XmlReader.Create(reader, settings);

            return xmlReader;
        }
    }
}
