using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ModelBinderProvider : AlternateType<IModelBinderProvider>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ModelBinderProvider(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new GetBinder(new ModelBinder(ProxyFactory))
                    });
            }
        }

        public class GetBinder : AlternateMethod
        {
            public GetBinder(AlternateType<IModelBinder> alternateModelBinder) : base(typeof(IModelBinderProvider), "GetBinder")
            {
                AlternateModelBinder = alternateModelBinder;
            }

            public AlternateType<IModelBinder> AlternateModelBinder { get; set; }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                var originalModelBinder = context.ReturnValue as DefaultModelBinder;
                
                if (originalModelBinder == null)
                {
                    context.Logger.Warn(Resources.GetBinderNewImplementationCannotProxyWarning, context.ReturnValue.GetType());
                    return;
                }

                IModelBinder newModelBinder;
                if (AlternateModelBinder.TryCreate(originalModelBinder, out newModelBinder))
                {
                    context.ReturnValue = newModelBinder;
                }
            }
        }
    }
}