using System;

namespace Rahl.XmlTransformer.Tests.Core
{
    public static class TestData
    {
        public const string Basic1 = "<items><item>A</item></items>";
        public const string Basic2 = "<items><item>A</item><item>B</item><item>C</item><item>D</item><item>E</item></items>";
        public const string Basic3 = "<items attr1=\"A1\" attr2=\"A2\"><item>A</item><item>B</item><item>C</item><item>D</item><item>E</item></items>";
        public const string BasicMappingNumeric = "<items><item>3</item></items>";
        public const string BasicMappingDecimal = "<items><item>3.9597</item></items>";
        public const string BasicMappingBoolean = "<items><item>true</item></items>";
        public const string BasicMappingDateTime = "<items><item>2018-12-11T13:23:32Z</item></items>";
        public const string BasicMappingDateTime2 = "<items><item>12/11/2018 13:23:32</item></items>";
        public const string Complex1 = "<items><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item></items>";
        public const string Complex2 = "<items><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item></items>";
        public const string Complex3 = "<items attr1=\"A1\" attr2=\"A2\"><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item></items>";
    }
}
