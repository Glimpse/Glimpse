using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Glimpse.Ado.Messages;
using Glimpse.Core.Extensibility;

namespace Glimpse.Ado.Plumbing.Profiler
{
    public class GlimpseProfileDbCommand : DbCommand
    {
        public GlimpseProfileDbCommand(DbCommand innerCommand, IPipelineInspectorContext context)
        {
            InnerCommand = innerCommand;
            InspectorContext = context;
        }

        public GlimpseProfileDbCommand(DbCommand innerCommand, IPipelineInspectorContext context, GlimpseProfileDbConnection connection):
            this(innerCommand, context)
        {
            InnerConnection = connection;
            InspectorContext = context;
        }


        private DbCommand InnerCommand { get; set; }
        private GlimpseProfileDbConnection InnerConnection { get; set; } 
        private IPipelineInspectorContext InspectorContext { get; set; }



        public override string CommandText
        {
            get { return InnerCommand.CommandText; }
            set { InnerCommand.CommandText = value; }
        }

        public override int CommandTimeout
        {
            get { return InnerCommand.CommandTimeout; }
            set { InnerCommand.CommandTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return InnerCommand.CommandType; }
            set { InnerCommand.CommandType = value; }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return InnerCommand.Parameters; }
        }

        public override bool DesignTimeVisible
        {
            get { return InnerCommand.DesignTimeVisible; }
            set { InnerCommand.DesignTimeVisible = value; }
        }

        public override ISite Site
        {
            get { return InnerCommand.Site; }
            set { InnerCommand.Site = value; }
        } 

        public override UpdateRowSource UpdatedRowSource
        {
            get { return InnerCommand.UpdatedRowSource; }
            set { InnerCommand.UpdatedRowSource = value; }
        }

        public override void Cancel()
        {
            InnerCommand.Cancel();
        }

        public override void Prepare()
        {
            InnerCommand.Prepare();
        }

        public bool BindByName
        {
            get
            {
                var property = InnerCommand.GetType().GetProperty("BindByName");
                if (property == null) 
                    return false;
                return (bool)property.GetValue(InnerCommand, null);
            }
            set
            {
                var property = InnerCommand.GetType().GetProperty("BindByName");
                if (property != null)
                    property.SetValue(InnerCommand, value, null); 
            }
        }

        protected override DbConnection DbConnection
        {
            get { return InnerConnection; }
            set
            {
                InnerConnection = value as GlimpseProfileDbConnection;
                if (InnerConnection != null)
                    InnerCommand.Connection = InnerConnection.InnerConnection;
                else
                {
                    // Create a new GlimpseProfileDbConnection, this will happen when using a EntityConnection(and created with a SqlConnection for example) as a argument to ObjectContext constructor.
                    var factory = (DbProviderFactory)typeof(GlimpseProfileDbProviderFactory<>).MakeGenericType(DbProviderServices.GetProviderFactory(value).GetType()).GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                    InnerConnection = new GlimpseProfileDbConnection(value, factory, InspectorContext, Guid.NewGuid());
                    InnerCommand.Connection = InnerConnection.InnerConnection;
                }
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                if (InnerCommand.Transaction == null)
                    return null; 
                return new GlimpseProfileDbTransaction(InnerCommand.Transaction, InspectorContext, InnerConnection);
            }
            set
            {
                var transaction = value as GlimpseProfileDbTransaction;
                InnerCommand.Transaction = (transaction != null) ? transaction.InnerTransaction : value;
            }
        }

        protected override DbParameter CreateDbParameter()
        {
            return InnerCommand.CreateParameter();
        }
        
        public DbCommand Inner
        {
            get { return InnerCommand; }
        } 

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            DbDataReader reader;
            var commandId = Guid.NewGuid();

            LogCommandStart(commandId); 
            var stopwatch = Stopwatch.StartNew();


                try
                {
                    reader = InnerCommand.ExecuteReader(behavior);
                }
                catch (Exception exception)
                {
                    LogCommandError(commandId, exception);
                    throw;
                }

            stopwatch.Stop(); 
            LogCommandEnd(commandId, stopwatch.ElapsedMilliseconds, reader.RecordsAffected);

            return new GlimpseProfileDbDataReader(reader, InnerCommand, InnerConnection.ConnectionId, commandId, InspectorContext); 
        }

        public override int ExecuteNonQuery()
        {
            int num;
            var commandId = Guid.NewGuid();

            LogCommandStart(commandId); 
            var stopwatch = Stopwatch.StartNew();
                try
                {
                    num = InnerCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    LogCommandError(commandId, exception);
                    throw;
                }
            stopwatch.Stop(); 
            LogCommandEnd(commandId, stopwatch.ElapsedMilliseconds, num);

            return num;
        }

        public override object ExecuteScalar()
        {
            object result;
            var commandId = Guid.NewGuid();

            LogCommandStart(commandId); 
            var stopwatch = Stopwatch.StartNew();
                try
                {
                    result = InnerCommand.ExecuteScalar();
                }
                catch (Exception exception)
                {
                    LogCommandError(commandId, exception);
                    throw;
                }
            stopwatch.Stop(); 
            LogCommandEnd(commandId, stopwatch.ElapsedMilliseconds, null);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && InnerCommand != null)
            {
                InnerCommand.Dispose();
            }
            InnerCommand = null;
            InnerConnection = null;
            base.Dispose(disposing);
        }


        #region Support Methods
        private static object GetParameterValue(IDataParameter parameter)
        {
            if (parameter.Value == DBNull.Value) 
                return "NULL"; 

            if (parameter.Value is byte[])
            {
                var builder = new StringBuilder("0x");
                foreach (byte num in (byte[])parameter.Value) 
                    builder.Append(num.ToString("X2")); 
                return builder.ToString();
            }
            return parameter.Value;
        }

        private void LogCommandStart(Guid commandId)
        {
            IList<Tuple<string, object, string, int>> parameters = null;
            if (Parameters.Count > 0)
            { 
                parameters = new List<Tuple<string, object, string, int>>();
                foreach (IDbDataParameter parameter in Parameters)
                {
                    var parameterName = parameter.ParameterName;
                    if (!parameterName.StartsWith("@"))
                        parameterName = "@" + parameterName; 
                    parameters.Add( new Tuple<string, object, string, int>(parameterName, GetParameterValue(parameter), parameter.DbType.ToString(), parameter.Size));
                }
            }

            InspectorContext.MessageBroker.Publish(new CommandExecutedMessage(InnerConnection.ConnectionId, commandId, InnerCommand.CommandText, parameters));
        }

        private void LogCommandEnd(Guid commandId, long elapsedMilliseconds, int? recordsAffected)
        { 
            InspectorContext.MessageBroker.Publish(new CommandDurationAndRowCountMessage(InnerConnection.ConnectionId, commandId, elapsedMilliseconds, recordsAffected));

        }

        private void LogCommandError(Guid commandId, Exception exception)
        {
            InspectorContext.MessageBroker.Publish(new CommandErrorMessage(InnerConnection.ConnectionId, commandId, exception));
        }
        #endregion
    }
}
