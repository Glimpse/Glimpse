//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Web;
//using Glimpse.Core.Extensibility;
//using Glimpse.Core.Extensions;
//using Glimpse.Core.Framework;
//using Glimpse.Core.ResourceResult;
//using MvcMusicStore.Models;

//namespace MvcMusicStore.Framework
//{
//    public class InventoryResource : IResource
//    {
//        private const string AlbumIdKey = "albumId";

//        public string Name
//        {
//            get { return "music_query"; }
//        }

//        public IEnumerable<ResourceParameterMetadata> Parameters
//        {
//            get { return new[] { new ResourceParameterMetadata(AlbumIdKey) }; }
//        }

//        public IResourceResult Execute(IResourceContext context)
//        {
//            var queryValue = int.Parse(context.Parameters.GetValueOrDefault(AlbumIdKey));

//            var data = InventoryManager.GetInventory(queryValue);

//            return new CacheControlDecorator(0, CacheSetting.NoCache, new JsonResourceResult(data, null)); ;
//        }
//    }
//}