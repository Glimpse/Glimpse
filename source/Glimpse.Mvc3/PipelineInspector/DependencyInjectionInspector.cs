using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.PipelineInspector
{
    public class DependencyInjectionInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var proxyFactory = context.ProxyFactory;
            var logger = context.Logger;

            var dependencyResolver = DependencyResolver.Current;
            if (proxyFactory.IsProxyable(dependencyResolver))
            {
                var alternateImplementations = Mvc.AlternateImplementation.DependencyResolver.AllMethods(context.RuntimePolicyStrategy, context.MessageBroker);

                var proxiedDependencyResolver = proxyFactory.CreateProxy(dependencyResolver, alternateImplementations);

                DependencyResolver.SetResolver(proxiedDependencyResolver);

                logger.Debug(Resources.ExecutionSetupProxiedIDependencyResolver, dependencyResolver.GetType());
            }
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}