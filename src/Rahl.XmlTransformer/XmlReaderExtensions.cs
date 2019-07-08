using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Rahl.XmlTransformer
{
    public static class XmlReaderExtensions
    {
        public static async Task<bool> ReadThenLookUntil(this XmlReader reader, XmlNodeType type)
        {
            await reader.ReadAsync();
            return await reader.ReadUntil(type);
        }

        public static async Task<bool> ReadUntil(this XmlReader reader, XmlNodeType type)
        {
            while (reader.NodeType != type)
            {
                if (!await reader.ReadAsync())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reads all elements at the current reader position with name.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="name">The element name</param>
        /// <returns></returns>
        public static IEnumerable<XElement> ReadXElements(this XmlReader reader, string name)
        {
            reader.MoveToContent();
            while (reader.Name == name)
            {
                yield return (XElement)XNode.ReadFrom(reader);
                reader.MoveToContent();
            }
        }
    }
}
