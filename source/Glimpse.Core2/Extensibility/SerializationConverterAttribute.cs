using System;
using System.ComponentModel.Composition;

namespace Glimpse.Core2.Extensibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SerializationConverterAttribute:ExportAttribute
    {
        public SerializationConverterAttribute():base(typeof(ISerializationConverter)){}
    }
}