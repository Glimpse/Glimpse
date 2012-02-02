using System.Collections.Generic;
using System.Reflection;

namespace Glimpse.NH.Plumbing.Injectors
{
    public interface INHibernateInfoProvider
    {
        Assembly GetNhibernateAssembly();
        IEnumerable<INHibernateDriverInfo> GetNHibernateDriverInfos();
    }
}