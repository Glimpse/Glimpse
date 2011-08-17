using System.Data;
using System.Data.Common;

namespace Glimpse.EF.Plumbing
{
    internal class GlimpseProfileDbDataAdapter : DbDataAdapter
    {
        private readonly DbDataAdapter _inner;

        public GlimpseProfileDbDataAdapter(DbDataAdapter inner)
        {
            _inner = inner;
        }


        public override bool ReturnProviderSpecificTypes
        {
            get { return _inner.ReturnProviderSpecificTypes; }
            set { _inner.ReturnProviderSpecificTypes = value; }
        }

        public override int UpdateBatchSize
        {
            get { return _inner.UpdateBatchSize; }
            set { _inner.UpdateBatchSize = value; }
        }


        protected override void Dispose(bool disposing)
        {
            _inner.Dispose();
        }

        public override int Fill(DataSet dataSet)
        {
            if (SelectCommand != null) 
                _inner.SelectCommand = ((GlimpseProfileDbCommand)SelectCommand).Inner; 
            return _inner.Fill(dataSet);
        }

        public override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            if (SelectCommand != null) 
                _inner.SelectCommand = ((GlimpseProfileDbCommand)SelectCommand).Inner; 
            return _inner.FillSchema(dataSet, schemaType);
        }

        public override IDataParameter[] GetFillParameters()
        {
            return _inner.GetFillParameters();
        }

        public override bool ShouldSerializeAcceptChangesDuringFill()
        {
            return _inner.ShouldSerializeAcceptChangesDuringFill();
        }

        public override bool ShouldSerializeFillLoadOption()
        {
            return _inner.ShouldSerializeFillLoadOption();
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public override int Update(DataSet dataSet)
        {
            if (UpdateCommand != null) 
                _inner.UpdateCommand = ((GlimpseProfileDbCommand)UpdateCommand).Inner; 
            if (InsertCommand != null) 
                _inner.InsertCommand = ((GlimpseProfileDbCommand)InsertCommand).Inner; 
            if (DeleteCommand != null) 
                _inner.DeleteCommand = ((GlimpseProfileDbCommand)DeleteCommand).Inner; 
            return _inner.Update(dataSet);
        }
    }
}
