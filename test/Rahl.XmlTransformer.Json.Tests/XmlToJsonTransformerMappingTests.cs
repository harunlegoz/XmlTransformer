using ApprovalTests;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Rahl.XmlTransformer;
using Rahl.XmlTransformer.Tests.Core;

namespace Rahl.XmlTransformer.Json.Tests
{
    public class XmlToJsonTransformerMappingTests
    {
        [Test]
        public async Task ShouldMappingTransformBasicXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                 Mapping = new[]
                 {
                     new Map("value", "/text()")
                 }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Basic1;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformComplexXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("prop1", "/prop1/text()"),
                    new Map("prop2", "/prop2"),
                    new Map("prop3", "/prop3/child/text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Complex2;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformArrayXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "/text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Basic2;
            var outputBuilder = new StringBuilder();

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformArrayWithAttributesXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("attr1", "$/@attr1"),
                    new Map("attr2", "$/@attr2"),
                    new Map("value", "/text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Basic3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformComplexArrayXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("prop1", "/prop1/text()"),
                    new Map("prop2", "/prop2"),
                    new Map("prop3", "/prop3/child/text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Complex3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformComplexArrayWithAttributesXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("attr1", "$/@attr1"),
                    new Map("attr2", "$/@attr2"),
                    new Map("prop1", "/prop1/text()"),
                    new Map("prop2", "/prop2"),
                    new Map("prop3", "/prop3/child/text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.Complex3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        // Data Type Tests

        [Test]
        public async Task ShouldMappingTransformIntFieldXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "int -> /text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.BasicMappingNumeric;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        public async Task ShouldMappingTransformLongFieldXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "long -> /text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.BasicMappingNumeric;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformDecimalFieldXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "decimal -> /text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.BasicMappingDecimal;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformBooleanFieldXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "bool -> /text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.BasicMappingBoolean;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformDateTimeFieldXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "DateTime -> /text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.BasicMappingDateTime;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }

        [Test]
        public async Task ShouldMappingTransformDateTimeFieldWithFormatXmlToJsonSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "DateTime(MM/dd/yyyy HH:mm:ss) -> /text()")
                }
            };
            var transformer = new XmlToJsonTransformer(config);
            var xml = TestData.BasicMappingDateTime2;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.VerifyJson(output);
        }
    }
}
