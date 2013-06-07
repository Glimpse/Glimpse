using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Core.Display
{
    [Obsolete]
    public class TimingsDisplay : IDisplay, ITabSetup, IKey
    {
        private const string InternalName = "Timings";

        public string Name 
        {
            get { return InternalName; }
        }

        public string Key 
        {
            get { return InternalName; }
        }

        public object GetData(ITabContext context)
        {
            return context.GetMessages<ITimelineMessage>()
                .Where(m => 
                    m.EventCategory.Name.Equals("Command") ||
                    m.EventCategory.Name.Equals("Controller") ||
                    m.EventCategory.Name.Equals("Render"));
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITimelineMessage>();
        }
    }
}