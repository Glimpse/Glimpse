#if NET45Plus
using Glimpse.Core.Extensibility;
using Glimpse.WebForms.Model;
using System;
using System.Collections.Generic;
using System.Web;
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
                var dataBindParameterModel = (DataBindParameterModel)HttpContext.Current.Items["_GlimpseWebFormModelBinding"];
                if (dataBindParameterModel != null)
                {
                    var bindingContext = (ModelBindingContext)context.Arguments[1];
                    dataBindParameterModel.DataBindParameters.Add(new ModelBindParameter(bindingContext.ModelName, bindingContext.ValueProvider.GetType().Name.Replace("ValueProvider", null), bindingContext.Model));
                }
            }
        }
    }
}
#endif