using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ViewEngineInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
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