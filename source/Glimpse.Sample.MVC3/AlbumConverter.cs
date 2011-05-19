using System;
using System.Collections.Generic;
using Glimpse.WebForms.Extensibility;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    [GlimpseConverter]
    public class AlbumConverter : IGlimpseConverter
    {
        public IDictionary<string, object> Serialize(object obj)
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

        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof (Album);
                yield break;
            }
        }
    }
}