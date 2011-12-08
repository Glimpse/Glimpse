namespace NerdDinner.Models
{
	using System;
    using System.Linq;
	using NHibernate;
	using NHibernate.Linq;

    public class NHibernateDinnerRepository : IDinnerRepository
	{
		private ISession session;

		public NHibernateDinnerRepository(ISession session)
		{
			this.session = session;
		}

		public IQueryable<Dinner> FindAllDinners()
		{
			return session.Query<Dinner>();
		}

		public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
		{
			// note that this isn't as nice as it can be, since linq for nhibernate 
			// doesn't support custom SQL functions right now
			
			var matching = session.CreateSQLQuery("select DinnerID from dbo.NearestDinners(:latitude, :longitude)")
				.SetParameter("longitude", longitude)
				.SetParameter("latitude", latitude)
				.List<int>();

			return from dinner in FindUpcomingDinners()
				   where matching.Any(x => x == dinner.DinnerID)
				   select dinner;

		}

		public IQueryable<Dinner> FindUpcomingDinners()
		{
			return from dinner in FindAllDinners()
				   //where dinner.EventDate >= DateTime.Now
				   orderby dinner.EventDate
				   select dinner;
		}

		public Dinner GetDinner(int id)
		{
			return session.Get<Dinner>(id);
		}

		public void Add(Dinner dinner)
		{
			EnsureTransaction();

            if (!dinner.IsValid)
                throw new ApplicationException("Rule violations prevent saving");

			session.Save(dinner);
		}

		public void Delete(Dinner dinner)
		{
			EnsureTransaction();

			session.Delete(dinner);
		}

		private void EnsureTransaction()
		{
			if (session.Transaction.IsActive)
				return;
			session.BeginTransaction();
		}

		public void Save()
		{
			if (session.Transaction.IsActive == false)
				return;

			session.Transaction.Commit();
		}
	}
}