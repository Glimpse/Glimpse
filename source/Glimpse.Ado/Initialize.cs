using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Ado.Inspector.Core;

namespace Glimpse.Ado
{
    public static class Initialize
    {
        public static void Ado(this Glimpse.Core.Setting.Initializer initializer)
        {
            var wrapDbProviderFactories = new WrapDbProviderFactories();
            wrapDbProviderFactories.Execute();
        }
    }
}
