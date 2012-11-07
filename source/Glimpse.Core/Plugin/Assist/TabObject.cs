using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
    public class TabObject : ITabBuild
    {
        private readonly IList<TabObjectRow> rows = new List<TabObjectRow>();

        public TabObjectRow AddRow()
        {
            var row = new TabObjectRow();
            rows.Add(row);
            return row;
        }

        public object Build()
        {
            var dictionary = new Dictionary<object, object>();
            foreach (var row in rows)
            {
                dictionary[row.BaseKey] = row.BaseValue;
            }

            return dictionary;
        } 
    }

    public class TabObjectRow : ITabObjectItem
    {
        internal object BaseKey { get; set; }

        internal object BaseValue { get; set; }

        public ITabObjectItem Key(object value)
        {
            BaseKey = value;
            return this;
        }

        public void Value(object value)
        {
            BaseValue = value;
        }
    }
}