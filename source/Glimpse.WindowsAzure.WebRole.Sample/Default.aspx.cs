using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Glimpse.WindowsAzure.Caching.Infrastructure;
using Glimpse.WindowsAzure.Storage.Infrastructure;
using Microsoft.ApplicationServer.Caching;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace Glimpse.WindowsAzure.WebRole.Sample
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Perform operations on Windows Azure storage
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            var blobclient = account.CreateCloudBlobClient();

            var container1 = blobclient.GetContainerReference("glimpse1");
            container1.CreateIfNotExists(operationContext: OperationContextFactory.Current.Create());
            container1.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob }, operationContext: OperationContextFactory.Current.Create());
            try
            {
                container1.Create(operationContext: OperationContextFactory.Current.Create());
            }
            catch
            {
                // We wanted this to fail so we have some output in our Glimpse tab...
            }

            var container2 = blobclient.GetContainerReference("glimpse2");
            container2.CreateIfNotExists(operationContext: OperationContextFactory.Current.Create());
            //container2.Metadata.Add("foo", "bar");
            container2.SetMetadata(operationContext: OperationContextFactory.Current.Create());

            
            var tableclient = account.CreateCloudTableClient();
            var table1 = tableclient.GetTableReference("glimpse1");
            table1.CreateIfNotExists(operationContext: OperationContextFactory.Current.Create());
            table1.ExecuteQuery(query: new TableQuery(), operationContext: OperationContextFactory.Current.Create()).ToList();
            table1.ExecuteQuery(query: new TableQuery().Where("Name eq 'Glimpse'"), operationContext: OperationContextFactory.Current.Create()).ToList();

            // Perform operations on Windows Azure Cache
            var cacheFactory = new DataCacheFactory();
            var cache = cacheFactory.GetDefaultCache();

            // Wire in Glimpse
            DataCacheEventSubscriberFactory.Current.Subscribe(cache);

            // Add some items
            for (int i = 0; i < 50; i++)
            {
                cache.Put(i.ToString(), "value");
            }

            // Read some items
            for (int i = 0; i < 50; i++)
            {
                cache.Get(i.ToString());
            }
        }
    }
}