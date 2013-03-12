using System; 
using System.Data.Common;
using System.Data.Entity.Infrastructure; 
using System.Reflection; 
using Glimpse.Ado.AlternateType; 
using Glimpse.Core.Extensibility;

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDbConnectionFactory inner; 
        private readonly IInspectorContext stats;

        public GlimpseDbConnectionFactory(IDbConnectionFactory inner, IInspectorContext stats)
        {
            this.inner = inner; 
            this.stats = stats;
        }

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            var connection = inner.CreateConnection(nameOrConnectionString); 

            //We need to pull out the factory from the connection 
            var factory = connection.GetType().GetProperty("ProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(connection, null) as DbProviderFactory;
            if (factory != null)
            {
                if (!(factory is GlimpseDbProviderFactory))
                {
                    var factoryType = typeof(GlimpseDbProviderFactory<>).MakeGenericType(factory.GetType());
                    factory = (DbProviderFactory)factoryType.GetField("Instance").GetValue(null);
                }
            }

            return new GlimpseDbConnection(connection, factory, stats, Guid.NewGuid());
        }
    }
}
