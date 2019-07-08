using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Rahl.XmlTransformer.Json;
using System.Linq;
using Rahl.XmlTransformer.Csv;
using System.Net.Http;
using System.Text;

namespace Rahl.XmlTransformer.Functions
{
    public static class Transform
    {
        [FunctionName("Transform")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var parameters = JsonConvert.DeserializeObject<TransformParameters>(await req.ReadAsStringAsync());
            var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(parameters.Data));
            var outputStream = new MemoryStream();

            await TransformFile(inputStream, outputStream, parameters);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StreamContent(outputStream)
            };
        }

        private static async Task TransformFile(Stream inputStream, Stream outputStream, TransformParameters parameters)
        {
            try
            {
                var transformer = CreateTransformer(parameters);
                await transformer.TransformAsync(inputStream, outputStream);
                outputStream.Position = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private static IXmlTransformer CreateTransformer(TransformParameters parameters)
        {
            var config = new TransformerConfiguration() { Mapping = parameters?.Mapping?.Select(p => new Map(p.Key, p.Value)) };
            if (parameters.Format == TransformFormat.Json)
                return new XmlToJsonTransformer(config);
            else
                return new XmlToCsvTransformer(config);
        }
    }

    public class TransformParameters
    {
        public TransformFormat Format { get; set; }
        public string Data { get; set; }
        public Dictionary<string, string> Mapping { get; set; }
    }

    public enum TransformFormat
    {
        Json = 0,
        Csv = 1
    }
}
