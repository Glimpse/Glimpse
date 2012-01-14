using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [PipelineInspector]
    public class DummyPipelineInspector1:IPipelineInspector
    {
        public void Setup()
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotImplementedException("I am DummyGlimpsePipelineInspector1");
        }

        public void Teardown()
        {
            //Simple IGlimpsePipelineInspector for testing
            throw new NotImplementedException("I am DummyGlimpsePipelineInspector1");
        }
    }
}