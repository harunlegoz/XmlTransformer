using System.Collections.Generic;
using System.Text;

namespace Rahl.XmlTransformer
{
    public class TransformerConfiguration
    {
        public TransformerConfiguration()
        {
            DefaultBufferSize = 1000000;
            DefaultEncoding = Encoding.UTF8;
        }

        public IEnumerable<Map> Mapping { get; set; }
        public int DefaultBufferSize { get; set; }
        public Encoding DefaultEncoding { get; set; }
    }
}
