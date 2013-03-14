using System.Linq;

namespace Glimpse.AspNet.Net35.Backport
{
    public static class Net35Backport
    {
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value == null) return true;

            return value.All(char.IsWhiteSpace);
        }
    }
}