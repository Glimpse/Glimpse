using System;
using System.Web;

namespace Glimpse.Core.Extensibility
{
    public interface IProvideGlimpsePaging
    {
        Guid PagerKey { get; }
        PagerType PagerType { get; }
        int PageSize { get; }
        int PageIndex { get; }
        int TotalNumberOfRecords { get; }
        object GetData(HttpContextBase context, int pageIndex);
    }

    public enum PagerType
    {
        TraditionalPager = 0,
        ContinuousPaging = 1,
        ContinuousScrolling = 2 
    }
}