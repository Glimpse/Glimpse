using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
		public class RSVPController : Controller
		{

        IDinnerRepository dinnerRepository;

        //
        // Dependency Injection enabled constructors

        public RSVPController()
			: this(new NHibernateDinnerRepository(MvcApplication.CurrentSession))
		{
        }

        public RSVPController(IDinnerRepository repository) {
            dinnerRepository = repository;
        }

        //
        // AJAX: /Dinners/Register/1

        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(int id) {

            Dinner dinner = dinnerRepository.GetDinner(id);

            if (!dinner.IsUserRegistered(User.Identity.Name)) {

                RSVP rsvp = new RSVP();
                rsvp.AttendeeName = User.Identity.Name;

                dinner.RSVPs.Add(rsvp);
                dinnerRepository.Save();
            }

            return Content("Thanks - we'll see you there!");
        }
    }
}
