using System;
using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    public class DataBindParameterModel
    {
        public TimeSpan Offset { get; set; }

        public List<ModelBindParameter> DataBindParameters { get; private set; }

        public DataBindParameterModel(TimeSpan offset)
        {
            Offset = offset;
            DataBindParameters = new List<ModelBindParameter>();
        }
    }
}
