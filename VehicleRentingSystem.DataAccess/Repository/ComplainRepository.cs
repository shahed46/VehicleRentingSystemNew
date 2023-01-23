using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess.Repository
{
    public class ComplainRepository : Repository<Complain>, IComplainRepository
    {
        private ApplicationDbContext _db;
        public ComplainRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       

        public void Update(Complain obj)
        {
            _db.Complain.Update(obj);
        }
    }
    
}
