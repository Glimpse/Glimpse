using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using System.Web;

namespace Glimpse.EF.Plumbing
{
    internal class ProviderStats
    {
        public bool IsEnabled { get { return true; } }

        public void ConnectionDisposed(Guid connectionId)
        {
            if (HttpContext.Current != null)
            {
                var connection = PullConnection(connectionId.ToString());
                connection.RegisterEnd(DateTime.Now);  
            } 

            //Trace.TraceInformation("ConnectionDisposed - connectionId = {0}", connectionId);
        }

        public void ConnectionStarted(Guid connectionId)
        {
            if (HttpContext.Current != null)
            {
                var connection = PullConnection(connectionId.ToString());
                connection.RegisterStart(DateTime.Now);
            }


            //Trace.TraceInformation("ConnectionStarted - connectionId = {0}", connectionId);
        }

        public void TransactionCommit(Guid connectionId)
        {
            Trace.TraceInformation("TransactionCommit - connectionId = {0}", connectionId);
        }

        public void TransactionBegan(Guid connectionId, System.Data.IsolationLevel isolationLevel)
        {
            Trace.TraceInformation("TransactionBegan - connectionId = {0}, isolationLevel = {1}", connectionId, isolationLevel);
        }

        public void TransactionDisposed(Guid connectionId)
        {
            Trace.TraceInformation("TransactionDisposed - connectionId = {0}", connectionId);
        }

        public void TransactionRolledBack(Guid connectionId)
        {
            Trace.TraceInformation("TransactionRolledBack - connectionId = {0}", connectionId);
        }

        public void DtcTransactionEnlisted(Guid connectionId, System.Transactions.IsolationLevel isolationLevel)
        {
            Trace.TraceInformation("DtcTransactionEnlisted - connectionId = {0}, isolationLevel = {1}", connectionId, isolationLevel);
        }

        public void DtcTransactionCompleted(Guid connectionId, TransactionStatus aborted)
        {
            Trace.TraceInformation("DtcTransactionCompleted - connectionId = {0}, aborted = {1}", connectionId, aborted);
        }

        public void CommandError(Guid connectionId, Guid commandId, Exception exception)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                command.Exception = exception;
            }


            //Trace.TraceInformation("CommandError - connectionId = {0}, exception = {1}", connectionId, exception);
        }

        public void CommandDurationAndRowCount(Guid connectionId, Guid commandId, long elapsedMilliseconds, int? recordsAffected)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                command.ElapsedMilliseconds = elapsedMilliseconds;
                command.RecordsAffected = recordsAffected;
            }


            //Trace.TraceInformation("CommandDurationAndRowCount - connectionId = {0}, elapsedMilliseconds = {1}, recordsAffected = {2}", connectionId, elapsedMilliseconds, recordsAffected);
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


            //Trace.TraceInformation("CommandExecuted - connectionId = {0}, commandId = {1}, toString = {2}", connectionId, commandId, commandString);
        }

        /// <summary>
        /// Used to register the amount of rows that are in a DataReader
        /// </summary> 
        public void CommandRowCount(Guid connectionId, Guid commandId, int rowCount)
        {
            if (HttpContext.Current != null)
            {
                var command = PullCommand(connectionId.ToString(), commandId.ToString());
                command.TotalRecords = rowCount;
            }


            //Trace.TraceInformation("CommandExecuted - connectionId = {0}, commandId = {1}, rowCount = {2}", connectionId, commandId, rowCount);
        }

        #region Support Memebers
         
        protected GlimpseDbQueryMetadata Metadata
        {
            get
            { 
                var contextStore = HttpContext.Current.Items;//Can this be removed?
                var metadata = contextStore[Plugin.EF.StoreKey] as GlimpseDbQueryMetadata;
                if (metadata == null)
                    contextStore[Plugin.EF.StoreKey] = metadata = new GlimpseDbQueryMetadata();
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
                connection.Commands.Add(commandId, command);
            }
            return command;
        }

        #endregion 
    }
}
