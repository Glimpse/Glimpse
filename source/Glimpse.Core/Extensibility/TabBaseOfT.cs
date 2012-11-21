using System;

namespace Glimpse.Core.Extensibility
{
    public abstract class TabBase<T> : ITab
    {
        public abstract string Name { get; }

        public virtual RuntimeEvent ExecuteOn
        {
            get
            {
                return RuntimeEvent.EndRequest;
            }
        }

        public Type RequestContextType
        {
            get { return typeof(T); }
        }

        public abstract object GetData(ITabContext context);
    }
}