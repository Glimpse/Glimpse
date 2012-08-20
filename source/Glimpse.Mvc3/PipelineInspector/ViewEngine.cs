using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.PipelineInspector
{
    public class ViewEngine : IPipelineInspector
    {
        private IList<IViewEngine> OriginalViewEngines { get; set; }

        public void Setup(IPipelineInspectorContext context)
        {
            var logger = context.Logger;
            var proxyFactory = context.ProxyFactory;

            var alternateImplementations = Mvc3.AlternateImplementation.ViewEngine.AllMethods(context.MessageBroker, context.ProxyFactory, context.Logger, context.TimerStrategy, context.RuntimePolicyStrategy);

            OriginalViewEngines = new List<IViewEngine>();

            var currentEngines = ViewEngines.Engines;
            for (var i = 0; i < currentEngines.Count; i++)
            {
                var engine = currentEngines[i];

                OriginalViewEngines.Add(engine);

                if (proxyFactory.IsProxyable(engine))
                {
                    currentEngines[i] = proxyFactory.CreateProxy(engine, alternateImplementations);
                    logger.Info(Resources.ViewEngineSetupReplacedViewEngine, engine.GetType());
                }
            }
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            if (OriginalViewEngines == null) return;

            var engines = ViewEngines.Engines;

            for (var i = 0; i < engines.Count; i++)
            {
                engines[i] = OriginalViewEngines[i];
            }

            OriginalViewEngines = null;
        }
    }
}