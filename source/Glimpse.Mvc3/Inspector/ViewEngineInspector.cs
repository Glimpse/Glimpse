using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;

namespace Glimpse.Mvc.Inspector
{
    public class ViewEngineInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;

            var alternateImplementation = new ViewEngine(context.ProxyFactory);

            var currentEngines = ViewEngines.Engines;
            for (var i = 0; i < currentEngines.Count; i++)
            {
                var originalEngine = currentEngines[i];

                IViewEngine newEngine;
                if (alternateImplementation.TryCreate(originalEngine, out newEngine))
                {
                    currentEngines[i] = newEngine;
                    logger.Info(Resources.ViewEngineSetupReplacedViewEngine, originalEngine.GetType());
                }
                else
                {
                    logger.Warn(Resources.ViewEngineSetupNotReplacedViewEngine, originalEngine.GetType()); 
                }
            }
        }
    }
}