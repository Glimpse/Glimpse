using Glimpse.Test.NH;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Glimpse.Test.NH212
{
    [TestFixture]
    public class When_the_WrapDbDriver_is_told_to_Inject_for_NH212 : When_the_WrapDbDriver_is_told_to_Inject
    {
        protected override void BuildSessionFactory()
        {
            new Configuration()
                .Configure()
                .BuildSessionFactory();
        }
    }
}