namespace Glimpse.Core.Framework
{
    public class PluginMetadata
    {
        public string DocumentationUri { get; set; }
        
        public bool HasMetadata 
        { 
            get
            {
                return !string.IsNullOrEmpty(DocumentationUri);
            } 
        }
    }
}