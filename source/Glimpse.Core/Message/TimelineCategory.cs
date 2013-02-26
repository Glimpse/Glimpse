namespace Glimpse.Core.Message
{
    /// <summary>
    /// Definition for a timeline category.
    /// </summary>
    public class TimelineCategory
    {
        public TimelineCategory(string name, string color, string colorHighlight)
        {
            Name = name;
            Color = color;
            ColorHighlight = colorHighlight;
        }

        /// <summary>
        /// Gets or sets the name that will be displayed in the UI.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the main bar color.
        /// </summary>
        /// <value>The color.</value>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the main bar border color.
        /// </summary>
        /// <value>The color highlight.</value>
        public string ColorHighlight { get; set; }
    }
}
