using System.Data.Common;
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;

namespace Glimpse.EF.AlternateType
{
    public class GlimpseDbCommandDefinition : DbCommandDefinition
    {
        public GlimpseDbCommandDefinition(DbCommandDefinition innerCommandDefinition)
        {
            InnerCommandDefinition = innerCommandDefinition; 
        }
         
        private DbCommandDefinition InnerCommandDefinition { get; set; }

        public override DbCommand CreateCommand()
        {
            return new GlimpseDbCommand(InnerCommandDefinition.CreateCommand());
        }
    }
}
