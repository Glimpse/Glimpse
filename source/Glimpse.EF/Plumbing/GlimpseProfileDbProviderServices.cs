using System.Data.Common;
using System.Data.Common.CommandTrees;
using System.Data.Metadata.Edm;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbProviderServices : DbProviderServices
    { 
        private readonly DbProviderServices _inner;
        private readonly ProviderStats _stats;

        public GlimpseProfileDbProviderServices(DbProviderServices inner, ProviderStats stats)
        {
            _inner = inner;
            _stats = stats;
        }

        public override DbCommandDefinition CreateCommandDefinition(DbCommand prototype)
        {
            return new GlimpseProfileDbCommandDefinition(_inner.CreateCommandDefinition(prototype), _stats);
        }

        protected override DbCommandDefinition CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree)
        {
            return new GlimpseProfileDbCommandDefinition(_inner.CreateCommandDefinition(commandTree), _stats);
        }

        protected override void DbCreateDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            _inner.CreateDatabase(((GlimpseProfileDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override string DbCreateDatabaseScript(string providerManifestToken, StoreItemCollection storeItemCollection)
        {
            return _inner.CreateDatabaseScript(providerManifestToken, storeItemCollection);
        }

        protected override bool DbDatabaseExists(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            return _inner.DatabaseExists(((GlimpseProfileDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override void DbDeleteDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            _inner.DeleteDatabase(((GlimpseProfileDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override DbProviderManifest GetDbProviderManifest(string manifestToken)
        {
            return _inner.GetProviderManifest(manifestToken);
        }

        protected override string GetDbProviderManifestToken(DbConnection connection)
        {
            return _inner.GetProviderManifestToken(((GlimpseProfileDbConnection)connection).InnerConnection);
        } 
    }
}