#if EF6Plus
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using Glimpse.Core.Framework.Support;
using Glimpse.EF.AlternateType;

namespace Glimpse.EF.Inspector.Core
{
    public class DbConfigurationExecutionTask : IExecutionTask
    { 
        public void Execute()
        {
            DbConfiguration.Loaded += (_, a) => 
                a.ReplaceService<DbProviderServices>((s, k) => 
                    s.GetType() == typeof(GlimpseDbProviderServices) ? s : new GlimpseDbProviderServices(s));
        }
    }
}
#endif