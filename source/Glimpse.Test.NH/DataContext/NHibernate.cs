namespace Glimpse.Test.NH.DataContext
{
    public static class NHibernate
    {
        public static INHibernateContext Version(NHibernateVersion version)
        {
            return new NHibernateContext(version);
        }
    }
}