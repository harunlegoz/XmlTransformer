using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Rahl.XmlTransformer
{
    public abstract class TransformerBase : IXmlTransformer
    {
        private const string AttributeMapExpressionStart = "$/@";
        private const string ValueElementName = "_value";
        private const string AttributePropertyStartCharacter = "_";

        public TransformerConfiguration Configuration { get; }

        public TransformerBase(TransformerConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<string> TransformAsync(string xml)
        {
            var outputBuilder = new StringBuilder();

            using (var inputReader = new StringReader(xml))
            using (var outputWriter = new StringWriter(outputBuilder))
            {
                await TransformAsync(inputReader, outputWriter);
            }

            return outputBuilder.ToString();
        }

        public async Task TransformAsync(Stream inputStream, Stream outputStream)
        {
            using (var reader = new StreamReader(inputStream, Configuration.DefaultEncoding, true, Configuration.DefaultBufferSize, true))
            using (var writer = new StreamWriter(outputStream, Configuration.DefaultEncoding, Configuration.DefaultBufferSize, true))
            {
                await TransformAsync(reader, writer);
            }
        }

        public async Task TransformAsync(TextReader reader, TextWriter writer)
        {
            using (var xmlReader = XmlReaderFactory.Create(reader))
            {
                TransformerContext context = null;
                try
                {
                    context = await StartAsync(xmlReader, writer);

                    await MoveToRootElement(xmlReader);

                    var attributes = CreateItem();
                    ReadRootAttributes(xmlReader, attributes);

                    await ProcessElements(context, attributes);

                    await EndAsync(context);
                }
                finally
                {
                    await CloseAsync(context);
                    context?.Dispose();
                }
            }
        }

        protected abstract Task<TransformerContext> StartAsync(XmlReader reader, TextWriter writer);

        protected abstract Task EndAsync(TransformerContext context);

        protected abstract Task CloseAsync(TransformerContext context);

        protected abstract Task WriteItem(TransformerContext context, Record item);

        protected abstract Task FlushAsync(TransformerContext context);

        private static async Task<string> MoveToRootElement(XmlReader reader)
        {
            await reader.ReadUntil(XmlNodeType.Element); // Moving to root element
            var rootElementName = reader.LocalName;
            return rootElementName;
        }

        private async Task ProcessElements(TransformerContext context, Record attributes)
        {
            var elementMapping = Configuration.Mapping?.Where(p => !p.Path.StartsWith(AttributeMapExpressionStart));

            while (true)
            {
                List<XElement> list = await LoadBatch(context.Reader);
                if (list.Count == 0)
                    break;

                var items = list.AsParallel().AsOrdered().Select(element =>
                {
                    var item = CreateItem(attributes);

                    if (elementMapping == null)
                        ParseItemValues(element, item);
                    else
                        ParseItemValues(element, item, elementMapping);

                    return item;
                }).ToArray();

                foreach (var item in items)
                {
                    await WriteItem(context, item);
                }

                await FlushAsync(context);
            }
        }

        private static async Task<List<XElement>> LoadBatch(XmlReader xmlReader)
        {
            int batchSize = 10000;
            var list = new List<XElement>(batchSize);
            for (int i = 0; i < batchSize; i++)
            {
                if (xmlReader.Depth == 0)
                    await xmlReader.ReadThenLookUntil(XmlNodeType.Element);

                if (!await xmlReader.ReadUntil(XmlNodeType.Element))
                    break;

                var element = (XElement)XNode.ReadFrom(xmlReader);
                list.Add(element);

                await xmlReader.ReadUntil(XmlNodeType.Element); //Ensure move to next element
            }

            return list;
        }

        private static Record CreateItem(Record attributes = null)
        {
            return attributes != null ? new Record(attributes) : new Record();
        }

        private static void ParseItemValues(XElement element, Record item)
        {
            if (element.HasAttributes)
            {
                foreach (var attribute in element.Attributes())
                {
                    item[AttributePropertyStartCharacter + attribute.Name.LocalName] = attribute.Value;
                }
            }

            if (element.HasElements)
            {
                foreach (var child in element.Elements())
                {
                    if (child.HasElements)
                    {
                        var grandChildren = child.Elements();
                        if (grandChildren.Any(p => p.HasAttributes || p.HasElements) ||
                            grandChildren.Select(p => p.Name.LocalName).Distinct().Count() > 1)
                        {
                            // This means this is going to be element tree
                            var childItem = CreateItem();
                            ParseItemValues(child, childItem);
                            item[child.Name.LocalName] = childItem;
                        }
                        else
                        {
                            // This means we will tread grandchildren as array
                            item[child.Name.LocalName] = grandChildren.Select(p => p.Value).ToArray();
                        }
                    }
                    else
                    {
                        item[child.Name.LocalName] = child.Value;
                    }
                }
            }
            else
            {
                item[ValueElementName] = element.Value;
            }
        }

        private void ParseItemValues(XElement element, Record item, IEnumerable<Map> elementMapping)
        {
            foreach (var elementToMap in elementMapping)
            {
                item[elementToMap.Name] = ReadXPathValue(element, elementToMap.Path, elementToMap.Type, elementToMap.Format);
            }
        }

        private void ReadRootAttributes(XmlReader reader, Record item)
        {
            if (reader.HasAttributes)
            {
                var attributesToRead = Configuration.Mapping?.Where(p => p.Path.StartsWith(AttributeMapExpressionStart)).ToDictionary(p => p.Path.Substring(3), p => p);
                while (reader.MoveToNextAttribute())
                {
                    if (attributesToRead != null)
                    {
                        if (attributesToRead.TryGetValue(reader.LocalName, out Map map))
                        {
                            item[map.Name] = ConvertHelper.ConvertValue(reader.Value, map.Type, map.Format);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        item[AttributePropertyStartCharacter + reader.LocalName] = reader.Value;
                    }
                }
            }
        }

        private static object ReadXPathValue(XElement element, string xpath, string type, string format)
        {
            return XValueToValue(element.XPathEvaluate(xpath), type, format);
        }

        private static object XValueToValue(object val, string type = "string", string format = null)
        {
            object value = null;

            if (val == null)
                value = null;
            else if (val is string strVal)
                value = ConvertHelper.ConvertValue(strVal, type, format);
            else if (val is XText textVal)
                value = ConvertHelper.ConvertValue(textVal.ToString(), type, format);
            else if (val is XAttribute attributeVal)
                value = ConvertHelper.ConvertValue(attributeVal.Value, type, format);
            else if (val is XElement elementVal)
            {
                var item = CreateItem();
                ParseItemValues(elementVal, item);
                value = item;
            }
            else if (val is IEnumerable<object> collVal)
            {
                var arr = collVal.ToArray();
                if (arr.Length == 0)
                    value = null;
                else if (arr.Length == 1)
                    value = XValueToValue(arr[0], type, format);
                else
                    value = collVal.Select(p => XValueToValue(p, type, format)).ToArray();
            }

            return value;
        }
    }

    public class TransformerContext : IDisposable
    {
        public TransformerContext(XmlReader reader, TextWriter writer)
        {
            Reader = reader;
            Writer = writer;
        }

        public XmlReader Reader { get; private set; }
        public TextWriter Writer { get; private set; }

        public T As<T>() where T : TransformerContext
        {
            return (T)this;
        }

        public virtual void Dispose()
        {
            Reader = null;
            Writer = null;
        }
    }
}
