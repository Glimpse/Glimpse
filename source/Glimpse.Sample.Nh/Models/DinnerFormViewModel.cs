using System.Web.Mvc;
using NerdDinner.Helpers;

namespace NerdDinner.Models {
    public class DinnerFormViewModel {
        
        public Dinner Dinner { get; private set; }
        public SelectList Countries { get; private set; }

        public DinnerFormViewModel(Dinner dinner) {
            Dinner = dinner;
            Countries = new SelectList(PhoneValidator.Countries, Dinner.Country);
        }
    }
}
