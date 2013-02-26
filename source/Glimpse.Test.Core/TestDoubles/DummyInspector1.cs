using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummyInspector1 : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            // Simple IInspector for testing
            throw new NotSupportedException("I am DummyInspector1");
        }
    }
}