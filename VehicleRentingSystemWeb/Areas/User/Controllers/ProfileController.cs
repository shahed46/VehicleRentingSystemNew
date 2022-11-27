using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;
using VehicleRentingSystem.Models.ViewModels;

namespace VehicleRentingSystemWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ProfileController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ProfileController(IUnitOfWork unitOfWork /*UserManager<ApplicationUser> userManager*/)
        {
            _unitOfWork = unitOfWork;
            //_userManager = userManager;
        }
        public IActionResult Index(string? userId)
        {
            if (userId == null)
            {


                //getting logged user id

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //var claim = claimsIdentity.FindFirst(ClaimTypes.Email);

                //getting logged user id end
                ProfileVM profileVM = new()
                {
                    post_Cars = _unitOfWork.Post_Car.GetAll(u => u.ApplicationUserId == claim.Value),
                    applicationUser = _unitOfWork.ApplicationUser.GetAll(u => u.Id == claim.Value),

                };
                return View(profileVM);

            }
            else
            {
                ProfileVM profileVM = new()
                {
                    post_Cars = _unitOfWork.Post_Car.GetAll(u => u.ApplicationUserId == userId),
                    applicationUser = _unitOfWork.ApplicationUser.GetAll(u => u.Id == userId),

                };
                return View(profileVM);

            }
            
        }


    }
}

