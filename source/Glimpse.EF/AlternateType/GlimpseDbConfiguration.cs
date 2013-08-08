#if EF6Plus
using System.Data.Entity.Config;

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbConfiguration : DbConfiguration
    {
        public GlimpseDbConfiguration()
        {
            AddDependencyResolver(new GlimpseDbDependencyResolver(this));
        }
    }
}
#endif