namespace Glimpse.WebForms.Model
{
    public class ModelBindParameter
    {
        public string Source { get; private set; }

        public string Field { get; private set; }

        public object Value { get; private set; }

        public ModelBindParameter(string field, string source, object value)
        {
            Field = field;
            Source = source;
            Value = value;
        }
    }
}
