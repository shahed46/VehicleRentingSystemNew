using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository.IRepository
{
    public interface IPost_CarRepository:IRepository<Post_Car>
    {
        void Update(Post_Car obj);
    }
}
