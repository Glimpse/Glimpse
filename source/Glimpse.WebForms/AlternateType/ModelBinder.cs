#if NET45Plus
using Glimpse.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Web.ModelBinding;

namespace Glimpse.WebForms.AlternateType
{
    public class ModelBinder : AlternateType<IModelBinder>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ModelBinder(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new BindModel(),
                });
            }
        }

        public class BindModel : AlternateMethod
        {
            public BindModel() : base(typeof(IModelBinder), "BindModel")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
            }
        }
    }
}
#endif