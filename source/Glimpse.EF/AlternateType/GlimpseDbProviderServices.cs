using System.Data.Common;
using System.Data.Common.CommandTrees;
using System.Data.Metadata.Edm;
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;

namespace Glimpse.EF.AlternateType
{
    internal class GlimpseDbProviderServices : DbProviderServices
    {
        public GlimpseDbProviderServices(DbProviderServices innerProviderServices)
        {
            InnerProviderServices = innerProviderServices; 
        }

        private DbProviderServices InnerProviderServices { get; set; } 

        public override DbCommandDefinition CreateCommandDefinition(DbCommand prototype)
        {
            return new GlimpseDbCommandDefinition(InnerProviderServices.CreateCommandDefinition(prototype));
        }

        protected override DbCommandDefinition CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree)
        {
            return new GlimpseDbCommandDefinition(InnerProviderServices.CreateCommandDefinition(commandTree));
        }

        protected override void DbCreateDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            InnerProviderServices.CreateDatabase(((GlimpseDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override string DbCreateDatabaseScript(string providerManifestToken, StoreItemCollection storeItemCollection)
        {
            return InnerProviderServices.CreateDatabaseScript(providerManifestToken, storeItemCollection);
        }

        protected override bool DbDatabaseExists(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            return InnerProviderServices.DatabaseExists(((GlimpseDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override void DbDeleteDatabase(DbConnection connection, int? commandTimeout, StoreItemCollection storeItemCollection)
        {
            InnerProviderServices.DeleteDatabase(((GlimpseDbConnection)connection).InnerConnection, commandTimeout, storeItemCollection);
        }

        protected override DbProviderManifest GetDbProviderManifest(string manifestToken)
        {
            return InnerProviderServices.GetProviderManifest(manifestToken);
        }

        protected override string GetDbProviderManifestToken(DbConnection connection)
        {
            return InnerProviderServices.GetProviderManifestToken(((GlimpseDbConnection)connection).InnerConnection);
        }
    }
}