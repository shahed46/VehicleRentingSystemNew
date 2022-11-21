using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        private ApplicationDbContext _db;
        public BidRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecreamentCount(Bid bid, int Count)
        {
            bid.Bidding -= Count;
            return bid.Bidding;
        }

        public int IncreamentCount(Bid bid, int Count)
        {
            bid.Bidding += Count;
            return bid.Bidding;
        }

        public void Update(Bid obj)
        {
            _db.Bids.Update(obj);
        }
    }
    
}
