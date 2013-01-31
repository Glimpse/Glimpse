using System;
using System.Web;

namespace Glimpse.AspNet.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GenerateGlimpseScriptTags(this HttpContextBase context)
        {
            var generateScripts = context.Items["__GlimpseClientScriptsStrategy"] as Func<string>;

            if (generateScripts == null)
            {
                return string.Empty;
            }

            return generateScripts();
        }
    }
}