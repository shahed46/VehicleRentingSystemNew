using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.DataAccess.Repository.IRepository;

namespace VehicleRentingSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            City = new CityRepository(_db);
            Post_Car=new Post_CarRepository(_db);
            Bid = new BidRepository(_db);
            ApplicationUser= new ApplicationUserRepository(_db);
            Post_Truck = new Post_TruckRepository(_db);
			TruckBid = new TruckBidRepository(_db);
            Complain=new ComplainRepository(_db);


		}

        public ICityRepository City { get;private set; }

        public IPost_CarRepository Post_Car { get; private set; }

        public IBidRepository Bid { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IPost_TruckRepository Post_Truck { get; private set; }
		public ITruckBidRepository TruckBid { get; private set; }
        public IComplainRepository Complain { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
