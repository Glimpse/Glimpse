#if EF6Plus
using Glimpse.Ado.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework.Support;

namespace Glimpse.EF.Inspector.Core
{
    public class EntityDbProviderFactoryExecutionTask : IExecutionTask
    {
        public void Execute(ILogger logger)
        {
            //Find the registered providers 
            var table = Support.FindDbProviderFactoryTable();

            //Add in a new row so it can be discovered
            var newRow = table.NewRow();
            newRow["Name"] = "Entity Provider Factory";
            newRow["Description"] = "";
            newRow["InvariantName"] = typeof(System.Data.Entity.Core.EntityClient.EntityProviderFactory).FullName;
            newRow["AssemblyQualifiedName"] = typeof(GlimpseDbProviderFactory<System.Data.Entity.Core.EntityClient.EntityProviderFactory>).AssemblyQualifiedName;

            table.Rows.Add(newRow);
        }
    }
}
#endif