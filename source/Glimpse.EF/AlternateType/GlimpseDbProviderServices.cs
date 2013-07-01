using System.Reflection;
#if EF43 || EF5
    using System.Data.Common;
    using System.Data.Common.CommandTrees;
    using System.Data.Metadata.Edm;  
#else
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.Core.Metadata.Edm;
    using DbCommand = System.Data.Common.DbCommand;
    using DbConnection = System.Data.Common.DbConnection;
    using System.Data.Entity.Spatial;
#endif
#if EF5 && NET45
    using System.Data.Spatial;
#endif
using Glimpse.Ado.AlternateType;

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

#if (EF5 && NET45) || EF6Plus
        //HACK: GetSpatialDataReader should be virtual, shouldn't have to do this

        private static MethodInfo GetDbSpatialDataReaderMethod = typeof(DbProviderServices).GetMethod("GetDbSpatialDataReader", BindingFlags.NonPublic | BindingFlags.Instance);

        protected override DbSpatialDataReader GetDbSpatialDataReader(System.Data.Common.DbDataReader fromReader, string manifestToken)
        {
            return (DbSpatialDataReader)GetDbSpatialDataReaderMethod.Invoke(InnerProviderServices, new object[] { ((GlimpseDbDataReader)fromReader).InnerDataReader, manifestToken });
        }
#endif
    }
}