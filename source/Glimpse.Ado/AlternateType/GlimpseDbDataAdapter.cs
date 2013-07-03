using System.Data;
using System.Data.Common;

namespace Glimpse.Ado.AlternateType
{
    public class GlimpseDbDataAdapter : DbDataAdapter
    {
        public GlimpseDbDataAdapter(DbDataAdapter innerDataAdapter)
        {
            InnerDataAdapter = innerDataAdapter;
        }

        private DbDataAdapter InnerDataAdapter { get; set; }

        public override bool ReturnProviderSpecificTypes
        {
            get { return InnerDataAdapter.ReturnProviderSpecificTypes; }
            set { InnerDataAdapter.ReturnProviderSpecificTypes = value; }
        }

        public override int UpdateBatchSize
        {
            get { return InnerDataAdapter.UpdateBatchSize; }
            set { InnerDataAdapter.UpdateBatchSize = value; }
        }

        protected override void Dispose(bool disposing)
        {
            InnerDataAdapter.Dispose();
        }

        public override int Fill(DataSet dataSet)
        {
            if (SelectCommand != null)
            {
                InnerDataAdapter.SelectCommand = ((GlimpseDbCommand)SelectCommand).Inner;
            } 

            return InnerDataAdapter.Fill(dataSet);
        }

        public override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            if (SelectCommand != null)
            {
                InnerDataAdapter.SelectCommand = ((GlimpseDbCommand)SelectCommand).Inner;
            } 

            return InnerDataAdapter.FillSchema(dataSet, schemaType);
        }

        public override IDataParameter[] GetFillParameters()
        {
            return InnerDataAdapter.GetFillParameters();
        }

        public override bool ShouldSerializeAcceptChangesDuringFill()
        {
            return InnerDataAdapter.ShouldSerializeAcceptChangesDuringFill();
        }

        public override bool ShouldSerializeFillLoadOption()
        {
            return InnerDataAdapter.ShouldSerializeFillLoadOption();
        }

        public override string ToString()
        {
            return InnerDataAdapter.ToString();
        }

        public override int Update(DataSet dataSet)
        {
            if (UpdateCommand != null)
            {
                InnerDataAdapter.UpdateCommand = ((GlimpseDbCommand)UpdateCommand).Inner;
            } 

            if (InsertCommand != null)
            {
                InnerDataAdapter.InsertCommand = ((GlimpseDbCommand)InsertCommand).Inner;
            } 

            if (DeleteCommand != null)
            {
                InnerDataAdapter.DeleteCommand = ((GlimpseDbCommand)DeleteCommand).Inner;
            } 

            return InnerDataAdapter.Update(dataSet);
        }
    }
}
