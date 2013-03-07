using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using Glimpse.Ado.AlternateType;

namespace Glimpse.EF.AlternateType.Asset
{
    internal class GlimpseDbConnectionFactory : IDbConnectionFactory
    { 
        private readonly IDbConnectionFactory inner;
        private readonly DbProviderFactory factory;
        private readonly ProviderStats stats;

        public GlimpseProfileDbConnectionFactory(IDbConnectionFactory inner, DbProviderFactory factory, ProviderStats stats)
        {
            this.inner = inner;
            this.factory = factory;
            this.stats = stats;
        }

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return new GlimpseProfileDbConnection(inner.CreateConnection(nameOrConnectionString), factory, stats, Guid.NewGuid());
        }

        public static void Initialize()
        {
            var current = Database.DefaultConnectionFactory;

            var dbConnection = current.CreateConnection("Anything");
            var provider = dbConnection.GetType().GetProperty("ProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbConnection, null);
            var providerType = typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(provider.GetType());

            Database.DefaultConnectionFactory = new GlimpseProfileDbConnectionFactory(current, (DbProviderFactory)providerType.GetField("Instance").GetValue(null), (ProviderStats)providerType.GetField("Stats").GetValue(null));
        } 
    }
}