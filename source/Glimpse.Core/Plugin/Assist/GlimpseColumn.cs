namespace Glimpse.Core.Plugin.Assist
{
	public class GlimpseColumn
	{
		public object Data { get; private set; }

		public GlimpseColumn(object o)
		{
			if (o is GlimpseSection)
				Data = o.AsGlimpseSection().Build();
			else
				Data = o;
		}

		internal void OverrideData(object data)
		{
			Data = data;
		}
	}
}