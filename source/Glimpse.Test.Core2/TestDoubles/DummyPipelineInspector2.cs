using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [GlimpsePipelineInspector]
    public class DummyPipelineInspector2 : IGlimpsePipelineInspector
    {
        public void Setup()
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotImplementedException("I am DummyPipelineInspector2");
        }

        public void Teardown()
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotImplementedException("I am DummyPipelineInspector2");
        }
    }
}