using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Represents a type that can be configured by a <see cref="IConfigurator"/>
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// Gets the configurator
        /// </summary>
        IConfigurator Configurator { get; }
    }
}