using System.Data.Common;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.AlternateType
{
    internal class GlimpseDbCommandDefinition : DbCommandDefinition
    {
        public GlimpseDbCommandDefinition(DbCommandDefinition innerCommandDefinition, IInspectorContext context)
        {
            InnerCommandDefinition = innerCommandDefinition;
            InspectorContext = context;
        }


        private DbCommandDefinition InnerCommandDefinition { get; set; }  
     
        private IInspectorContext InspectorContext { get; set; }

        public override DbCommand CreateCommand()
        {
            return new GlimpseDbCommand(InnerCommandDefinition.CreateCommand(), InspectorContext);
        }
    }
}
