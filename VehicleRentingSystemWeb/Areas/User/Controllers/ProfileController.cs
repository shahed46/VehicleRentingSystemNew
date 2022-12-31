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
       
        private readonly IUnitOfWork _unitOfWork;
        public ProfileController(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index(string? userId)
        {
            if (userId == null)
            {


                //getting logged user id

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claimRole = claimsIdentity.FindFirst(ClaimTypes.Role);
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
               
                //getting logged user id end

                
              if(claimRole.Value=="Driver")
                {
                    
                    var post = _unitOfWork.Bid.GetAll(u => u.ApplicationUserId == claim.Value);
                    if (post.Count()!= null)
                    {
                        foreach(var item in post)
                        {
                            if(item.Confirmed==true)
                            {
                                var confirmedPost = _unitOfWork.Post_Car.GetFirstOrDefault(u => u.Id == item.PostId); 
                                confirmedPost.Confirm=true;
                            }
                        }
                    }
                    

                    var n = post.Count();
                    int?[] postId = new int?[n];
                    int i = 0;
                    foreach (var item in post)
                    {
                        postId[i] = item.PostId;
                        i++;
                    }
                    int?[] unique = postId.Distinct().ToArray();
                    ProfileVM profileVM = new ProfileVM();


                    
                    T[] InitializeArray<T>(int length) where T : new()
                    {
                        T[] array = new T[length];
                        for (int i = 0; i < length; ++i)
                        {
                            array[i] = new T();
                        }

                        return array;
                    }

                    Post_Car[] objPostList = InitializeArray<Post_Car>(unique.Length);
                    
                    for (int j = 0; j < unique.Length; j++)
                    {
                        objPostList[j] = _unitOfWork.Post_Car.GetFirstOrDefault(u => u.Id == unique[j]);

                    }

                    profileVM.post_Cars = objPostList;

                    profileVM.applicationUser = _unitOfWork.ApplicationUser.GetAll(u => u.Id == claim.Value);
                    return View(profileVM);
                }
                else 
                {
                    ProfileVM profileVM = new()
                    {
                        post_Cars = _unitOfWork.Post_Car.GetAll(u => u.ApplicationUserId == claim.Value),
                        applicationUser = _unitOfWork.ApplicationUser.GetAll(u => u.Id == claim.Value),
                        bidHistory = _unitOfWork.Bid.GetAll(u => u.Confirmed == true),

                    };
                    return View(profileVM);
                }

                

            }
            else
            {
                var post=_unitOfWork.Bid.GetAll(u=>u.ApplicationUserId==userId);
                var n=post.Count();
                int?[] postId = new int?[n];
                int i = 0;
                foreach(var item in post) 
                {
                    postId[i] = item.PostId;
                    i++;
                }
                int?[] unique = postId.Distinct().ToArray();
                ProfileVM profileVM = new ProfileVM();
               

                
                T[] InitializeArray<T>(int length) where T : new()
                {
                    T[] array = new T[length];
                    for (int i = 0; i < length; ++i)
                    {
                        array[i] = new T();
                    }

                    return array;
                }

                Post_Car[] objPostList = InitializeArray<Post_Car>(unique.Length);
                
                for (int j=0; j<unique.Length; j++)
                {
                    objPostList[j]= _unitOfWork.Post_Car.GetFirstOrDefault(u => u.Id == unique[j]);
                    
                }
              
                profileVM.post_Cars = objPostList;
                
                profileVM.applicationUser = _unitOfWork.ApplicationUser.GetAll(u => u.Id == userId);
                return View(profileVM);

            }
            
        }


    }
}

