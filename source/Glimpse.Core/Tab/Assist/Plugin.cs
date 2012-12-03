using System;

namespace Glimpse.Core.Tab.Assist
{
    public static class Plugin
    {
        public static TabSection Create(params string[] headers)
        {
            return new TabSection(headers);
        }

        public static TabSection Section(this TabSection current, string sectionName, Action<TabSection> section)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section", "Section must not be null!");
            }

            var tabSection = new TabSection();
            section.Invoke(tabSection);
            return current.Section(sectionName, tabSection);
        }

        public static TabSection Section(this TabSection current, string sectionName, TabSection section)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException("Section name must not be null or empty!", "sectionName");
            }

            if (section == null)
            {
                throw new ArgumentNullException("section", "Section must not be null!");
            }

            current.AddRow()
                .Column(sectionName).Strong()
                .Column(section);

            return current;
        }
    }
}