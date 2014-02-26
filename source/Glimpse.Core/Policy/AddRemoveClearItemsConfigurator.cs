using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Base class for <see cref="IConfigurator"/> types that process custom configurations based on add/remove/clear nodes
    /// </summary>
    /// <typeparam name="TConfiguredItem">Type of the configured item</typeparam>
    public abstract class AddRemoveClearItemsConfigurator<TConfiguredItem> : IConfigurator
    {
        private static readonly object configuredItemsLock = new object();
        private readonly List<TConfiguredItem> configuredItems = new List<TConfiguredItem>();
        private readonly IComparer<TConfiguredItem> Comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRemoveClearItemsConfigurator{TItem}" />
        /// </summary>
        /// <param name="customConfigurationKey">The custom configuration key</param>
        /// <param name="comparer">The comparer to use to check whether an item has already been added to the list of configured items</param>
        protected AddRemoveClearItemsConfigurator(string customConfigurationKey, IComparer<TConfiguredItem> comparer)
        {
            CustomConfigurationKey = customConfigurationKey;
            Comparer = comparer;
        }

        /// <summary>
        /// Gets the name of the configuration element which the configurator wants to process
        /// </summary>
        public string CustomConfigurationKey { get; private set; }

        /// <summary>
        /// Will be called when custom configuration is available for the given custom configuration key
        /// </summary>
        /// <param name="customConfiguration">The custom configuration</param>
        public void ProcessCustomConfiguration(string customConfiguration)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(customConfiguration);

                if (doc.DocumentElement != null)
                {
                    foreach (XmlNode element in doc.DocumentElement.ChildNodes)
                    {
                        string elementName = element.Name.ToLower();
                        switch (elementName)
                        {
                            case "add":
                                AddItem(CreateItem(element));
                                break;
                            case "clear":
                                ClearItems();
                                break;
                            case "remove":
                                RemoveItem(CreateItem(element));
                                break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                GlimpseConfiguration.GetLogger().Error("Failed to process custom configuration by '" + this.GetType().FullName + "'", exception);
            }
        }

        /// <summary>
        /// Gets the configured items
        /// </summary>
        protected IEnumerable<TConfiguredItem> ConfiguredItems
        {
            get { return configuredItems; }
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TConfiguredItem"/> based on the given <paramref name="itemNode"/>
        /// </summary>
        /// <param name="itemNode">The <see cref="XmlNode"/> from which an instance of <typeparamref name="TConfiguredItem"/> must be created</param>
        /// <returns>An instance of <typeparamref name="TConfiguredItem"/></returns>
        protected abstract TConfiguredItem CreateItem(XmlNode itemNode);

        /// <summary>
        /// Adds the given item to the collection if it does not yet exist
        /// </summary>
        /// <param name="item">The item to add</param>
        protected void AddItem(TConfiguredItem item)
        {
            lock (configuredItemsLock)
            {
                if (ConfiguredItems.All(existingItem => Comparer.Compare(existingItem, item) != 0))
                {
                    configuredItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes the given configured item
        /// </summary>
        /// <param name="item">The item to remove</param>
        protected void RemoveItem(TConfiguredItem item)
        {
            lock (configuredItemsLock)
            {
                var itemToRemove = ConfiguredItems.SingleOrDefault(existingItem => Comparer.Compare(existingItem, item) == 0);
                if (!object.Equals(itemToRemove, default(TConfiguredItem)))
                {
                    configuredItems.Remove(itemToRemove);
                }
            }
        }

        /// <summary>
        /// Removes all the configured items
        /// </summary>
        protected void ClearItems()
        {
            lock (configuredItemsLock)
            {
                configuredItems.Clear();
            }
        }
    }
}