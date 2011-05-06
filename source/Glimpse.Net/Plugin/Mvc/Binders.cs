using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.Net.Extensibility;
using Glimpse.Net.Extensions;
using Glimpse.Net.Plumbing;
using System;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Binders:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Binding"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.BinderStore();

            var table = new List<object[]> { new[] { "Property/Parameter", "Type", "Attempted Providers", "Raw Value", "Attempted Value", "Culture" } };

            foreach (var boundProperty in store.Properties)
            {
                var providers = new List<object[]>{new []{"Provider", "Found"}};
                providers.AddRange(boundProperty.NotFoundIn.Select(valueProvider => new[] {valueProvider.GetType().ToString(), "False"}));

                if (boundProperty.FoundIn != null)
                    providers.Add(new[]{boundProperty.FoundIn.GetType().ToString(), "True", "selected"});

                table.Add(new [] {  string.IsNullOrEmpty(boundProperty.MemberOf) ? boundProperty.Name : boundProperty.MemberOf + "." + boundProperty.Name, 
                                    boundProperty.Type.ToString(), 
                                    providers, 
                                    boundProperty.RawValue, 
                                    boundProperty.AttemptedValue, 
                                    boundProperty.Culture != null ? boundProperty.Culture.DisplayName:"",
                                    boundProperty.FoundIn == null ? "error":""
                });
            }

            return table;
        }

        public void SetupInit(HttpApplication application)
        {
            GlimpsePipelineInitiation.ModelBinders();

            GlimpsePipelineInitiation.ValueProviders();
        }
    }
}
