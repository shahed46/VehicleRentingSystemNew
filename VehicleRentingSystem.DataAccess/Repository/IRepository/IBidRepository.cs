using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository.IRepository
{
    public interface IBidRepository:IRepository<Bid>
    {
        int IncreamentCount(Bid bid, int Count);
        int DecreamentCount(Bid bid, int Count);
        void Update(Bid obj);
    }
}
