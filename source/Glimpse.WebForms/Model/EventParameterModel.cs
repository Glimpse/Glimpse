using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    public class EventParameterModel
    {
        public int Key { get; set; }

        public List<DataBindParameter> Value{ get; set; }

        public EventParameterModel(int key, List<DataBindParameter> value)
        {
            Key = key;
            Value = value;
        }
    }
}
