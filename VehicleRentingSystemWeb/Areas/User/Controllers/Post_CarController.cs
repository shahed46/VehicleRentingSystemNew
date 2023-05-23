using VehicleRentingSystem.DataAccess;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using VehicleRentingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Linq.Expressions;

namespace VehicleRentingSystemWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    
    public class Post_CarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private object postCarFromDbFirst;
        public Bid Bid { get; set; }

        public Post_CarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public IActionResult Index()
        {
            IEnumerable<Post_Car> objPostList = _unitOfWork.Post_Car.GetAll();

            
            foreach(var item in objPostList)
            {
                //remaining time counting
                DateTime dateTime = DateTime.Now;

                TimeSpan remainingTime = dateTime.Subtract(item.TargetTime);


                if (remainingTime.Hours==0 && remainingTime.Minutes<0 || remainingTime.Hours==1 && remainingTime.Minutes<0)
                {
                    item.DewTime = Math.Abs(remainingTime.Minutes);
                }
                else
                {
                    item.DewTime = 0;
                    item.TimeOver = true;
                }

                //remaining time counting end


                var post = _unitOfWork.Bid.GetAll(u => u.PostId==item.Id);
                foreach(var item2 in post)
                {
                    if (item2.Confirmed == true)
                    {
                        
                        item.Confirm = true;
                    }
                }
            }

            PostVM postVM = new()
            {
                
                objPostCart = objPostList.OrderByDescending(u=>u.Id),

            };
            
           
            return View(postVM);
        }

        

         
        //GET
        public IActionResult Create()
        {


            return View();
        }

        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Create(Post_Car obj)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            //countDown timer
            DateTime dateTime= DateTime.Now;
            obj.PostTime = dateTime;
            obj.TargetTime= dateTime.AddMinutes(30);

            //countDown timer end

            obj.ApplicationUserId = claim.Value;

          
                _unitOfWork.Post_Car.Add(obj);
                _unitOfWork.Save();
               

            return RedirectToAction(nameof(Index));

        }

        

        public IActionResult Auction(int postId)
        {
            double? lowest;
            IEnumerable<Bid>? bidList = _unitOfWork.Bid.GetAll(u => u.PostId == postId);
            if (bidList.Count() != 0)
            {
                lowest = bidList.Min(u => u.Bidding);
            }
            else
            {
                lowest = 0;
            }

           

            Bid bid = new Bid()
            {
                PostId = postId,
               MinBid=lowest,
                

        };

  
            return View(bid);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Auction(Bid bid)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            bid.ApplicationUserId = claim.Value;

            
            _unitOfWork.Bid.Add(bid);
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Plus(Bid bidObj,  int postId)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            bidObj.ApplicationUserId = claim.Value;
            
            IEnumerable<Bid> bidsFromDb = _unitOfWork.Bid.GetAll(u => u.PostId == postId);
            int minBid = bidsFromDb.Min(u => u.Bidding);

            bidObj.Bidding=minBid+100;
            _unitOfWork.Bid.Add(bidObj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(Bid bidObj, int postId)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            bidObj.ApplicationUserId = claim.Value;
            

            IEnumerable<Bid> bidsFromDb = _unitOfWork.Bid.GetAll(u => u.PostId == postId);
            int minBid = bidsFromDb.Min(u => u.Bidding);

            bidObj.Bidding = minBid - 100;
            _unitOfWork.Bid.Add(bidObj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult BiddingHistory(int postId)
        {
            IEnumerable<Bid> biddings = _unitOfWork.Bid.GetAll(u => u.PostId == postId, includeProperties: "ApplicationUser");
          
           // return View(bidHistory);

            BiddingHistoryVM biddingHistory = new()
            {
                bidHistory = _unitOfWork.Bid.GetAll(u => u.PostId == postId, includeProperties: "ApplicationUser"),
                
            };
            foreach (var bookedChecking in biddings)
            {
                if (bookedChecking.Confirmed == true)
                {
                    biddingHistory.booked = true;
                }
                //else
                //{
                //    biddingHistory.booked = false;
                //}
               
            }

            return View(biddingHistory);
        }


       



        public IActionResult Confirm(int bidId)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end
            var bidItem = _unitOfWork.Bid.GetFirstOrDefault(u => u.Id == bidId);
            var postOwner = _unitOfWork.Post_Car.GetFirstOrDefault(u => u.Id == bidItem.PostId);
            if (postOwner.ApplicationUserId == claim.Value)
            {
                bidItem.Confirmed = true;
                _unitOfWork.Bid.Update(bidItem);
                _unitOfWork.Save();
                TempData["success"] = "Confirmed successfully";
            }
            else
            {
                //throw new Exception("You dont have right to confirm");
                TempData["error"] = "You are not allowed";
            }

            return RedirectToAction(nameof(BiddingHistory));

            
        }

        //Summary

        public IActionResult Summary(int postId)
        {
            Post_Car post=_unitOfWork.Post_Car.GetFirstOrDefault(u => u.Id == postId, includeProperties: "ApplicationUser");
            Bid bidding=_unitOfWork.Bid.GetFirstOrDefault(u=>u.PostId==postId && u.Confirmed==true, includeProperties: "ApplicationUser");
            SummaryVM summary = new()
            {
                post= post,
                bid= bidding,
            };

            return View(summary);
        }

    }
}
