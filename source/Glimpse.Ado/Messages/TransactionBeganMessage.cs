using System;
using System.Data;

namespace Glimpse.Ado.Messages
{
    public class TransactionBeganMessage : AdoTransactionMessage
    {        
        public IsolationLevel IsolationLevel { get; protected set; }

        public TransactionBeganMessage(Guid connectionId, Guid transactionId, IsolationLevel isolationLevel) 
            : base(connectionId, transactionId)
        {
            TransactionId = transactionId;
            IsolationLevel = isolationLevel;
        }
    }
}