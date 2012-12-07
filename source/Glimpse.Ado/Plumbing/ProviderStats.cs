using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web;
using Glimpse.Ado.Plumbing.Models;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Plumbing
{
    public class ProviderStats
    {
        public ProviderStats()
        {
            
        }

        public static IMessageBroker MessageBroker { get; set; }

        public bool IsEnabled { get { return true; } }

        public void ConnectionStarted(Guid connectionId)
        {
            if (HttpContext.Current != null)
            {
                var connection = PullConnection(connectionId.ToString());
                connection.RegisterStart(DateTime.Now); 
            } 
        }

        public void ConnectionClosed(Guid connectionId)
        {
            if (HttpContext.Current != null)
            {
                var connection = PullConnection(connectionId.ToString());
                connection.RegisterEnd(DateTime.Now);
            }
        }

        public void TransactionBegan(Guid connectionId, Guid transactionId, System.Data.IsolationLevel isolationLevel)
        {
            if (HttpContext.Current != null)
            {
                var transaction = PullTranasction(connectionId.ToString(), transactionId.ToString());
                transaction.IsolationLevel = isolationLevel.ToString();

                var connection = PullConnection(connectionId.ToString());
                connection.RegiserTransactionStart(transaction);
            }  
        }

        public void TransactionCommit(Guid connectionId, Guid transactionId)
        {
            TransactionComplete(connectionId, transactionId, true); 
        }

        public void TransactionRolledBack(Guid connectionId, Guid transactionId)
        {
            TransactionComplete(connectionId, transactionId, false); 
        }

        private void TransactionComplete(Guid connectionId, Guid transactionId, bool committed)
        {
            if (HttpContext.Current != null)
            {
                var transaction = PullTranasction(connectionId.ToString(), transactionId.ToString());
                transaction.Committed = committed;

                var connection = PullConnection(connectionId.ToString());
                connection.RegiserTransactionEnd(transaction);
            }
        } 

        public void DtcTransactionEnlisted(Guid connectionId, System.Transactions.IsolationLevel isolationLevel)
        {
            //Trace.TraceInformation("DtcTransactionEnlisted - connectionId = {0}, isolationLevel = {1}", connectionId, isolationLevel);
        }

        public void DtcTransactionCompleted(Guid connectionId, TransactionStatus aborted)
        {
            //Trace.TraceInformation("DtcTransactionCompleted - connectionId = {0}, aborted = {1}", connectionId, aborted);
        }

        public void CommandError(Guid connectionId, Guid commandId, Exception exception)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                command.Exception = exception;
            } 
        }

        public void CommandDurationAndRowCount(Guid connectionId, Guid commandId, long elapsedMilliseconds, int? recordsAffected)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                command.ElapsedMilliseconds = elapsedMilliseconds;
                command.RecordsAffected = recordsAffected;
            } 
        }

        public void CommandExecuted(Guid connectionId, Guid commandId, string commandString, IEnumerable<Tuple<string, object, string, int>> parameters)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        var par = new GlimpseDbQueryCommandParameterMetadata
                                      {
                                          Name = parameter.Item1,
                                          Value = parameter.Item2,
                                          Type = parameter.Item3,
                                          Size = parameter.Item4
                                      };
                        command.Parameters.Add(par);
                    }
                }
                command.Command = commandString; 
            } 
        }
         
        public void CommandRowCount(Guid connectionId, Guid commandId, int rowCount)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                command.TotalRecords = rowCount;
            } 
        }

        #region Support Memebers
         
        protected GlimpseDbQueryMetadata Metadata
        {
            get
            { 
                var contextStore = HttpContext.Current.Items;//Can this be removed?
                var metadata = contextStore[Plugin.SQL.StoreKey] as GlimpseDbQueryMetadata;
                if (metadata == null)
                    contextStore[Plugin.SQL.StoreKey] = metadata = new GlimpseDbQueryMetadata();
                return metadata;
            }
        }

        protected GlimpseDbQueryConnectionMetadata PullConnection(string connectionId)
        { 
            GlimpseDbQueryConnectionMetadata connection;
            if (!Metadata.Connections.TryGetValue(connectionId, out connection))
            {
                connection = new GlimpseDbQueryConnectionMetadata(connectionId);
                Metadata.Connections.Add(connectionId, connection);
            }
            return connection;
        }

        protected GlimpseDbQueryCommandMetadata PullCommand(string connectionId, string commandId)
        {
            GlimpseDbQueryCommandMetadata command;
            if (!Metadata.Commands.TryGetValue(commandId, out command))
            {
                command = new GlimpseDbQueryCommandMetadata(commandId, connectionId);
                Metadata.Commands.Add(commandId, command);

                var connection = PullConnection(connectionId);
                connection.RegiserCommand(command);
            }
            return command;
        }

        protected GlimpseDbQueryTransactionMetadata PullTranasction(string connectionId, string transactionId)
        {
            GlimpseDbQueryTransactionMetadata transaction;
            if (!Metadata.Transactions.TryGetValue(transactionId, out transaction))
            {
                transaction = new GlimpseDbQueryTransactionMetadata(transactionId, connectionId);
                Metadata.Transactions.Add(transactionId, transaction); 
            }
            return transaction; 
        }

        #endregion 
    }
}