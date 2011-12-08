namespace NerdDinner.Models
{
    public class RSVP
    {
        public int RsvpID { get; set; }
        public int DinnerID { get; set; }
        public string AttendeeName { get; set; }
        public Dinner Dinner { get; set; }
    }
}