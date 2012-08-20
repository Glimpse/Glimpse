using System;

namespace Glimpse.Core.Extensibility
{
    public abstract class TabBase<T>:ITab
    {
        public abstract object GetData(ITabContext context);

        public abstract string Name{get; }

        public virtual RuntimeEvent ExecuteOn
        {
            get
            {
                return RuntimeEvent.EndRequest;
            }
        }

        public Type RequestContextType
        {
            get { return typeof (T); }
        }
    }
}