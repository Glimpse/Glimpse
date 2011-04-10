using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseValueProviderFactory:ValueProviderFactory
    {
        public ValueProviderFactory Factory { get; set; }

        public GlimpseValueProviderFactory(ValueProviderFactory factory)
        {
            Factory = factory;
        }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var cc = controllerContext;

            var valueProvider = Factory.GetValueProvider(cc);

            if (valueProvider != null && !(valueProvider is GlimpseValueProvider))
                valueProvider = new GlimpseValueProvider(valueProvider);

            return valueProvider;
        }
    }
}
