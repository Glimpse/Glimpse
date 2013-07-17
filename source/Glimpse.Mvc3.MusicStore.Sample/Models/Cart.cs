using System.ComponentModel.DataAnnotations;

namespace MvcMusicStore.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual Album Album { get; set; }
    }

    public class CartItem
    { 
        public string AlbumTitle { get; set; }
        public decimal AlbumPrice { get; set; }
        public string GenreName { get; set; }
        public string ArtistName { get; set; }
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int RecordId { get; set; }
        public string CartId { get; set; }
    }
}