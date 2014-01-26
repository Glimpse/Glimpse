using System;
using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    /// <summary>
    /// A single element of information regarding ASP.NET data binding
    /// </summary>
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
