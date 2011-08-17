using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbCommand : DbCommand
    {
        public GlimpseProfileDbCommand(DbCommand innerCommand, ProviderStats stats)
        {
            InnerCommand = innerCommand;
            Stats = stats;
        }

        public GlimpseProfileDbCommand(DbCommand innerCommand, ProviderStats stats, GlimpseProfileDbConnection connection):this(innerCommand, stats)
        {
            InnerConnection = connection; 
        }


        private DbCommand InnerCommand { get; set; }
        private GlimpseProfileDbConnection InnerConnection { get; set; }
        private ProviderStats Stats { get; set; }



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

        protected override DbConnection DbConnection
        {
            get { return InnerConnection; }
            set
            {
                InnerConnection = value as GlimpseProfileDbConnection;
                InnerCommand.Connection = (InnerConnection != null) ? InnerConnection.InnerConnection : null;
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                if (InnerCommand.Transaction == null)
                    return null;

                return new GlimpseProfileDbTransaction(InnerCommand.Transaction, Stats, InnerConnection);
            }
            set
            {
                var transaction = value as GlimpseProfileDbTransaction;
                InnerCommand.Transaction = (transaction != null) ? transaction.Inner : null;
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
            LogCommand(commandId);
            var stopwatch = Stopwatch.StartNew();
            try
            {
                reader = InnerCommand.ExecuteReader(behavior);
            }
            catch (Exception exception)
            {
                Stats.CommandError(InnerConnection.ConnectionId, commandId, exception);
                throw;
            }
            Stats.CommandDurationAndRowCount(InnerConnection.ConnectionId, commandId, stopwatch.ElapsedMilliseconds, reader.RecordsAffected);

            return new GlimpseProfileDbDataReader(reader, InnerCommand, InnerConnection.ConnectionId, commandId, Stats); 
        }

        public override int ExecuteNonQuery()
        {
            int num;
            var commandId = Guid.NewGuid();
            LogCommand(commandId);
            var stopwatch = Stopwatch.StartNew();
            try
            {
                num = InnerCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Stats.CommandError(InnerConnection.ConnectionId, commandId, exception);
                throw;
            }
            Stats.CommandDurationAndRowCount(InnerConnection.ConnectionId, commandId, stopwatch.ElapsedMilliseconds, num);

            return num;
        }

        public override object ExecuteScalar()
        {
            object result;
            var commandId = Guid.NewGuid();
            LogCommand(commandId);
            var stopwatch = Stopwatch.StartNew();
            try
            {
                result = InnerCommand.ExecuteScalar();
            }
            catch (Exception exception)
            {
                Stats.CommandError(InnerConnection.ConnectionId, commandId, exception);
                throw;
            }
            Stats.CommandDurationAndRowCount(InnerConnection.ConnectionId, commandId, stopwatch.ElapsedMilliseconds, null);

            return result;
        }





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

        private void LogCommand(Guid commandId)
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

            Stats.CommandExecuted(InnerConnection.ConnectionId, commandId, InnerCommand.CommandText, parameters);
        }
    }
}
