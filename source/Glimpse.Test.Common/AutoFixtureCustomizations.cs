using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Glimpse.Test.Common
{
    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations() : base(new AutoMoqCustomization(), new MvcCustomizations(), new GlimpseCustomizations())
        {
        }
    }
}