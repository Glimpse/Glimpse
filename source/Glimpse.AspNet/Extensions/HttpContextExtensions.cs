using System;
using System.Web;

namespace Glimpse.AspNet.Extensions
{
    public static class HttpContextExtensions
    {
        private const string HeadersSentKey = "__GlimpseHttpHeadersSent";

        public static string GenerateGlimpseScriptTags(this HttpContextBase context)
        {
            var generateScripts = context.Items["__GlimpseClientScriptsStrategy"] as Func<Guid?, string>;

            if (generateScripts == null)
            {
                return string.Empty;
            }

            return generateScripts(null); // null means to use the current request id
        }

        internal static void HeadersSent(this HttpContextBase context, bool value)
        {
            context.Items[HeadersSentKey] = value;
        }

        internal static bool HeadersSent(this HttpContextBase context)
        {
            var result = context.Items[HeadersSentKey] as bool?;

            return result.HasValue ? result.Value : false;
        }
    }
}