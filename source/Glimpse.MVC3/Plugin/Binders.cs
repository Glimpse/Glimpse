using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Extensions;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Binders:IGlimpsePlugin, IProvideGlimpseHelp
    {
        internal IGlimpseLogger Logger { get; set; }

        [ImportingConstructor]
        public Binders(IGlimpseFactory factory)
        {
            Logger = factory.CreateLogger();
        }

        public string Name
        {
            get { return "Binding"; }
        }

        public object GetData(HttpContextBase context)
        {
            var store = context.BinderStore();

            if (store.Properties.Count == 0) return null;

            var table = new List<object[]> { new[] { "Ordinal", "Model Binder", "Property/Parameter", "Type", "Attempted Value Providers", "Attempted Value", "Culture", "Raw Value" } };

            var ordinal = 0;

            foreach (var boundProperty in store.Properties)
            {
                var providers = new List<object[]>{new []{"Provider", "Successful"}};
                providers.AddRange(boundProperty.NotFoundIn.Select(valueProvider => new[] {valueProvider.GetType().ToString(), "False"}));

                if (boundProperty.FoundIn != null)
                    providers.Add(new[]{boundProperty.FoundIn.GetType().ToString(), "True", "selected"});

                table.Add(new [] {  ordinal++,
                                    boundProperty.ModelBinderType == null ? null : boundProperty.ModelBinderType.ToString(),
                                    string.IsNullOrEmpty(boundProperty.MemberOf) ? boundProperty.Name : boundProperty.MemberOf + "." + boundProperty.Name, 
                                    boundProperty.Type.ToString(), 
                                    providers, 
                                    boundProperty.AttemptedValue, 
                                    boundProperty.Culture != null ? boundProperty.Culture.DisplayName:null,
                                    boundProperty.RawValue,
                                    string.IsNullOrEmpty(boundProperty.MemberOf) ? "":"quiet"
                });
            }

            return table;
        }

        public void SetupInit()
        {
            var initiator = new GlimpsePipelineInitiator(Logger);

            initiator.ModelBinderProviders();

            initiator.ModelBinders();

            initiator.ValueProviders();
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Binders"; }
        }
    }
}
