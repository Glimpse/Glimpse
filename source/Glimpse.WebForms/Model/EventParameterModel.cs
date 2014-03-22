using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    public class EventParameterModel
    {
        public int Index { get; set; }

        public List<ModelBindParameter> Values { get; set; }

        public EventParameterModel(int index, List<ModelBindParameter> values)
        {
            Index = index;
            Values = values;
        }
    }
}
