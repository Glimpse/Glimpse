using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ViewEngine : IPipelineInspector
    {
        private IList<IViewEngine> OriginalViewEngines { get; set; }

        public void Setup(IPipelineInspectorContext context)
        {
            var logger = context.Logger;
            logger.Debug("Setup IPipelineInspector of type '{0}'.", GetType());

            var proxyFactory = context.ProxyFactory;
            var engines = ViewEngines.Engines;

            var alternateImplementations = AlternateImplementation.ViewEngine.All(context.MessageBroker, context.ProxyFactory, context.TimerStrategy);
            OriginalViewEngines = new List<IViewEngine>();

            for (var i = 0; i < engines.Count; i++)
            {
                var engine = engines[i];

                OriginalViewEngines.Add(engine);

                if (proxyFactory.IsProxyable(engine))
                {
                    engines[i] = proxyFactory.CreateProxy(engine, alternateImplementations);
                    logger.Debug("Replaced IViewEngine of type '{0}' with proxy implementation.", engine.GetType()); //TODO: Move to resource
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