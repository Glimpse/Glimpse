using System.Data.Common;

namespace Glimpse.Ado.Plumbing.Profiler
{
    internal class GlimpseProfileDbCommandDefinition : DbCommandDefinition
    {
        public GlimpseProfileDbCommandDefinition(DbCommandDefinition innerCommandDefinition, ProviderStats stats)
        {
            InnerCommandDefinition = innerCommandDefinition;
            Stats = stats;
        }


        private DbCommandDefinition InnerCommandDefinition { get; set; }
        private ProviderStats Stats { get; set; }


        public override DbCommand CreateCommand()
        {
            return new GlimpseProfileDbCommand(InnerCommandDefinition.CreateCommand(), Stats);
        }
    }
}
