namespace Glimpse.Mvc.AlternateImplementation
{
    public interface IViewMixin
    {
        string ViewName { get; }
        bool IsPartial { get; }
    }
}