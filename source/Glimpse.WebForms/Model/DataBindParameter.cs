namespace Glimpse.WebForms.Model
{
    public class DataBindParameter : ModelBindParameter
    {
        public string Type { get; private set; }

        public object Default { get; private set; }

        public DataBindParameter(string field, string source, object value, string type, object defaultValue)
            : base(field, source, value)
        {
            Type = type;
            Default = defaultValue;
        }
    }
}