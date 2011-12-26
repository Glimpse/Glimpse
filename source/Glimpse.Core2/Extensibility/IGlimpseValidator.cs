namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpseValidator
    {
        GlimpseMode GetMode(RequestMetadata requestMetadata);
    }
}