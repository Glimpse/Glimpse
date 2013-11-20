using System.Collections.Generic;

namespace Glimpse.Ado.Model
{
    public class QueryMetadata
    {
        public QueryMetadata()
        {
            Connections = new Dictionary<string, ConnectionMetadata>();
            Commands = new Dictionary<string, CommandMetadata>();
            Transactions = new Dictionary<string, TransactionMetadata>(); 
        }
         
        public IDictionary<string, ConnectionMetadata> Connections { get; private set; }

        public IDictionary<string, CommandMetadata> Commands { get; private set; }

        public IDictionary<string, TransactionMetadata> Transactions { get; private set; } 

        public IList<string> Warnings { get; private set; }
    } 
}
