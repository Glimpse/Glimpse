using System;
using System.Collections.Generic;

namespace Glimpse.WebForms.Model
{
    /// <summary>
    /// A single element of information regarding ASP.NET data binding
    /// </summary>
    public class DataBindParameterModel
    {
        public TimeSpan Time { get; set; }
        public List<DataBindParameter> DataBindParameters { get; private set; }
        public DataBindParameterModel(TimeSpan time)
        {
            Time = time;
            DataBindParameters = new List<DataBindParameter>();
        }
    }
}
