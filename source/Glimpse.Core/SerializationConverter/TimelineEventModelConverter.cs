using Glimpse.Core.Extensibility;
using Glimpse.Core.Model;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="TimelineEventModel"/> representation's into a format suitable for serialization.
    /// </summary>
    public class TimelineEventModelConverter : SerializationConverter<TimelineEventModel>
    {
        /// <summary>
        /// Converts the specified model.
        /// </summary>
        /// <param name="model">The object to convert.</param>
        /// <returns>An object suitable for serialization.</returns>
        public override object Convert(TimelineEventModel model)
        {
            return new { model.Title, model.Category, model.SubText, model.StartTime, model.Details, model.Duration, model.StartPoint };
        } 
    }
}
