using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ViewEngineInspector : IPipelineInspector
    {
        private IList<IViewEngine> OriginalViewEngines { get; set; }

        public void Setup(IPipelineInspectorContext context)
        {
            var logger = context.Logger;

            var alternateImplementation = new ViewEngine(context.ProxyFactory);

            OriginalViewEngines = new List<IViewEngine>();

            var currentEngines = ViewEngines.Engines;
            for (var i = 0; i < currentEngines.Count; i++)
            {
                var originalEngine = currentEngines[i];

                OriginalViewEngines.Add(originalEngine);

                IViewEngine newEngine;
                if (alternateImplementation.TryCreate(originalEngine, out newEngine))
                {
                    currentEngines[i] = newEngine;
                    logger.Info(Resources.ViewEngineSetupReplacedViewEngine, originalEngine.GetType());
                }
            }
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            if (OriginalViewEngines == null)
            {
                return;
            }

            var engines = ViewEngines.Engines;

            for (var i = 0; i < engines.Count; i++)
            {
                engines[i] = OriginalViewEngines[i];
            }

            OriginalViewEngines = null;
        }
    }
}