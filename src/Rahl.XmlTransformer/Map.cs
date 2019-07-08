using System.Text.RegularExpressions;

namespace Rahl.XmlTransformer
{
    public class Map
    {
        private static readonly Regex _mappingParser = new Regex(@"((?<type>string|datetime|int|bool|decimal|long)\s*(\((?<format>[a-zA-Z0-9-\/\\\+\=\:\%_\s]+)\))?\s*-\>)?\s*(?<path>[a-zA-Z0-9\/\\\$].+)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        public Map(string name, string mapExpression)
        {
            Name = name;
            Type = "string";
            var match = _mappingParser.Match(mapExpression);
            if (!match.Success)
                return;

            Type = match.Groups["type"]?.Value;
            Format = match.Groups["format"]?.Value;
            Path = match.Groups["path"]?.Value;

            if (string.IsNullOrEmpty(Type))
                Type = "string";
        }

        public string Name { get; }
        public string Path { get; }
        public string Type { get; }
        public string Format { get; }
    }
}
