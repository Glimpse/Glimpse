using System.Data.Common; 
using System.Reflection; 

namespace Glimpse.Ado.AlternateType
{
    public static class Support
    {
        public static DbProviderFactory TryGetProviderFactory(this DbConnection connection)
        {
            // If we can pull it out quickly and easily
            var profiledConnection = connection as GlimpseDbConnection;
            if (profiledConnection != null)
            {
                return profiledConnection.InnerProviderFactory;
            }

#if (NET45)
            return DbProviderFactories.GetFactory(connection);
#else
            return connection.GetType().GetProperty("ProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(connection, null) as DbProviderFactory;
#endif
        }

        public static DbProviderFactory TryGetProfiledProviderFactory(this DbConnection connection)
        {
            var factory = connection.TryGetProviderFactory();
            if (factory != null && !(factory is GlimpseDbProviderFactory))
            { 
                var factoryType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType());
                factory = (DbProviderFactory)factoryType.GetField("Instance").GetValue(null); 
            }

            return factory;
        }
    }
}
