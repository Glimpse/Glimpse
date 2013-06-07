using System;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Display;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.SerializationConverter
{
    public class MvcDisplayModelConverter : SerializationConverter<MvcDisplayModel>
    {
        public override object Convert(MvcDisplayModel obj)
        {
            return new
                {
                    controllerName = obj.ControllerName,
                    actionName = obj.ActionName,
                    actionExecutionTime = Math.Round(obj.ActionExecutionTime.Value, 2),
                    childActionCount = obj.ChildActionCount,
                    childViewCount = obj.ChildViewCount,
                    viewName = obj.ViewName,
                    viewRenderTime = Math.Round(obj.ViewRenderTime.Value, 2),
                    matchedRouteName = obj.MatchedRouteName,
                };
        }
    }
}