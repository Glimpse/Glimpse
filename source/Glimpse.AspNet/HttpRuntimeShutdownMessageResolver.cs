using System;
using System.Reflection;
using System.Web;

namespace Glimpse.AspNet
{
    public static class HttpRuntimeShutdownMessageResolver
    {
        public static string ResolveShutdownMessage()
        {
            string shutDownMessage = "Reason for shutdown: ";
            try
            {
                var httpRuntimeType = typeof(HttpRuntime);

                // Get shutdown message from HttpRuntime via ScottGu: http://weblogs.asp.net/scottgu/archive/2005/12/14/433194.aspx
                var httpRuntime = httpRuntimeType.InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null) as HttpRuntime;
                if (httpRuntime != null)
                {
                    shutDownMessage += httpRuntimeType.InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, httpRuntime, null) as string;
                }
                else
                {
                    shutDownMessage += "unknown.";
                }
            }
            catch (Exception exception)
            {
                shutDownMessage += "Failed to determine shutdown message -> " + exception.Message;
            }

            return shutDownMessage.Replace(Environment.NewLine, ";");
        }
    }
}