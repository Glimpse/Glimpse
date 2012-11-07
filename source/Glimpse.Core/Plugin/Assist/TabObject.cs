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
}