using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using NUnit.Framework;
using Rahl.XmlTransformer;
using Rahl.XmlTransformer.Tests.Core;

namespace Rahl.XmlTransformer.Csv.Tests
{
    public class XmlToCsvTransformerDirectTests
    {
        [Test]
        public async Task ShouldDirectTransformBasicXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Basic1;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldDirectTransformComplexXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Complex1;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldDirectTransformArrayXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Basic2;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldDirectTransformArrayWithAttributesXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Basic3;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldDirectTransformComplexArrayXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Complex2;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldDirectTransformComplexArrayWithAttributesXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration();
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Complex3;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }
    }
}