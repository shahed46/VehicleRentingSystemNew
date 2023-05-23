using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICityRepository City { get; }
        IPost_CarRepository Post_Car { get; }
        IBidRepository Bid { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IPost_TruckRepository Post_Truck { get; }
		ITruckBidRepository TruckBid { get; }
        IComplainRepository Complain { get; }
        void Save();
    }
}
