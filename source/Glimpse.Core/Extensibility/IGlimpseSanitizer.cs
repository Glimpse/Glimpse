namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseSanitizer
    {
        string Sanitize(string json);
    }
}