using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Test.Core.TestDoubles
{
    public class DummyPipelineInspector1:IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotSupportedException("I am DummyGlimpsePipelineInspector1");
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotSupportedException("I am DummyGlimpsePipelineInspector1");
        }
    }
}