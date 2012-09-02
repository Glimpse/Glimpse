using System;

namespace Glimpse.Core.Plugin.Assist
{
	public static class Plugin
	{
		public static GlimpseSection Create(params string[] headers)
		{
			return new GlimpseSection(headers);
		}

		public static GlimpseSection Section(this GlimpseSection current, string sectionName, Action<GlimpseSection> section)
		{
			if (section == null)
				throw new ArgumentNullException("section", "Section must not be null!");

			var glimpseSection = new GlimpseSection();
			section.Invoke(glimpseSection);
			return current.Section(sectionName, glimpseSection);
		}

		public static GlimpseSection Section(this GlimpseSection current, string sectionName, GlimpseSection section)
		{
			if (String.IsNullOrEmpty(sectionName))
				throw new ArgumentException("Section name must not be null or empty!", "sectionName");

			if (section == null)
				throw new ArgumentNullException("section", "Section must not be null!");

			current.AddRow()
				.Column(sectionName).Bold()
				.Column(section);

			return current;
		}
	}
}