using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbProviderFactory : IDbConnectionFactory
    { 
        private readonly IDbConnectionFactory _inner;
        private readonly DbProviderFactory _factory;
        private readonly ProviderStats _stats;

        public GlimpseProfileDbProviderFactory(IDbConnectionFactory inner, DbProviderFactory factory, ProviderStats stats)
        {
            _inner = inner;
            _factory = factory;
            _stats = stats;
        }

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return new GlimpseProfileDbConnection(this._inner.CreateConnection(nameOrConnectionString), _factory, _stats, Guid.NewGuid());
        }

        public static void Initialize()
        {
            var current = Database.DefaultConnectionFactory;

            var dbConnection = current.CreateConnection("Anything");
            var provider = dbConnection.GetType().GetProperty("ProviderFactory", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbConnection, null);
            var providerType = typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(provider.GetType());

            Database.DefaultConnectionFactory = new GlimpseProfileDbProviderFactory(current, (DbProviderFactory)providerType.GetField("Instance").GetValue(null), (ProviderStats)providerType.GetField("Stats").GetValue(null));
        } 
    }
}