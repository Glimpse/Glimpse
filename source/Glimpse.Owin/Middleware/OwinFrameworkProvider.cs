using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Middleware
{
    public class OwinFrameworkProvider : IFrameworkProvider
    {
        private IDictionary<string, object> environment;
        private OwinRequest request;
        private OwinResponse response;
        private IDictionary<string, object> serverStore;

        public OwinFrameworkProvider(IDictionary<string, object> environment, IDictionary<string, object> serverStore)
        {
            this.environment = environment;
            this.request = new OwinRequest(environment); // Merge RequestMetadata and FrameworkProvider together?
            this.response = new OwinResponse(environment);
            this.serverStore = serverStore;
        }

        public IDataStore HttpRequestStore 
        {
            get
            {
                const string key = "glimpse.requestStore";

                if (environment.ContainsKey(key))
                {
                    return (IDataStore)environment[key];
                }

                var result = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
                environment.Add(key, result);
                return result;
            }
        }

        public IDataStore HttpServerStore 
        {
            get
            {
                return new DictionaryDataStoreAdapter((Dictionary<string, object>)serverStore);
            }
        }

        public object RuntimeContext 
        {
            get { return environment; }
        }

        public IRequestMetadata RequestMetadata 
        {
            get { return new RequestMetadata(request, response); }
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            response.Headers[name] = value;
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            response.StatusCode = statusCode;
        }

        public void SetCookie(string name, string value)
        {
            response.Cookies.Append(name, value);
        }

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            // Hack: doing nothing because this has been temporarily moved to HeadMiddlewear
        }

        public void WriteHttpResponse(byte[] content)
        {
            response.Write(content);
        }

        public void WriteHttpResponse(string content)
        {
            response.Write(content);
        }
    }

    public class PreBodyTagFilter : Stream
    {
        private const string BodyClosingTag = "</body>";

        private const string TroubleshootingDocsUri = "http://getglimpse.com/Help/Troubleshooting";

        public PreBodyTagFilter(string htmlSnippet, Stream outputStream, Encoding contentEncoding, ILogger logger)
        {
            HtmlSnippet = htmlSnippet + BodyClosingTag;
            OutputStream = outputStream;
            ContentEncoding = contentEncoding;
            BodyEnd = new Regex(BodyClosingTag, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            Logger = logger;
        }

        public override bool CanRead
        {
            get { return OutputStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return OutputStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return OutputStream.CanWrite; }
        }

        public override long Length
        {
            get { return OutputStream.Length; }
        }

        public override long Position
        {
            get { return OutputStream.Position; }
            set { OutputStream.Position = value; }
        }

        private ILogger Logger { get; set; }

        private string HtmlSnippet { get; set; }

        private Stream OutputStream { get; set; }

        private Encoding ContentEncoding { get; set; }

        private Regex BodyEnd { get; set; }

        public override void Close()
        {
            OutputStream.Close();
        }

        public override void Flush()
        {
            OutputStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return OutputStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            OutputStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return OutputStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string contentInBuffer = ContentEncoding.GetString(buffer);

            if (BodyEnd.IsMatch(contentInBuffer))
            {
                string bodyCloseWithScript = BodyEnd.Replace(contentInBuffer, HtmlSnippet, 1);

                byte[] outputBuffer = ContentEncoding.GetBytes(bodyCloseWithScript);

                OutputStream.Write(outputBuffer, 0, outputBuffer.Length);
            }
            else
            {
                Logger.Warn("Unable to locate '</body>' with content encoding '{0}'. The response may be compressed. See {1} for information on troubleshooting this issue.", ContentEncoding.EncodingName, TroubleshootingDocsUri);
                OutputStream.Write(buffer, offset, count);
            }
        }
    }

}