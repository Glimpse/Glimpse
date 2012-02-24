using System;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Mvc.Message
{
    public class ViewEngineFindCall
    {
        public ViewEngineFindCall(FunctionTimerResult timerResult, ViewEngineResult viewEngineResult, IView view, bool isPartial, string viewName, string masterName, bool useCache, Type viewEngineType)
        {
            Offset = timerResult.Offset;
            Duration = timerResult.Duration;
            ViewEngineResult = viewEngineResult;
            IsPartial = isPartial;
            ViewName = viewName;
            MasterName = masterName;
            UseCache = useCache;
            View = view;
            ViewEngineType = viewEngineType;
        }

        public ViewEngineResult ViewEngineResult { get; set; }
        public bool IsPartial { get; set; }
        public string ViewName { get; set; }
        public string MasterName { get; set; }
        public bool UseCache { get; set; }
        public IView View { get; set; }
        public Type ViewEngineType { get; set; }
        public long Offset { get; set; }
        public TimeSpan Duration { get; set; }
    }
}