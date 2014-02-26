namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents a configurator
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// Gets the name of the configuration element which the configurator wants to process
        /// </summary>
        string CustomConfigurationKey { get; }

        /// <summary>
        /// Will be called when custom configuration is available for the given custom configuration key
        /// </summary>
        /// <param name="customConfiguration">The custom configuration</param>
        void ProcessCustomConfiguration(string customConfiguration);
    }
}