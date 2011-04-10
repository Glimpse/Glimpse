using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Protocol;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    [GlimpseConverter]
    public class CartConverter : IGlimpseConverter
    {
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var source = obj as Cart;
            if (source == null) return null;

            return new Dictionary<string, object>
                       {
                           {"Id", source.CartId},
                           {"Count", source.Count},
                           {"Created", source.DateCreated},
                           {"RecordId", source.RecordId},
                           {"Album", new Dictionary<string, object>
                                            {
                                                {"Artist", source.Album.Artist.Name},
                                                {"Genre", source.Album.Genre.Name},
                                                {"Price", source.Album.Price.ToString("c")},
                                                {"Title", source.Album.Title},
                                            }
                               },
                       };
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof (Cart);
                yield break;
            }
        }
    }
}