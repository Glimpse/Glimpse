using System.Data.Common;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbCommandDefinition : DbCommandDefinition
    {
        private DbCommandDefinition InnerCommandDefinition { get; set; }
        private ProviderStats Stats { get; set; }

        public GlimpseProfileDbCommandDefinition(DbCommandDefinition innerCommandDefinition, ProviderStats stats)
        {
            InnerCommandDefinition = innerCommandDefinition;
            Stats = stats;
        }

        public override DbCommand CreateCommand()
        {
            return new GlimpseProfileDbCommand(InnerCommandDefinition.CreateCommand(), Stats);
        }
    }
}
