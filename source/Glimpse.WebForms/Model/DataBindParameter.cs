using System;

namespace Glimpse.WebForms.Model
{
    public class DataBindParameter
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public object Value { get; private set; }
        public DataBindParameter(string name, Type type, object value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
