using System;
using System.Reflection;
using Glimpse.NH.Plumbing.Injectors;
using Glimpse.NH.Plumbing.Profiler;
using Glimpse.Test.NH.Core;
using Glimpse.Test.NH.Core.Extensions;
using NUnit.Framework;

namespace Glimpse.Test.NH
{
    [TestFixture]
    public abstract class When_the_GlimpseProfileDbDriverFactory_is_told_to_GetProfileDbDriverType : InstanceContextSpecification<IGlimpseProfileDbDriverFactory>
    {
        private Assembly _nhibernateAssembly;
        private Type _result;

        protected override void Arrange()
        {
            _nhibernateAssembly = Assembly.Load("NHibernate");
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
        public void It_should_return_the_profiled_db_driver_type()
        {
            _result.ShouldNotBeNull();
            _result.ShouldImplement<IGlimpseProfileDbDriver>();
            
            var version = _nhibernateAssembly.GetName().Version;
            var versionNumber = string.Format("{0}{1}{2}", version.Major, version.Minor, version.Build);
            _result.FullName.ShouldBeEqualTo(string.Format("Glimpse.NH.Plumbing.Profiler.GlimpseProfileDbDriverNh{0}", versionNumber));
        }
    }
}