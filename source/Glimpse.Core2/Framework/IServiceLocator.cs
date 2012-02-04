using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public interface IServiceLocator
    {
        T GetInstance<T>() where T: class;
        IEnumerable<T> GetAllInstances<T>() where T : class;
    }
}