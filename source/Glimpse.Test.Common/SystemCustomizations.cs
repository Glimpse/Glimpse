using System.Reflection;
using Ploeh.AutoFixture;

namespace Glimpse.Test.Common
{
    public class SystemCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<MethodInfo>(() => typeof(object).GetMethod("ToString"));
        }
    }
}