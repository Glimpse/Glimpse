using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// An implementation of an <see cref="ICollection{T}"/> that can be used as base class for discoverable collections that 
    /// want to make sure that adding/removing/clearing items is under their control.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
    public abstract class DiscoverableCollection<TItem> : ICollection<TItem>
    {
        private CollectionConfiguration Configuration { get; set; }

        protected ILogger Logger { get; private set; }

        protected List<TItem> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoverableCollection{TItem}" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        protected DiscoverableCollection(CollectionConfiguration configuration, ILogger logger)
        {
            Guard.ArgumentNotNull("configuration", configuration);
            Guard.ArgumentNotNull("logger", logger);

            Configuration = configuration;
            Logger = logger;

            Items = new List<TItem>();

#warning maybe extract the discover part outside of the collection, there is no need for the collection to do that, it only needs to keep track of the elements and initialize it with their custom configuration
            if (configuration.AutoDiscover)
            {
                var resolvedInstances = new ReflectionBasedTypeDiscoverer(logger).ResolveAndCreateInstancesOfType<TItem>(
                    configuration.DiscoveryLocation, configuration.TypesToIgnore);

                foreach (var resolvedInstance in resolvedInstances)
                {
                    this.AddCore(resolvedInstance);
                }

                AssignInitialCustomConfigurations();
            }
        }

        private void AssignInitialCustomConfigurations()
        {
            // get the list of configurators
            var configurators = Items.OfType<IConfigurable>()
                .Select(configurable => configurable.Configurator)
                .GroupBy(configurator => configurator.CustomConfigurationKey);

            // have each configurator, configure its "configurable"
            foreach (var groupedConfigurators in configurators)
            {
                if (groupedConfigurators.Count() != 1)
                {
                    // there are multiple configurators using the same custom configuration key inside the same discoverable collection
                    // this means that any existing custom configuration content must be resolved by using the custom configuration key
                    // and the type for which the configurator is
                    foreach (var configurator in groupedConfigurators)
                    {
                        string customConfiguration = Configuration.GetCustomConfiguration(configurator.CustomConfigurationKey, configurator.GetType());
                        if (!string.IsNullOrEmpty(customConfiguration))
                        {
                            configurator.ProcessCustomConfiguration(customConfiguration);
                        }
                    }
                }
                else
                {
                    var configurator = groupedConfigurators.Single();
                    string customConfiguration = Configuration.GetCustomConfiguration(configurator.CustomConfigurationKey);
                    if (!string.IsNullOrEmpty(customConfiguration))
                    {
                        configurator.ProcessCustomConfiguration(customConfiguration);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Add(TItem item)
        {
            AddCore(item);
        }

        private void AddCore(TItem item)
        {
            // before adding the item, we'll initialize it with a custom configuration if requested
            Items.Add(item);
            Logger.Debug(Resources.DiscoverableCollectionAddItem, typeof(TItem).Name, item.GetType());
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> is the item was removed.</returns>
        public virtual bool Remove(TItem item)
        {
            var result = Items.Remove(item);

            if (result)
            {
                Logger.Debug(Resources.DiscoverableCollectionRemoveItem, typeof(TItem).Name, item.GetType());
            }

            return result;
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public virtual void Clear()
        {
            Items.Clear();
            Logger.Debug(Resources.DiscoverableCollectionClearItems, typeof(TItem).Name);
        }

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TItem item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Copies the collection into the given array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Starting index of the target array.</param>
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }
    }
}