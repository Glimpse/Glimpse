using System;
using Glimpse.Core.Extensibility;
// ReSharper disable RedundantUsingDirective
using Glimpse.Core.Extensions;
// ReSharper restore RedundantUsingDirective
using Glimpse.Core.Framework;

namespace Glimpse.Core.ResourceResult
{
    public class JsonResourceResult : IResourceResult
    {
        public JsonResourceResult(object data) : this(data, null)
        {
        }

        public JsonResourceResult(object data, string callback)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Data = data;
            Callback = callback;
            ContentType = string.IsNullOrEmpty(callback) ? @"application/json" : @"application/x-javascript";
        }

        public string Callback { get; set; }
        
        public string ContentType { get; set; }
        
        public object Data { get; set; }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;
            var serializer = context.Serializer;

            var result = serializer.Serialize(Data);

            frameworkProvider.SetHttpResponseHeader("Content-Type", ContentType);

            var toWrite = string.IsNullOrEmpty(Callback) ? result : string.Format("{0}({1});", Callback, result);

            frameworkProvider.WriteHttpResponse(toWrite);
        }
    }
}