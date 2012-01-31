using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [PipelineInspector]
    public class DummyPipelineInspector2 : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotSupportedException("I am DummyPipelineInspector2");
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotSupportedException("I am DummyPipelineInspector2");
        }
    }
}