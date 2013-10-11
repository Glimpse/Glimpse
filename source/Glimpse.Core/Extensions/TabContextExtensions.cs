using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    /// <summary>
    /// Extension methods to simplify common tasks completed with <see cref="ITabSetupContext"/> and <see cref="ITabContext"/> together.
    /// </summary>
    public static class TabContextExtensions
    {
        /// <summary>
        /// Gets all messages of type <c>T</c> that have been persisted with <c>PersistMessage&lt;T&gt;</c>.
        /// </summary>
        /// <typeparam name="T">The type of message to get.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of any persisted messages. If no messages has been persisted, an empty enumerable will be returned.
        /// </returns>
        public static IEnumerable<T> GetMessages<T>(this ITabContext context)
        {
            var tabStore = context.TabStore;

            if (!tabStore.Contains<IList<T>>())
            {
                var messages = new List<T>();
                tabStore.Set<IList<T>>(messages);
                return messages;
            }

            return tabStore.Get<IList<T>>();
        }

        /// <summary>
        /// Persists all messages of type <c>T</c> to a thread safe store. Persisted messages can be retrieved with <c>GetMessages&lt;T&gt;</c>.
        /// </summary>
        /// <typeparam name="T">The type of message to persist.</typeparam>
        /// <param name="context">The context.</param>
        public static void PersistMessages<T>(this ITabSetupContext context)
        {
            context.MessageBroker.Subscribe<T>(message => PersistMessage(message, context));
        }

        private static void PersistMessage<T>(T message, ITabSetupContext context)
        {
            var tabStore = context.GetTabStore();

            if (!tabStore.Contains<IList<T>>())
            {
                tabStore.Set<IList<T>>(new List<T>());
            }

            var messages = tabStore.Get<IList<T>>();

            messages.Add(message);
        }
    }
}