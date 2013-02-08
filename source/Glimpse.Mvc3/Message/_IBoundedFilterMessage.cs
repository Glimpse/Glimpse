namespace Glimpse.Mvc.Message
{
    public interface IBoundedFilterMessage : IActionMessage
    {
        FilterBounds Bounds { get; }
    }
}