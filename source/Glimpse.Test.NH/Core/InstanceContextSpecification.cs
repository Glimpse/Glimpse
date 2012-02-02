using NUnit.Framework;

namespace Glimpse.Test.NH.Core
{
    [TestFixture]
    public abstract class InstanceContextSpecification<TSut>
    {
        protected TSut Sut;

        [SetUp]
        public void SetUp()
        {
            Arrange();
            Sut = CreateSystemUnderTest();
            Act();
        }

        protected abstract void Arrange();

        protected abstract TSut CreateSystemUnderTest();

        protected abstract void Act();
    }
}