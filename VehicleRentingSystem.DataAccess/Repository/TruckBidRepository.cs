using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository
{
    public class TruckBidRepository : Repository<TruckBid>, ITruckBidRepository
    {
        private ApplicationDbContext _db;
        public TruckBidRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecreamentCount(TruckBid bid, int Count)
        {
            bid.Bidding -= Count;
            return bid.Bidding;
        }

        public int IncreamentCount(TruckBid bid, int Count)
        {
            bid.Bidding += Count;
            return bid.Bidding;
        }

        public void Update(TruckBid obj)
        {
            _db.TruckBids.Update(obj);
        }
    }
    
}
