using System.Collections.Generic;

namespace Glimpse.Ado.Message
{
    public class CommandExecutedBatchParameter
    {
        public int Index { get; set; }

        public IList<CommandExecutedParamater> Value { get; set; }
    }
}