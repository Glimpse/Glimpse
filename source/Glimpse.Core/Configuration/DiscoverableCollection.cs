using System;
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
        where TItem : class
    {
        /// <summary>
        /// Occurs when a change has been made to the collection. The change can be an addition or a removal
        /// </summary>
        public event EventHandler Changed = delegate { };

        private CollectionSettings Configuration { get; set; }

        protected ILogger Logger { get; private set; }

        protected List<TItem> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoverableCollection{TItem}" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="onChange">Event handler to call when the collection has changed.</param>
        protected DiscoverableCollection(CollectionSettings configuration, ILogger logger, EventHandler onChange = null)
        {
            Guard.ArgumentNotNull(configuration, "configuration");
            Guard.ArgumentNotNull(logger, "logger");

            Configuration = configuration;
            Logger = logger;

            Items = new List<TItem>();

            if (configuration.AutoDiscover)
            {
                var resolvedInstances = new ReflectionBasedTypeDiscoverer(logger).ResolveAndCreateInstancesOfType<TItem>(
                    configuration.DiscoveryLocation, configuration.TypesToIgnore);

                foreach (var resolvedInstance in resolvedInstances)
                {
                    this.AddCore(resolvedInstance);
                }

                AssignCustomConfigurations();
            }

            Changed += onChange;
        }

        private void AssignCustomConfigurations(string customConfigurationKeyFilter = null)
        {
            // get the list of configurators
            var configurators = Items.OfType<IConfigurable>()
                .Select(configurable => configurable.Configurator)
                .Where(configurator => customConfigurationKeyFilter == null || string.Equals(configurator.CustomConfigurationKey, customConfigurationKeyFilter, StringComparison.InvariantCulture))
                .GroupBy(configurator => configurator.CustomConfigurationKey);

            // have each configurator, configure its "configurable"
            foreach (var groupedConfigurators in configurators)
            {
                // a default custom configuration (one without explicitly having specified a configurator type) can be used but only when there is only
                // one configurator available for the given key. This is not allowed when there are multiple configurators using the same custom configuration 
                // key inside the same discoverable collection.
                var defaultCustomConfigurationAllowed = groupedConfigurators.Count() == 1;

                foreach (var configurator in groupedConfigurators)
                {
                    // it is important to get the custom configuration first, before checking whether it will be processed as it is always
                    // possible that getting the custom configuration might result in an exception due to a misconfiguration.
                    string customConfiguration = Configuration.GetCustomConfiguration(configurator.CustomConfigurationKey, configurator.GetType(), defaultCustomConfigurationAllowed);

                    if (!configurator.ProcessedCustomConfiguration && !string.IsNullOrEmpty(customConfiguration))
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
            Changed(this, EventArgs.Empty);
        }

        private void AddCore(TItem item)
        {
            Items.Add(item);
            var itemAsConfigurable = item as IConfigurable;
            if (itemAsConfigurable != null)
            {
                // once the item has been added, we'll assign the updated list of configurables with their corresponding custom configuration.
                // By specifying the custom configuration key of the freshly added item's configurator, we limit the group of affected configurators.
                AssignCustomConfigurations(itemAsConfigurable.Configurator.CustomConfigurationKey);
            }

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

            Changed(this, EventArgs.Empty);

            return result;
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public virtual void Clear()
        {
            Items.Clear();
            Logger.Debug(Resources.DiscoverableCollectionClearItems, typeof(TItem).Name);

            Changed(this, EventArgs.Empty);
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