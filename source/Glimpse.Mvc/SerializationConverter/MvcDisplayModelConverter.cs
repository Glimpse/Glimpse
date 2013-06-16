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
                    actionExecutionTime = GetRoundedValueIfExists(obj.ActionExecutionTime),
                    childActionCount = obj.ChildActionCount,
                    childViewCount = obj.ChildViewCount,
                    viewName = obj.ViewName,
                    viewRenderTime = GetRoundedValueIfExists(obj.ViewRenderTime),
                    matchedRouteName = obj.MatchedRouteName,
                };
        }

        private object GetRoundedValueIfExists(double? value)
        {
            if (value.HasValue)
            {
                return Math.Round(value.Value, 2);
            }

            return null;
        }
    }
}
