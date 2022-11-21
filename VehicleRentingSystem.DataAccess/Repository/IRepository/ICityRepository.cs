using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository.IRepository
{
    public interface ICityRepository:IRepository<City>
    {
        void Update(City obj);
    }
}
