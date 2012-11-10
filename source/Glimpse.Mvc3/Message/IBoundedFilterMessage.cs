namespace Glimpse.Mvc.Message
{
    public interface IBoundedFilterMessage
    {
        FilterBounds Bounds { get; }
    }
}