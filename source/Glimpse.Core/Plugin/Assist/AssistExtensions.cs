using System;

namespace Glimpse.Core.Plugin.Assist
{
	public static class AssistExtensions
	{
		public static TabSection ToTabSection(this object o)
		{
			if (o == null) throw new ArgumentNullException("o");

			var section = o as TabSection;
			if (section != null)
				return section;

			var instance = o as TabSection.Instance;
			if (instance != null)
				return instance.Data;

			var message = String.Format("The object is not a {0}. Object is of type {1}.", typeof(TabSection).Name, o.GetType());
			throw new InvalidOperationException(message);
		}
	}
}