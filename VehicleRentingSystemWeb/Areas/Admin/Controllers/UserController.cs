using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using VehicleRentingSystem.DataAccess;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystemWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        
       
       

        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
           _dbContext= dbContext;
        }
        public IActionResult Index()
        {
            

            IEnumerable<ApplicationUser> userList = _unitOfWork.ApplicationUser.GetAll();
            

            return View(userList);
        }

        //get

        public IActionResult Complain()
        {
            IEnumerable<Complain> complainList=_unitOfWork.Complain.GetAll(includeProperties: "ApplicationUser");
           
            return View(complainList);
        }

        //POST
        [HttpDelete]

        public IActionResult Delete(string? driverId)
        {
            var obj = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == driverId);
            

            

            _unitOfWork.ApplicationUser.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));



        }
    }
}
