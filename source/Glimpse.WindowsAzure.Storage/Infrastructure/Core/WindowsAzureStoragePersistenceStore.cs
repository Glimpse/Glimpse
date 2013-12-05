using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Glimpse.Core.Framework;
using Glimpse.WindowsAzure.Storage.Infrastructure.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Glimpse.WindowsAzure.Storage.Infrastructure.Core
{
    public class WindowsAzureStoragePersistenceStore
        : IPersistenceStore
    {
        private readonly CloudBlobContainer blobContainer;
        private readonly CloudBlobClient blobClient;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        private const int BufferSize = 100;

        private GlimpseMetadata Metadata { get; set; }

        public WindowsAzureStoragePersistenceStore()
        {
            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new IPAddressConverter());
            jsonSerializerSettings.Converters.Add(new IPEndPointConverter());
            jsonSerializerSettings.Formatting = Formatting.None;
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            CloudStorageAccount account = null;
            if (CloudStorageAccount.TryParse(
                ConfigurationManager.AppSettings["Glimpse.WindowsAzure.Storage.ConnectionString"], out account))
            {
                blobClient = account.CreateCloudBlobClient();
                blobContainer = blobClient.GetContainerReference("glimpse-requests");

                blobContainer.CreateIfNotExists();
            }
        }

        public GlimpseRequest GetByRequestId(Guid requestId)
        {
            var listBlobItem = blobContainer.ListBlobs().FirstOrDefault(b => b.Uri.AbsoluteUri.EndsWith(string.Format("{0}-request.json", requestId)));
            if (listBlobItem != null)
            {
                var blob = blobClient.GetBlobReferenceFromServer(listBlobItem.Uri) as CloudBlockBlob;
                if (blob != null && blob.Exists())
                {
                    return JsonConvert.DeserializeObject<GlimpseRequest>(blob.DownloadText(), jsonSerializerSettings);
                }
            }
            return null;
        }

        public TabResult GetByRequestIdAndTabKey(Guid requestId, string tabKey)
        {
            if (string.IsNullOrEmpty(tabKey))
            {
                throw new ArgumentException("tabKey");
            }

            var glimpseRequest = GetByRequestId(requestId);
            if (glimpseRequest != null && glimpseRequest.TabData[tabKey] != null)
            {
                return glimpseRequest.TabData[tabKey];
            }
            return null;
        }

        public IEnumerable<GlimpseRequest> GetByRequestParentId(Guid parentRequestId)
        {
            return GetTop(BufferSize).Where(r => r.ParentRequestId == parentRequestId);
        }

        public IEnumerable<GlimpseRequest> GetTop(int count)
        {
            var blobs = blobContainer.ListBlobs().Take(count);
            foreach (var listBlobItem in blobs)
            {
                var blob = blobClient.GetBlobReferenceFromServer(listBlobItem.Uri) as CloudBlockBlob;
                if (blob != null && blob.Exists())
                {
                    yield return JsonConvert.DeserializeObject<GlimpseRequest>(blob.DownloadText(), jsonSerializerSettings);
                }
            }
        }

        public void Save(GlimpseRequest request)
        {
            var inverseTimeKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - request.DateTime.Ticks);

            var blob = blobContainer.GetBlockBlobReference(string.Format("{0}-{1}-request.json", inverseTimeKey, request.RequestId));
            blob.UploadText(JsonConvert.SerializeObject(request, jsonSerializerSettings));

            CleanOldRequests();
        }

        private void CleanOldRequests()
        {
            var blobs = blobContainer.ListBlobs().Skip(BufferSize);
            foreach (var listBlobItem in blobs)
            {
                var blob = blobClient.GetBlobReferenceFromServer(listBlobItem.Uri) as CloudBlockBlob;
                blob.DeleteIfExists();
            }
        }

        public GlimpseMetadata GetMetadata()
        {
            return Metadata;
        }

        public void Save(GlimpseMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}