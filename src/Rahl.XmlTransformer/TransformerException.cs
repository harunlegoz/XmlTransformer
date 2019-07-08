using System;

namespace Rahl.XmlTransformer
{
    public class TransformerException : Exception
    {
        public TransformerException(string message, Exception ex = null)
            : base(message, ex)
        {
            
        }
    }
}
