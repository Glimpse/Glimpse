using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class TabContextExtensions
    {
        public static IEnumerable<T> GetMessages<T>(this ITabContext context)
        {
            var tabStore = context.TabStore;

            if (!tabStore.Contains<IList<T>>())
            {
                return null;
            }

            return tabStore.Get<IList<T>>();
        }

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