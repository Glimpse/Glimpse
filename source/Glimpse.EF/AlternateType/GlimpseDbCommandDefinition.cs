#if EF43 || EF5
    using System.Data.Common; 
#else
    using System.Data.Entity.Core.Common; 
    using DbCommand = System.Data.Common.DbCommand; 
#endif
using Glimpse.Ado.AlternateType;

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
