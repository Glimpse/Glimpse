using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Glimpse.Protocol;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    [GlimpseConverter]
    public class AlbumConverter : IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as Album;
            if (source == null) return null;

            return new Dictionary<string, object>
                       {
                           {"Artist", source.Artist != null ? source.Artist.Name : null},
                           {"Genre", source.Genre != null ? source.Genre.Name : null},
                           {"Price", source.Price.ToString("c")},
                           {"Title", source.Title},
                       };
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof (Album);
                yield break;
            }
        }
    }
}