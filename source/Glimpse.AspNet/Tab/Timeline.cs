using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Timeline : AspNetTab
    {
        public override string Name
        {
            get { return "Timeline"; }
        }

        public override object GetData(ITabContext context)
        {
            return null;
        }
    }
}