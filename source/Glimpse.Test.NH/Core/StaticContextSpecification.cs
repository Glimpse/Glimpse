using NUnit.Framework;

namespace Glimpse.Test.NH.Core
{
    [TestFixture]
    public abstract class StaticContextSpecification
    {
        [SetUp]
        public void SetUp()
        {
            Arrange();
            Act();
        }

        protected abstract void Arrange();

        protected abstract void Act();
    }
}