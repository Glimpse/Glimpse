using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    public class EventParameterModel
    {
        public int Key { get; set; }

        public List<ModelBindParameter> Value{ get; set; }

        public EventParameterModel(int key, List<ModelBindParameter> value)
        {
            Key = key;
            Value = value;
        }
    }
}
