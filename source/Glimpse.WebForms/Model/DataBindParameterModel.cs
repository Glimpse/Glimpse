using System;

namespace Glimpse.WebForms.Model
{
    /// <summary>
    /// A single element of information regarding ASP.NET data binding
    /// </summary>
    public class DataBindParameterModel
    {
        public string name { get; set; }
        public Type type { get; set; }
        public object value { get; set; }
        public DataBindParameterModel(string name, Type type, object value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
}
