using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using Dapper;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace MvcMusicStore.Framework
{
    public class QueryResource : IResource
    {
        private const string QueryKey = "query";

        public string Name
        {
            get { return "music_query"; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { new ResourceParameterMetadata(QueryKey) }; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            var queryValue = context.Parameters.GetValueOrDefault(QueryKey);

            var connectionString = ConfigurationManager.ConnectionStrings["MusicStoreEntities"];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString.ConnectionString;
                connection.Open();

                var data = connection.Query(queryValue).ToList();

                return new CacheControlDecorator(0, CacheSetting.NoCache, new JsonResourceResult(data, null));
            }
        }
    }
}