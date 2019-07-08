using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rahl.XmlTransformer
{
    public class Record : IEnumerable<Field>
    {
        private List<Field> _fields = new List<Field>();

        public Record()
        {
            
        }

        public Record(Record parent)
        {
            _fields.AddRange(parent._fields);
        }

        public object this[string name]
        {
            get { return _fields.FirstOrDefault(p => p.Name == name); }
            set
            {
                if (_fields.Any(p => p.Name == name))
                    throw new TransformerException($"Field with name {name} already exists.");

                _fields.Add(new Field(name, value));
            }
        }

        public IEnumerator<Field> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
