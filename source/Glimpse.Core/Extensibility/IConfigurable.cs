using Glimpse.Core.Configuration;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IConfigurable</c> allows types to participate in their own configuration.
    /// </summary>
    /// <remarks>
    /// Several <see cref="IRuntimePolicy"/> implementations leverage <c>IConfigurable</c> to allow for configuration via <c>web.config</c>.
    /// </remarks>
    public interface IConfigurable
    {
        /// <summary>
        /// Provides implementations an instance of <see cref="Section"/> to self populate any end user configuration options.
        /// </summary>
        /// <param name="section">The configuration section, <c>&lt;glimpse&gt;</c> from <c>web.config</c>.</param>
        void Configure(Section section);
    }
}