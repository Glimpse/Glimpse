namespace Glimpse.Core2.Extensibility
{
    public class ResourceParameterMetadata
    {
        public ResourceParameterMetadata(string name):this(name, null, true){}
        public ResourceParameterMetadata(string name, bool isRequired):this(name, null, isRequired){}
        public ResourceParameterMetadata(string name, string defaultValue):this(name, defaultValue, true){}

        public ResourceParameterMetadata(string name, string defaultValue, bool isRequired)
        {
            Name = name;
            Value = defaultValue;
            IsRequired = isRequired;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsRequired { get; set; }

        public bool HasValue {
            get { return !string.IsNullOrEmpty(Value); }
        }
    }
}