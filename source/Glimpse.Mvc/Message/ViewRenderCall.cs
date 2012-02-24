using System;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.Message
{
    public class ViewRenderCall
    {
        public ViewRenderCall(FunctionTimerResult timerResult, ViewContext viewContext, IViewMixin viewMixin)
        {
            Offset = timerResult.Offset;
            Duration = timerResult.Duration;
            ViewData = viewContext.ViewData;
            //TODO: Add ViewData.ModelState.IsValid?
            //TODO: Add ViewData.Model.GetType()?
            TempData = viewContext.TempData;

            if (viewMixin != null)
            {
                ViewName = viewMixin.ViewName;
                IsPartial = viewMixin.IsPartial;
            }
        }

        public long Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public ViewDataDictionary ViewData { get; set; }
        public TempDataDictionary TempData { get; set; }
        public string ViewName { get; set; }
        public bool? IsPartial { get; set; }
    }
}