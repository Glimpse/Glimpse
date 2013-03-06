using System;

namespace Glimpse.Ado.Message
{
    public class TransactionRollbackMessage : AdoTransactionMessage
    {
        public TransactionRollbackMessage(Guid connectionId, Guid transactionId) : base(connectionId, transactionId)
        {
        }
    }
}