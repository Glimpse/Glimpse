using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace Glimpse.Test.Common
{
    public class AutoMockAttribute : AutoDataAttribute
    {
        public AutoMockAttribute() : base(new Fixture().Customize(new AutoFixtureCustomizations()))
        {
        }
    }
}
