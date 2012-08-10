using Glimpse.AspNet.Extensibility;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Timeline:AspNetTab
    {
        public override object GetData(ITabContext context)
        {
            return null;
        }

        public override string Name
        {
            get { return "Timeline"; }
        }
    }
}