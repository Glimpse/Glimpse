namespace Glimpse.Core.Plugin.Assist
{
    public class TabColumn
    {
        public TabColumn(object columnData)
        {
            Data = columnData is TabSection
                ? columnData.ToTabSection().Build()
                : columnData;
        }

        public object Data { get; private set; }

        internal void OverrideData(object columnData)
        {
            Data = columnData;
        }
    }
}