using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository.IRepository
{
    public interface IComplainRepository:IRepository<Complain>
    {
        void Update(Complain obj);
    }
}
