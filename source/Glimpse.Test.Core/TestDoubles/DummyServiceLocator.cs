using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummyServiceLocator:IServiceLocator
    {
        public T GetInstance<T>() where T : class
        {
            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }
    }
}