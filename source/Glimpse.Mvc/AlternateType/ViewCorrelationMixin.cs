using System;

namespace Glimpse.Mvc.AlternateType
{
    public interface IViewCorrelationMixin
    {
        string ViewName { get; }

        bool IsPartial { get; }

        Guid ViewEngineFindCallId { get; }
    }

    public class ViewCorrelationMixin : IViewCorrelationMixin
    {
        public ViewCorrelationMixin(string viewName, bool isPartial, Guid viewEngineFindCallId)
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
