using System;

namespace Glimpse.Mvc.AlternateImplementation
{
    public interface IViewCorrelation
    {
        string ViewName { get; }

        bool IsPartial { get; }

        Guid ViewEngineFindCallId { get; }
    }

    public class ViewCorrelation : IViewCorrelation
    {
        public ViewCorrelation(string viewName, bool isPartial, Guid viewEngineFindCallId)
        {
            ViewName = viewName;
            IsPartial = isPartial;
            ViewEngineFindCallId = viewEngineFindCallId;
        }

        public string ViewName { get; set; }

        public bool IsPartial { get; set; }

        public Guid ViewEngineFindCallId { get; set; }
    }
}
