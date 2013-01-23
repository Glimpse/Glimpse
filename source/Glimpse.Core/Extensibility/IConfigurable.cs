using Glimpse.Core.Configuration;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for an extension to <see cref="IRuntimePolicy"/> that allows the 
    /// policy to receive current GlimpseSection.
    /// </summary>
    /// <remarks>
    /// An example of its use it to pick up types that are to be used from the Glimpse 
    /// section in the web.config.
    /// </remarks>
    public interface IConfigurable
    {
        /// <summary>
        /// Allows a given policy using the specified configuration section.
        /// </summary>
        /// <param name="section">The Glimpse configuration section.</param>
        void Configure(GlimpseSection section);
    }
}