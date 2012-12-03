using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public class TabLayoutRow : ITabBuild
    {
        private readonly List<TabLayoutCell> cells = new List<TabLayoutCell>();

        public TabLayoutCell Cell(int cell)
        {
            var layoutCell = new TabLayoutCell(cell);
            cells.Add(layoutCell);
            return layoutCell;
        }

        public object Build()
        {
            return cells.Select(x => x.Build());
        }
    }
}