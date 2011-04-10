using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Glimpse.Protocol;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    [GlimpseConverter]
    public class ListAlbumConverter:IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as IList<Album>;
            if (source == null) return null;

            var result = new Dictionary<string, object>();

            var count = 0;
            foreach (var album in source)
            {
                result.Add(count++.ToString(), new Dictionary<string, string>
                       {
                           {"Artist", album.Artist.Name},
                           {"Genre", album.Genre.Name},
                           {"Price", album.Price.ToString("c")},
                           {"Title", album.Title},
                       });
            }

            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(List<Album>);
                yield break; 
            }
        }
    }
}