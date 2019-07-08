namespace Rahl.XmlTransformer
{
    public class Field
    {
        public string Name { get; }
        public object Value { get; }

        public Field(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
