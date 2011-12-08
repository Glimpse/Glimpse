using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NerdDinner.Models
{
    [Bind(Include = "Title,Description,EventDate,Address,Country,ContactPhone,Latitude,Longitude")]
	public class Dinner
	{
	    public int DinnerID { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string HostedBy { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IList<RSVP> RSVPs { get; set; }

		public bool IsHostedBy(string userName)
		{
			return HostedBy.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
		}

		public bool IsUserRegistered(string userName)
		{
			return RSVPs.Any(r => r.AttendeeName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}

		public bool IsValid
		{
			get { return (GetRuleViolations().Count() == 0); }
		}

		public IEnumerable<RuleViolation> GetRuleViolations()
		{

			if (String.IsNullOrEmpty(Title))
				yield return new RuleViolation("Title is required", "Title");

			if (String.IsNullOrEmpty(Description))
				yield return new RuleViolation("Description is required", "Description");

			if (String.IsNullOrEmpty(HostedBy))
				yield return new RuleViolation("HostedBy is required", "HostedBy");

			if (String.IsNullOrEmpty(Address))
				yield return new RuleViolation("Address is required", "Address");

			if (String.IsNullOrEmpty(Country))
				yield return new RuleViolation("Country is required", "Country");

			if (String.IsNullOrEmpty(ContactPhone))
				yield return new RuleViolation("Phone# is required", "ContactPhone");

			if (Latitude == 0 || Longitude == 0)
				yield return new RuleViolation("Make sure to enter a valid address!", "Address");

			//TODO: For now, PhoneValidator is more trouble than it's worth. People 
			// get very frustrated when it doesn't work.
			//if (!PhoneValidator.IsValidNumber(ContactPhone, Country))
			//    yield return new RuleViolation("Phone# does not match country", "ContactPhone");

			yield break;
		}
	}
}