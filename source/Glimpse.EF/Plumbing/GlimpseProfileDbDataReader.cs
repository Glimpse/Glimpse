using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbDataReader : DbDataReader
    {
        private readonly DbDataReader _inner;
        private readonly DbCommand _command;
        private readonly ProviderStats _stats;
        private readonly Guid _connectionId;
        private readonly Guid _commandId;
        private bool _disposed;
        private int _rowCount;


        public GlimpseProfileDbDataReader(DbDataReader inner, DbCommand command, Guid connectionId, Guid statementGuid, ProviderStats stats)
        {
            _inner = inner;
            _command = command;
            _connectionId = connectionId;
            _commandId = statementGuid;
            _stats = stats;
        }


        public override int Depth
        {
            get { return _inner.Depth; }
        }

        public override int FieldCount
        {
            get { return _inner.FieldCount; }
        }

        public override bool HasRows
        {
            get { return _inner.HasRows; }
        }

        public override bool IsClosed
        {
            get { return _inner.IsClosed; }
        }

        public override object this[int ordinal]
        {
            get { return _inner[ordinal]; }
        }

        public override object this[string name]
        {
            get { return _inner[name]; }
        }

        public override int RecordsAffected
        {
            get { return _inner.RecordsAffected; }
        }

        public override int VisibleFieldCount
        {
            get { return _inner.VisibleFieldCount; }
        }

        public override void Close()
        {
            _stats.CommandRowCount(_connectionId, _commandId, _rowCount);

            var inner = _inner as SqlDataReader;
            if (!_disposed && inner != null && _command.Transaction == null && inner.Read()) 
                _command.Cancel(); 

            _disposed = true;
            _inner.Close();
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;

            if (disposing) 
                _inner.Dispose();

            base.Dispose(disposing);
        }

        public override bool GetBoolean(int ordinal)
        {
            return _inner.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return _inner.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return _inner.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override char GetChar(int ordinal)
        {
            return _inner.GetChar(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return _inner.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _inner.GetDataTypeName(ordinal);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return _inner.GetDateTime(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return _inner.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return _inner.GetDouble(ordinal);
        }

        public override IEnumerator GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public override Type GetFieldType(int ordinal)
        {
            return _inner.GetFieldType(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return _inner.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return _inner.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return _inner.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return _inner.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return _inner.GetInt64(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return _inner.GetName(ordinal);
        }

        public override int GetOrdinal(string name)
        {
            return _inner.GetOrdinal(name);
        }

        public override Type GetProviderSpecificFieldType(int ordinal)
        {
            return _inner.GetProviderSpecificFieldType(ordinal);
        }

        public override object GetProviderSpecificValue(int ordinal)
        {
            return _inner.GetProviderSpecificValue(ordinal);
        }

        public override int GetProviderSpecificValues(object[] values)
        {
            return _inner.GetProviderSpecificValues(values);
        }

        public override DataTable GetSchemaTable()
        {
            return _inner.GetSchemaTable();
        }

        public override string GetString(int ordinal)
        {
            return _inner.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return _inner.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return _inner.GetValues(values);
        }

        public override bool IsDBNull(int ordinal)
        {
            return _inner.IsDBNull(ordinal);
        }

        public override bool NextResult()
        {
            return _inner.NextResult();
        }

        public override bool Read()
        {
            var flag = _inner.Read();
            if (flag) 
                _rowCount++; 
            return flag;
        }
    }
}
