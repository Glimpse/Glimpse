using System;
using System.ComponentModel;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node representing a .NET Framework type.
    /// </summary>
    public class TypeElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the type based on an assembly qualified name string.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        [TypeConverter(typeof(TypeConverter))]
        public Type Type
        {
            get { return (Type)base["type"]; }
            set { base["type"] = value; }
        }
    }
}