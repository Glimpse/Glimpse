using System;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class EnvironmentsCollection:ConfigurationElementCollection
    {
        public void Add(Environment environment)
        {
            BaseAdd(environment);
        }

        public Environment this[int index]
        {
            get { return BaseGet(index) as Environment; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public Environment GetCurrent(Uri requestUri)
        {
            foreach (Environment environment in this)
            {
                if (environment.Authority == requestUri.Authority)
                    return environment;
            }

            return null;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Environment();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }
}