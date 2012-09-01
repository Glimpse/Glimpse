namespace Glimpse.Core.Plugin.Assist
{
	public class GlimpseColumn
	{
		public object Data { get; private set; }

		public GlimpseColumn(object columnData)
		{
			Data = columnData is GlimpseSection
				? columnData.ToGlimpseSection().Build()
				: columnData;
		}

		internal void OverrideData(object columnData)
		{
			Data = columnData;
		}
	}
}