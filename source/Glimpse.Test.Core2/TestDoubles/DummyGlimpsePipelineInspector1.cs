using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Test.Core2.TestDoubles
{
    [GlimpsePipelineInspector]
    public class DummyGlimpsePipelineInspector1:IGlimpsePipelineInspector
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