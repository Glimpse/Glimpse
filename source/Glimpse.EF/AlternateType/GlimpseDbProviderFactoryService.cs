using System.Data.Common;
using System.Data.Entity.Infrastructure; 
using Glimpse.Ado.AlternateType;

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbProviderFactoryService : IDbProviderFactoryService
    {
        private readonly IDbProviderFactoryService innerFactoryService;
         
        public GlimpseDbProviderFactoryService(IDbProviderFactoryService innerFactoryService)
        {
            this.innerFactoryService = innerFactoryService;
        }

        public DbProviderFactory GetProviderFactory(DbConnection connection)
        {
            var factory = innerFactoryService.GetProviderFactory(connection);

            return factory.WrapProviderFactory(); 
        }
    }
}