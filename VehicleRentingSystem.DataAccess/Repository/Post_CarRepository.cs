using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository
{
    public class Post_CarRepository : Repository<Post_Car>, IPost_CarRepository
    {
        private ApplicationDbContext _db;
        public Post_CarRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       

        public void Update(Post_Car obj)
        {
            _db.Post_Cars.Update(obj);
        }
    }
    
}
