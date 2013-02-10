using System;

namespace Glimpse.Ado.Messages
{
    public abstract class AdoTransactionMessage : AdoMessage
    {
        public Guid TransactionId { get; protected set; }

        protected AdoTransactionMessage(Guid connectionId, Guid transactionId) : base(connectionId)
        {
            TransactionId = transactionId;
        }
    }
}