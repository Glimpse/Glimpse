using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.PipelineInspector
{
    [PipelineInspector]
    public class Routes:IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}