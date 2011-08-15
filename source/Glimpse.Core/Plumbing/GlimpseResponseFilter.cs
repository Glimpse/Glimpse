using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Plumbing
{
    //Heavily influenced by http://www.4guysfromrolla.com/articles/120308-1.aspx
    public class GlimpseResponseFilter : MemoryStream
    {
        internal StringBuilder ResponseContent { get; set; }
        internal Stream OutputStream { get; set; }
        internal HttpContextBase Context { get; set; }

        private Regex htmlEnd = new Regex("</html>", RegexOptions.Compiled | RegexOptions.Multiline);
        private Regex bodyEnd = new Regex("</body>", RegexOptions.Compiled | RegexOptions.Multiline);

        public GlimpseResponseFilter(Stream output, HttpContextBase context)
        {
            ResponseContent = new StringBuilder();
            OutputStream = output;
            Context = context;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Convert the content in buffer to a string
            string contentInBuffer = UTF8Encoding.UTF8.GetString(buffer);

            // Buffer content in responseContent until we reach the end of the page's markup
            ResponseContent.Append(contentInBuffer);

            if (htmlEnd.IsMatch(ResponseContent.ToString()))
            {

                if (Context.GetGlimpseMode() == GlimpseMode.On)
                {
                    //var path = Context.GlimpseResourcePath("");
                    //var html = string.Format(@"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{1}'>var glimpse = {0}, glimpsePath = '{2}';</script>", json, requestId, path);

                    var dataPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId());
                    var clientPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("client.js"));

                    var html = @"<script type='text/javascript' id='glimpseData' src='" + dataPath + "'></script><script type='text/javascript' id='glimpseClient' src='" + clientPath + "'></script></body>";
                
                    // Add glimpse output notice
                    string contentWithCopyright = bodyEnd.Replace(ResponseContent.ToString(),html);

                    // Write content to the outputStream
                    byte[] outputBuffer = UTF8Encoding.UTF8.GetBytes(contentWithCopyright);

                    OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
                }
            }
        }
    }
}