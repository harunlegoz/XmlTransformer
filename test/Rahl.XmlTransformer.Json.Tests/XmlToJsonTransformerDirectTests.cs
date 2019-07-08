using ApprovalTests;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Rahl.XmlTransformer;
using Rahl.XmlTransformer.Tests.Core;

namespace Rahl.XmlTransformer.Json.Tests
{
    public class XmlToJsonTransformerDirectTests
    {
        [Test]
        public async Task ShouldDirectTransformBasicXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Basic1;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldDirectTransformComplexXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Complex1;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldDirectTransformArrayXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Basic2;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldDirectTransformArrayWithAttributesXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Basic3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldDirectTransformComplexArrayXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Complex2;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldDirectTransformComplexArrayWithAttributesXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Complex3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }
    }
}
