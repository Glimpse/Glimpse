namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents a type that can be configured by a <see cref="IConfigurator"/>
    /// </summary>
    public interface IConfigurableExtended
    {
#warning should replace the IConfigurable
        /// <summary>
        /// Gets the configurator
        /// </summary>
        IConfigurator Configurator { get; }
    }
}