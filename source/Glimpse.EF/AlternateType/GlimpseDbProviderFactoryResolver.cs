#if EF6Plus
using System.Data.Common;
using System.Data.Entity.Infrastructure; 
using Glimpse.Ado.AlternateType;

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbProviderFactoryResolver : IDbProviderFactoryResolver
    {
        private readonly IDbProviderFactoryResolver innerFactoryService;
         
        public GlimpseDbProviderFactoryResolver(IDbProviderFactoryResolver innerFactoryService)
        {
            this.innerFactoryService = innerFactoryService;
        }

        public DbProviderFactory ResolveProviderFactory(DbConnection connection)
        {
            var factory = innerFactoryService.ResolveProviderFactory(connection);

            return factory.WrapProviderFactory(); 
        }
    }
}
#endif