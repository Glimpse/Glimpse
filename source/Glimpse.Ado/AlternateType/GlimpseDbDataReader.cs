using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Glimpse.Ado.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Ado.AlternateType
{
    internal class GlimpseDbDataReader : DbDataReader
    {
        private IMessageBroker messageBroker;

        public GlimpseDbDataReader(DbDataReader dataReader, DbCommand command, Guid connectionId, Guid commandId)
        {
            InnerDataReader = dataReader;
            InnerCommand = command;        
            ConnectionId = connectionId;
            CommandId = commandId; 
        }

        public GlimpseDbDataReader(DbDataReader dataReader, DbCommand command, Guid connectionId, Guid commandId, IMessageBroker messageBroker) 
            : this(dataReader, command, connectionId, commandId)
        {
            MessageBroker = messageBroker;
        }
         
        private DbDataReader InnerDataReader { get; set; }

        private DbCommand InnerCommand { get; set; }

        private Guid ConnectionId { get; set; }

        private Guid CommandId { get; set; } 

        private bool Disposed { get; set; }

        private int RowCount { get; set; }

        private IMessageBroker MessageBroker
        {
            get { return messageBroker ?? (messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker()); }
            set { messageBroker = value; }
        }
         
        public override int Depth
        {
            get { return InnerDataReader.Depth; }
        }

        public override int FieldCount
        {
            get { return InnerDataReader.FieldCount; }
        }

        public override bool HasRows
        {
            get { return InnerDataReader.HasRows; }
        }

        public override bool IsClosed
        {
            get { return InnerDataReader.IsClosed; }
        }

        public override object this[int ordinal]
        {
            get { return InnerDataReader[ordinal]; }
        }

        public override object this[string name]
        {
            get { return InnerDataReader[name]; }
        }

        public override int RecordsAffected
        {
            get { return InnerDataReader.RecordsAffected; }
        }

        public override int VisibleFieldCount
        {
            get { return InnerDataReader.VisibleFieldCount; }
        }

        public override void Close()
        {
            if (MessageBroker != null)
            {
                MessageBroker.Publish(new CommandRowCountMessage(ConnectionId, CommandId, RowCount));
            }

            var inner = InnerDataReader as SqlDataReader;
            if (!Disposed && inner != null && InnerCommand.Transaction == null && inner.Read())
            {
                InnerCommand.Cancel();
            } 

            Disposed = true;
            InnerDataReader.Close();
        }

        protected override void Dispose(bool disposing)
        {
            Disposed = true;

            if (disposing)
            {
                InnerDataReader.Dispose();
            }

            base.Dispose(disposing);
        }

        public override bool GetBoolean(int ordinal)
        {
            return InnerDataReader.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return InnerDataReader.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return InnerDataReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override char GetChar(int ordinal)
        {
            return InnerDataReader.GetChar(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return InnerDataReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return InnerDataReader.GetDataTypeName(ordinal);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return InnerDataReader.GetDateTime(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return InnerDataReader.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return InnerDataReader.GetDouble(ordinal);
        }

        public override IEnumerator GetEnumerator()
        {
            return InnerDataReader.GetEnumerator();
        }

        public override Type GetFieldType(int ordinal)
        {
            return InnerDataReader.GetFieldType(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return InnerDataReader.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return InnerDataReader.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return InnerDataReader.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return InnerDataReader.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return InnerDataReader.GetInt64(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return InnerDataReader.GetName(ordinal);
        }

        public override int GetOrdinal(string name)
        {
            return InnerDataReader.GetOrdinal(name);
        }

        public override Type GetProviderSpecificFieldType(int ordinal)
        {
            return InnerDataReader.GetProviderSpecificFieldType(ordinal);
        }

        public override object GetProviderSpecificValue(int ordinal)
        {
            return InnerDataReader.GetProviderSpecificValue(ordinal);
        }

        public override int GetProviderSpecificValues(object[] values)
        {
            return InnerDataReader.GetProviderSpecificValues(values);
        }

        public override DataTable GetSchemaTable()
        {
            return InnerDataReader.GetSchemaTable();
        }

        public override string GetString(int ordinal)
        {
            return InnerDataReader.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return InnerDataReader.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return InnerDataReader.GetValues(values);
        }

        public override bool IsDBNull(int ordinal)
        {
            return InnerDataReader.IsDBNull(ordinal);
        }

        public override bool NextResult()
        {
            return InnerDataReader.NextResult();
        }

        public override bool Read()
        {
            var flag = InnerDataReader.Read();
            if (flag)
            {
                RowCount++;
            } 

            return flag;
        }
    }
}
