using System;
using System.IO;
using System.Threading.Tasks;

namespace Rahl.XmlTransformer
{
    public interface IXmlTransformer
    {
        TransformerConfiguration Configuration { get; }
        Task<string> TransformAsync(string xml);
        Task TransformAsync(Stream inputStream, Stream outputStream);
        Task TransformAsync(TextReader reader, TextWriter writer);
    }
}
