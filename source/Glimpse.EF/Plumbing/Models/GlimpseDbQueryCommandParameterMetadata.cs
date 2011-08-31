namespace Glimpse.EF.Plumbing.Models
{
    public class GlimpseDbQueryCommandParameterMetadata
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
    }
}