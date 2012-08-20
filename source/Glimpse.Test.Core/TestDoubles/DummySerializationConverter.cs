using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummySerializationConverter:ISerializationConverter
    {
        public IEnumerable<Type> SupportedTypes
        {
            get { return new[] {typeof (DummyObjectContext)}; }
        }

        public object Convert(object obj)
        {
            throw new NotSupportedException();
        }
    }
}
