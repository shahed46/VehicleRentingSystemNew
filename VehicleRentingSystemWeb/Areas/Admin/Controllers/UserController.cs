using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystemWeb.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
       
       

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> userList= _unitOfWork.ApplicationUser.GetAll();
            

            return View();
        }
    }
}
