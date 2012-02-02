using System;
using System.Reflection;
using Glimpse.NH.Plumbing.Injectors;
using Glimpse.Test.NH.Core;
using Glimpse.Test.NH.Core.Extensions;
using NUnit.Framework;

namespace Glimpse.Test.NH
{
    [TestFixture]
    public class When_the_GlimpseProfileDbDriverFactory_is_told_to_GetProfileDbDriverType_with_no_input_nhibernate_assembly : InstanceContextSpecification<IGlimpseProfileDbDriverFactory>
    {
        private Assembly _nhibernateAssembly;
        private Type _result;

        protected override void Arrange()
        {
            _nhibernateAssembly = null;
        }

        protected override IGlimpseProfileDbDriverFactory CreateSystemUnderTest()
        {
            return new GlimpseProfileDbDriverFactory();
        }

        protected override void Act()
        {
            _result = Sut.GetProfileDbDriverType(_nhibernateAssembly);
        }

        [Test]
        public void It_should_return_null()
        {
            _result.ShouldBeNull();
        }
    }
}