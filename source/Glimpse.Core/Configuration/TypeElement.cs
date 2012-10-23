using System;
using System.ComponentModel;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class TypeElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        [TypeConverter(typeof(TypeConverter))]
        public Type Type
        {
            get { return (Type)base["type"]; }
            set { base["type"] = value; }
        }
    }
}