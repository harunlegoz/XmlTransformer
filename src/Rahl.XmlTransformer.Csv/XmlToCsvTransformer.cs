using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CsvHelper;

namespace Rahl.XmlTransformer.Csv
{
    public class XmlToCsvTransformer : TransformerBase
    {
        private const string CsvWriter = "CsvWriter";
        private const string HeaderWritten = "HeaderWritten";

        public XmlToCsvTransformer()
            : this(new TransformerConfiguration())
        {
            
        }

        public XmlToCsvTransformer(TransformerConfiguration configuration)
            : base(configuration)
        {
        }

        protected override Task<TransformerContext> StartAsync(XmlReader reader, TextWriter writer)
        {
            return Task.FromResult<TransformerContext>(new CsvTransformerContext(reader, writer, new CsvWriter(writer, new CsvHelper.Configuration.Configuration() { ShouldQuote = (a,b) => true, CultureInfo = CultureInfo.InvariantCulture, Encoding = Encoding.UTF8 })));
        }

        protected override Task EndAsync(TransformerContext context)
        {
            return Task.CompletedTask;
        }

        protected override Task CloseAsync(TransformerContext context)
        {
            context.As<CsvTransformerContext>().CsvWriter.Dispose();

            return Task.CompletedTask;
        }

        protected override async Task WriteItem(TransformerContext context, Record item)
        {
            var writer = context.As<CsvTransformerContext>().CsvWriter;
            if (!context.As<CsvTransformerContext>().HeaderWritten)
            {
                foreach (var field in item)
                {
                    writer.WriteField(field.Name);
                }

                context.As<CsvTransformerContext>().HeaderWritten = true;
                await writer.NextRecordAsync();
            }

            foreach (var field in item)
            {
                writer.WriteField(AsCsvField(field.Value));
            }

            await writer.NextRecordAsync();
        }

        private static object AsCsvField(object val)
        {
            object value = null;
            if (val == null)
                value = null;
            else if (val is Record item)
            {
                var obj = new StringBuilder();
                foreach (var field in item)
                {
                    if (obj.Length > 0)
                        obj.Append("|");

                    obj.Append($"\"{field.Name}\"=\"{field.Value}\"");
                }

                value = obj.ToString();
            }
            else if (val is IEnumerable<object> collVal)
            {
                var obj = new StringBuilder();
                foreach (var field in collVal.Select(p => AsCsvField(p)).ToArray())
                {
                    if (obj.Length > 0)
                        obj.Append("|");

                    obj.Append(field);
                }
                value = obj.ToString();
            }
            else
                value = val;

            return value;
        }

        protected override async Task FlushAsync(TransformerContext context)
        {
            await context.As<CsvTransformerContext>().CsvWriter.FlushAsync();
        }

        public class CsvTransformerContext : TransformerContext
        {
            public CsvTransformerContext(XmlReader reader, TextWriter writer, CsvWriter csvWriter) : base(reader, writer)
            {
                CsvWriter = csvWriter;
            }

            public CsvWriter CsvWriter { get; set; }
            public bool HeaderWritten { get; set; }
        }
    }
}
