namespace Glimpse.Core2.Extensibility
{
    public class ResourceParameterMetadata
    {
        public ResourceParameterMetadata(string name, bool isRequired = true)
        {
            Name = name;
            IsRequired = isRequired;
        }

        public string Name { get; set; }
        public bool IsRequired { get; set; }
    }
}