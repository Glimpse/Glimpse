using Glimpse.Core.Extensibility;
using Glimpse.WebForms.Model;

namespace Glimpse.WebForms.SerializationConverter
{
    public class DataBindParameterConverter : SerializationConverter<DataBindParameter>
    {
        public override object Convert(DataBindParameter parameter)
        {
            return new
            {
                parameter.Type,
                parameter.Source,
                parameter.Field,
                parameter.Default,
                parameter.Value
            };
        }
    }
}
