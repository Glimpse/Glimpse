using System;
using Glimpse.NH.Plumbing.Injectors;
using Glimpse.Test.NH.Core;
using Glimpse.Test.NH.Core.Extensions;
using NUnit.Framework;

namespace Glimpse.Test.NH
{
    [TestFixture]
    public class When_the_GlimpseProfileDbDriverActivator_is_told_to_CreateProfileDbDriver_with_no_empty_constructor : InstanceContextSpecification<IGlimpseProfileDbDriverActivator>
    {
        private Type _profileDbDriverType;
        private Action _action;

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
            _action = () => Sut.CreateProfileDbDriver(_profileDbDriverType);
        }

        [Test]
        public void It_should_throw_an_invalid_operation_exception()
        {
            _action
                .ShouldThrow<InvalidOperationException>()
                .ShouldHaveMessage(string.Format("{0} should have a parameterless constructor", _profileDbDriverType));
        }

        private class GlimpseProfileDbDriverDummy
        {
            public GlimpseProfileDbDriverDummy(object parameter)
            {
            }
        }
    }
}