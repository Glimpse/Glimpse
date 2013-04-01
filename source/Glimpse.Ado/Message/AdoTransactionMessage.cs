using System;

using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public abstract class AdoTransactionMessage : AdoMessage
    {
        public Guid TransactionId { get; protected set; }

        protected AdoTransactionMessage(Guid connectionId, Guid transactionId) 
            : base(connectionId)
        {
            TransactionId = transactionId;
        }
    }
}