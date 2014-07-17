using System;
using System.Web;

namespace Glimpse.AspNet.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GenerateGlimpseScriptTags(this HttpContextBase context)
        {
            var generateScripts = context.Items["__GlimpseClientScriptsStrategy"] as Func<Guid?, string>;

            if (generateScripts == null)
            {
                return string.Empty;
            }

            return generateScripts(null); // null means to use the current request id
        }
    }
}