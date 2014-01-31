namespace Glimpse.WebForms.Model
{
    public class DataBindParameter : ModelBindParameter
    {
        public string Type { get; private set; }

        public DataBindParameter(string field, string source, string type, object value) : base(field, source, value)
        {
            Type = type;
        }
    }
}
