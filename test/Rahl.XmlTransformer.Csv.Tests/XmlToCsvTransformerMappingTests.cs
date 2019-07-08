using ApprovalTests;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Rahl.XmlTransformer;
using Rahl.XmlTransformer.Tests.Core;

namespace Rahl.XmlTransformer.Csv.Tests
{
    public class XmlToCsvTransformerMappingTests
    {
        [Test]
        public async Task ShouldMappingTransformBasicXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                 Mapping = new[]
                 {
                     new Map("value", "/text()")
                 }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Basic1;
            var outputBuilder = new StringBuilder();

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformComplexXmlToCsvSuccessfully()
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
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Complex2;

            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformArrayXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "/text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Basic2;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformArrayWithAttributesXmlToCsvSuccessfully()
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
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Basic3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformComplexArrayXmlToCsvSuccessfully()
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
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Complex3;
           
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformComplexArrayWithAttributesXmlToCsvSuccessfully()
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
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.Complex3;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        // Data Type Tests

        [Test]
        public async Task ShouldMappingTransformIntFieldXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "int -> /text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.BasicMappingNumeric;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        public async Task ShouldMappingTransformLongFieldXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "long -> /text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.BasicMappingNumeric;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformDecimalFieldXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "decimal -> /text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.BasicMappingDecimal;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformBooleanFieldXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "bool -> /text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.BasicMappingBoolean;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformDateTimeFieldXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "DateTime -> /text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.BasicMappingDateTime;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }

        [Test]
        public async Task ShouldMappingTransformDateTimeFieldWithFormatXmlToCsvSuccessfully()
        {
            // Assign
            var config = new TransformerConfiguration()
            {
                Mapping = new[]
                {
                    new Map("value", "DateTime(MM/dd/yyyy HH:mm:ss) -> /text()")
                }
            };
            var transformer = new XmlToCsvTransformer(config);
            var xml = TestData.BasicMappingDateTime2;
            
            // Act
            var output = await transformer.TransformAsync(xml);

            // Assert
            Approvals.Verify(output);
        }
    }
}
