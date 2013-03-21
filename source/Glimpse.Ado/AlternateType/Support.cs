using System;
using System.Data;
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
            if (factory != null)
            { 
                if (!(factory is GlimpseDbProviderFactory))
                {
                    factory = factory.WrapProviderFactory(); 
                }
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.DbFactoryNotFoundInDbConnection, connection.GetType().FullName));
            }

            return factory;
        }

        public static DbProviderFactory WrapProviderFactory(this DbProviderFactory factory)
        {
            if (!(factory is GlimpseDbProviderFactory))
            { 
                var factoryType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType());
                return (DbProviderFactory)factoryType.GetField("Instance").GetValue(null);    
            }

            return factory;
        }

        public static DataTable FindDbProviderFactoryTable()
        {
            var dbProviderFactories = typeof(DbProviderFactories);
            var providerField = dbProviderFactories.GetField("_configTable", BindingFlags.NonPublic | BindingFlags.Static) ?? dbProviderFactories.GetField("_providerTable", BindingFlags.NonPublic | BindingFlags.Static);
            var registrations = providerField.GetValue(null);
            return registrations is DataSet ? ((DataSet)registrations).Tables["DbProviderFactories"] : (DataTable)registrations;
        }
    }
}
