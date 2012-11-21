namespace Glimpse.AspNet.Model
{
    public class ConfigurationHttpHandlersModel
    {
        public string Type { get; set; }

        public string Path { get; set; }

        public string Verb { get; set; }

        public bool Validate { get; set; }
    }
}