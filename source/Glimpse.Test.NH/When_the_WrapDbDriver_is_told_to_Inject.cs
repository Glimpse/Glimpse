using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.NH.Plumbing.Injectors;
using Glimpse.Test.NH.Core;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.NH
{
    [TestFixture]
    public abstract class When_the_WrapDbDriver_is_told_to_Inject : InstanceContextSpecification<WrapDbDriver>
    {
        private Mock<IGlimpseLogger> _logger;
        private NHibernateInfoProvider _nhibernateInfoProvider;
        private GlimpseProfileDbDriverFactory _profileDbDriverFactory;
        private GlimpseProfileDbDriverActivator _profileDbDriverActivator;

        protected override void Arrange()
        {
            _logger = new Mock<IGlimpseLogger>();
            _nhibernateInfoProvider = new NHibernateInfoProvider();
            _profileDbDriverFactory = new GlimpseProfileDbDriverFactory();
            _profileDbDriverActivator = new GlimpseProfileDbDriverActivator();

            BuildSessionFactory();
        }

        protected override WrapDbDriver CreateSystemUnderTest()
        {
            return new WrapDbDriver(_logger.Object, _nhibernateInfoProvider, _profileDbDriverFactory, _profileDbDriverActivator);
        }

        protected override void Act()
        {
            Sut.Inject();
        }

        protected abstract void BuildSessionFactory();

        [Test]
        public void It_should_wrap_all_drivers()
        {
            var nhibernateDriverInfos = _nhibernateInfoProvider.GetNHibernateDriverInfos();
            Assert.AreEqual(1, nhibernateDriverInfos.Count());

            foreach (var nhibernateDriverInfo in nhibernateDriverInfos)
            {
                Assert.IsTrue(nhibernateDriverInfo.IsWrapped());
            }
        }
    }
}