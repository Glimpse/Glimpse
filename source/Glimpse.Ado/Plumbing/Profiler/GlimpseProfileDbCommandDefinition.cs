using System.Data.Common;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Plumbing.Profiler
{
    internal class GlimpseProfileDbCommandDefinition : DbCommandDefinition
    {
        public GlimpseProfileDbCommandDefinition(
            DbCommandDefinition innerCommandDefinition, IInspectorContext context)
        {
            InnerCommandDefinition = innerCommandDefinition;
            InspectorContext = context;
        }


        private DbCommandDefinition InnerCommandDefinition { get; set; }       
        private IInspectorContext InspectorContext { get; set; }

        public override DbCommand CreateCommand()
        {
            return new GlimpseProfileDbCommand(InnerCommandDefinition.CreateCommand(), InspectorContext);
        }
    }
}
