using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Glimpse.Test.Common
{
    public class AutoFixtureCustomizations : CompositeCustomization
    {
        public AutoFixtureCustomizations() : base(new AutoMoqCustomization(), new SystemCustomizations(), new MvcCustomizations(), new GlimpseCustomizations())
        {
        }
    }
}