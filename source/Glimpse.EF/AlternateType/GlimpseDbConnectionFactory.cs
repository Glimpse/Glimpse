using System.Data.Common;
using System.Data.Entity.Infrastructure;
using Glimpse.Ado.AlternateType;  

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbConnectionFactory : IDbConnectionFactory
    { 
        public GlimpseDbConnectionFactory(IDbConnectionFactory inner)
        {
            Inner = inner;  
        }

        private IDbConnectionFactory Inner { get; set; } 

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            var connection = Inner.CreateConnection(nameOrConnectionString);

            return connection is GlimpseDbConnection ? connection : new GlimpseDbConnection(connection);
        } 
    }
}
