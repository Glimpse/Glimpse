namespace Glimpse.Core.Plugin.Assist
{
    public class TabSectionColumn : ITabBuild
    {
        public TabSectionColumn(object columnData)
        { 
            Data = columnData; 
        }

        public object Data { get; private set; }

        public object Build()
        {
            var columnData = Data as ITabBuild;
            return columnData != null ? columnData.Build() : Data;
        }

        internal void OverrideData(object columnData)
        {
            Data = columnData;
        }
    }
}