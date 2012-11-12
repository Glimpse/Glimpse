namespace Glimpse.Mvc.Message
{
    public interface IBoundedFilterMessage : IActionBasedMessage
    {
        FilterBounds Bounds { get; }
    }
}