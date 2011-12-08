using System;
using System.Linq;

namespace NerdDinner.Models {
	using System.Collections.Generic;

	public interface IDinnerRepository {

        IQueryable<Dinner> FindAllDinners();
		IQueryable<Dinner> FindByLocation(float latitude, float longitude);
        IQueryable<Dinner> FindUpcomingDinners();
        Dinner GetDinner(int id);

        void Add(Dinner dinner);
        void Delete(Dinner dinner);
        
        void Save();
    }
}
