using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository.IRepository
{
    public interface ITruckBidRepository:IRepository<TruckBid>
    {
        int IncreamentCount(TruckBid bid, int Count);
        int DecreamentCount(TruckBid bid, int Count);
        void Update(TruckBid obj);
    }
}
