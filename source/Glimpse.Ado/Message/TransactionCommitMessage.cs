using System;

namespace Glimpse.Ado.Message
{
    public class TransactionCommitMessage : AdoTransactionMessage
    {
        public TransactionCommitMessage(Guid connectionId, Guid transactionId) : base(connectionId, transactionId)
        {
        }
    }
}