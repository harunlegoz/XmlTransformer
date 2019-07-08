using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rahl.XmlTransformer.Json
{
    public class XmlToJsonTransformer : TransformerBase
    {
        private const string ValueItemName = "_value";

        public XmlToJsonTransformer()
            : this(new TransformerConfiguration())
        {
            
        }

        public XmlToJsonTransformer(TransformerConfiguration configuration)
            : base(configuration)
        {
        }

        protected override async Task<TransformerContext> StartAsync(XmlReader reader, TextWriter writer)
        {
            var jsonWriter = new JsonTextWriter(writer);
            await jsonWriter.WriteStartArrayAsync();

            return new JsonTransformerContext(reader, writer, jsonWriter);
        }

        protected override async Task EndAsync(TransformerContext context)
        {
            await context.As<JsonTransformerContext>().JsonWriter.WriteEndArrayAsync();
        }

        protected override async Task CloseAsync(TransformerContext context)
        {
            await context.As<JsonTransformerContext>().JsonWriter?.CloseAsync();
        }

        protected override async Task WriteItem(TransformerContext context, Record item)
        {
            var jsonWriter = context.As<JsonTransformerContext>().JsonWriter;
            if (item.Count() == 1 && item.First().Name == ValueItemName)
            {
                await AsJToken(item.First().Value).WriteToAsync(jsonWriter);
            }
            else
            {
                await jsonWriter.WriteStartObjectAsync();

                foreach (var field in item)
                {
                    await jsonWriter.WritePropertyNameAsync(field.Name);
                    await AsJToken(field.Value).WriteToAsync(jsonWriter);
                }

                await jsonWriter.WriteEndObjectAsync();
            }
        }

        private static JToken AsJToken(object val)
        {
            JToken value = null;
            if (val == null)
                value = JValue.CreateNull();
            else if (val is Record item)
            {
                var obj = new JObject();
                foreach (var field in item)
                {
                    obj.Add(field.Name, AsJToken(field.Value));
                }
                value = obj;
            }
            else if (val is IEnumerable<object> collVal)
                value = new JArray(collVal.Select(p => AsJToken(p)).ToArray());
            else
                value = new JValue(val);

            return value;
        }

        protected override async Task FlushAsync(TransformerContext context)
        {
            await context.As<JsonTransformerContext>().JsonWriter.FlushAsync();
        }

        public class JsonTransformerContext : TransformerContext
        {
            public JsonTransformerContext(XmlReader reader, TextWriter writer, JsonTextWriter jsonWriter) : base(reader, writer)
            {
                JsonWriter = jsonWriter;
            }

            public JsonTextWriter JsonWriter { get; set; }

            public override void Dispose()
            {
                JsonWriter = null;

                base.Dispose();
            }
        }
    }
}
