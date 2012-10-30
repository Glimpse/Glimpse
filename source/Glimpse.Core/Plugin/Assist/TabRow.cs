using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public class TabRow
    {
        private readonly List<TabColumn> columns = new List<TabColumn>();

        public IEnumerable<TabColumn> Columns
        {
            get { return columns; }
        }

        public TabRow Column(object columnData)
        { 
            var column = new TabColumn(columnData);
            columns.Add(column);
            
            return this;
        }

        internal object[] Build()
        {
            return columns.Select(c => c.Data).ToArray();
        }
    }
}