using System;
using Glimpse.NH.Plumbing.Injectors;
using Glimpse.NH.Plumbing.Profiler;
using Glimpse.Test.NH.Core;
using Glimpse.Test.NH.Core.Extensions;
using NUnit.Framework;

namespace Glimpse.Test.NH
{
    [TestFixture]
    public class When_the_GlimpseProfileDbDriverActivator_is_told_to_CreateProfileDbDriver : InstanceContextSpecification<IGlimpseProfileDbDriverActivator>
    {
        private Type _profileDbDriverType;
        private IGlimpseProfileDbDriver _result;

        protected override void Arrange()
        {
            _profileDbDriverType = typeof(GlimpseProfileDbDriverDummy);
        }

        protected override IGlimpseProfileDbDriverActivator CreateSystemUnderTest()
        {
            return new GlimpseProfileDbDriverActivator();
        }

        protected override void Act()
        {
            _result = Sut.CreateProfileDbDriver(_profileDbDriverType);
        }

        [Test]
        public void It_should_return_the_profiled_db_driver()
        {
            _result.ShouldNotBeNull();
            _result.ShouldBeOfType<GlimpseProfileDbDriverDummy>();
        }

        private class GlimpseProfileDbDriverDummy : IGlimpseProfileDbDriver
        {
            public void Wrap(object driver)
            {
                throw new NotImplementedException();
            }
        }
    }
}