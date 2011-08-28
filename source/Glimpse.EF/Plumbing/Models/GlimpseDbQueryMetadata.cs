using System.Collections.Generic;

namespace Glimpse.EF.Plumbing.Models
{
    internal class GlimpseDbQueryMetadata
    {
        public GlimpseDbQueryMetadata()
        {
            Connections = new Dictionary<string, GlimpseDbQueryConnectionMetadata>();
            Commands = new Dictionary<string, GlimpseDbQueryCommandMetadata>();
            Transactions = new Dictionary<string, GlimpseDbQueryTransactionMetadata>(); 
        }

        public IDictionary<string, GlimpseDbQueryConnectionMetadata> Connections { get; private set; }
        public IDictionary<string, GlimpseDbQueryCommandMetadata> Commands { get; private set; }
        public IDictionary<string, GlimpseDbQueryTransactionMetadata> Transactions { get; private set; }

        public IList<string> Warnings { get; private set; }
    }
}
