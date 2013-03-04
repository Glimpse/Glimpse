using System;

namespace Glimpse.Ado.Messages
{
    public class TransactionRollbackMessage : AdoTransactionMessage
    {
        public TransactionRollbackMessage(Guid connectionId, Guid transactionId) : base(connectionId, transactionId)
        {
        }
    }
}