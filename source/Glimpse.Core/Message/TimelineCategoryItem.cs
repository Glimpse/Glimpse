namespace Glimpse.Core.Message
{
    /// <summary>
    /// Definition for a timeline category.
    /// </summary>
    public class TimelineCategoryItem
    {
        /// <summary>
        /// Constructs the timeline category item.
        /// </summary>
        /// <param name="name">Name that will be displayed in the UI.</param>
        /// <param name="color">The main bar color.</param>
        /// <param name="colorHighlight">The main bar border color.</param>
        public TimelineCategoryItem(string name, string color, string colorHighlight)
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
