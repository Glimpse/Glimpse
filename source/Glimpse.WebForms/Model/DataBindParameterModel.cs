using System;
using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    public class DataBindParameterModel
    {
        public TimeSpan Offset { get; set; }
        public List<DataBindParameter> DataBindParameters { get; private set; }
        public DataBindParameterModel(TimeSpan offset)
        {
            Offset = offset;
            DataBindParameters = new List<DataBindParameter>();
        }
    }
}
