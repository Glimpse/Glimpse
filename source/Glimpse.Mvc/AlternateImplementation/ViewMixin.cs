namespace Glimpse.Mvc.AlternateImplementation
{
    public class ViewMixin:IViewMixin
    {
        public string ViewName { get; set; }
        public bool IsPartial { get; set; }
    }
}