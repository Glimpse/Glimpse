using System;

namespace Glimpse.WebForms.Model
{
    public class DataBindParameter
    {
        public string Name { get; private set; }

        public string Source { get; private set; }

        public object Value { get; private set; }

        public DataBindParameter(string name, string source, object value)
        {
            Name = name;
            Source = source;
            Value = value;
        }
    }
}
