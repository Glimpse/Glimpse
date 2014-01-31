using System;

namespace Glimpse.WebForms.Model
{
    public class DataBindParameter
    {
        public string Field { get; private set; }

        public string Source { get; private set; }

        public object Value { get; private set; }

        public DataBindParameter(string field, string source, object value)
        {
            Field = field;
            Source = source;
            Value = value;
        }
    }
}
