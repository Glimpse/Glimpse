namespace Glimpse.Core.Plugin.Assist
{
    public class TabColumn
    {
        public TabColumn(object columnData)
        {
            var typeData = columnData as ITabBuild;
            Data = typeData ?? columnData; 
        }

        public object Data { get; private set; }

        internal void OverrideData(object columnData)
        {
            Data = columnData;
        }
    }
}