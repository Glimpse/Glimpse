namespace Glimpse.Core.Plugin.Assist
{
	public class TabColumn
	{
		public object Data { get; private set; }

		public TabColumn(object columnData)
		{
			Data = columnData is TabSection
				? columnData.ToTabSection().Build()
				: columnData;
		}

		internal void OverrideData(object columnData)
		{
			Data = columnData;
		}
	}
}