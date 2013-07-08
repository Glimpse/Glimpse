using System;
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
                var typedCommand = SelectCommand as GlimpseDbCommand;
                if (typedCommand != null)
                {
                    InnerDataAdapter.SelectCommand = typedCommand.Inner;

                    var result = 0;
                    var commandId = Guid.NewGuid();

                    var timer = typedCommand.LogCommandSeed();
                    typedCommand.LogCommandStart(commandId, timer);
                    try
                    {
                        result = InnerDataAdapter.Fill(dataSet);
                    }
                    catch (Exception exception)
                    {
                        typedCommand.LogCommandError(commandId, timer, exception, "ExecuteDbDataReader");
                        throw;
                    }
                    finally
                    {
                        typedCommand.LogCommandEnd(commandId, timer, result, "ExecuteDbDataReader");
                    }

                    return result;
                }

                InnerDataAdapter.SelectCommand = SelectCommand;
            }

            return InnerDataAdapter.Fill(dataSet);
        }

        public override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            if (SelectCommand != null)
            {
                InnerDataAdapter.SelectCommand = RetrieveBaseType(SelectCommand);
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
                InnerDataAdapter.UpdateCommand = RetrieveBaseType(UpdateCommand);
            }

            if (InsertCommand != null)
            {
                InnerDataAdapter.InsertCommand = RetrieveBaseType(InsertCommand);
            }

            if (DeleteCommand != null)
            {
                InnerDataAdapter.DeleteCommand = RetrieveBaseType(DeleteCommand);
            }

            return InnerDataAdapter.Update(dataSet);
        }

        private DbCommand RetrieveBaseType(DbCommand command)
        {
            var typedCommand = command as GlimpseDbCommand;
            return typedCommand ?? command;
        }
    }
}
