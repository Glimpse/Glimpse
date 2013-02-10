using System.Data.Common;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Plumbing.Profiler
{
    internal class GlimpseProfileDbCommandDefinition : DbCommandDefinition
    {
        public GlimpseProfileDbCommandDefinition(
            DbCommandDefinition innerCommandDefinition, IPipelineInspectorContext context, ProviderStats stats)
        {
            InnerCommandDefinition = innerCommandDefinition;
            Stats = stats;
            InspectorContext = context;
        }


        private DbCommandDefinition InnerCommandDefinition { get; set; }
        private ProviderStats Stats { get; set; }
        private IPipelineInspectorContext InspectorContext { get; set; }

        public override DbCommand CreateCommand()
        {
            return new GlimpseProfileDbCommand(InnerCommandDefinition.CreateCommand(), InspectorContext, Stats);
        }
    }
}
