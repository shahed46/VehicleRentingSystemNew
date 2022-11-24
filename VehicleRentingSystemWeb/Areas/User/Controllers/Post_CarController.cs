﻿using VehicleRentingSystem.DataAccess;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using VehicleRentingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;

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
            //IEnumerable<Post_Car> objPostCaryList = _unitOfWork.Post_Car.GetAll(includeProperties: "Bid");
            //return View(objPostCaryList);
            PostVM postVM = new()
            {
                objPostCart = _unitOfWork.Post_Car.GetAll(),
                
            };
            return View(postVM);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Index(PostVM obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Bid.Add(obj);
        //        _unitOfWork.Save();
                
        //    }
        //    return View(obj);
        //}     
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

            obj.ApplicationUserId = claim.Value;

          
                _unitOfWork.Post_Car.Add(obj);
                _unitOfWork.Save();
               

            return RedirectToAction(nameof(Index));

        }

        

        public IActionResult Auction(int postId)
        {
            Bid bid = new Bid()
            {
                PostId = postId,
            };




            ////var categoryFromDbFirst = _unitOfWork.Bid.GetFirstOrDefault(u => u.PostId == postId);
            //IEnumerable<Bid> bidsFromDb = _unitOfWork.Bid.GetAll(u => u.PostId == postId);
            ////int minBid = bidsFromDb.Min(u => u.Bidding);
            //if (bidsFromDb == null)
            //{
            //    Bid bid = new Bid()
            //    {
            //        PostId = postId,
            //    };
            //    return View(bid);
            //}
            //else
            //{
            //    int minBid = bidsFromDb.Min(u => u.Bidding);
            //    Bid bidObj = new Bid()
            //    {
            //        PostId = postId,
            //        Bidding = minBid,
            //    };




            //    return View(bidObj);
            //}
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
            var postOwner=_unitOfWork.Post_Car.GetFirstOrDefault(u => u.Id == bidItem.PostId);
            if (postOwner.ApplicationUserId == claim.Value)
            {
                bidItem.Confirmed = true;
                _unitOfWork.Bid.Update(bidItem);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("You dont have right to confirm");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}