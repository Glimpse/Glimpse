namespace Glimpse.Core.Plugin.Assist
{
    public class TabColumn : ITabBuild
    {
        public TabColumn(object columnData)
        { 
            Data = columnData; 
        }

        public object Data { get; private set; }

        internal void OverrideData(object columnData)
        {
            Data = columnData;
        }

        public object Build()
        {
            var columnData = Data as ITabBuild;
            return columnData != null ? columnData.Build() : Data;
        }
    }
}