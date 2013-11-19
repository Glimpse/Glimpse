namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public class TraceModel
    {
        public string Category { get; set; }

        public string Message { get; set; }

        public double FromFirst { get; set; }

        public double FromLast { get; set; }

        public int IndentLevel { get; set; }
    }
}