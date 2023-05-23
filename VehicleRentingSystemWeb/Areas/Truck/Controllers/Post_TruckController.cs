using VehicleRentingSystem.DataAccess;
using VehicleRentingSystem.DataAccess.Repository.IRepository;
using VehicleRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using VehicleRentingSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;

namespace VehicleRentingSystemWeb.Areas.Truck.Controllers
{
    [Area("Truck")]
    [Authorize]
    public class Post_TruckController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private object postTruckFromDbFirst;
        public Bid Bid { get; set; }

        public Post_TruckController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {


            IEnumerable<Post_Truck> objPostList = _unitOfWork.Post_Truck.GetAll();


            foreach (var item in objPostList)
            {
                //remaining time counting
                DateTime dateTime = DateTime.Now;

                TimeSpan remainingTime = dateTime.Subtract(item.TargetTime);


                if (remainingTime.Hours == 0 && remainingTime.Minutes < 0 || remainingTime.Hours == 1 && remainingTime.Minutes < 0)
                {
                    item.DewTime = Math.Abs(remainingTime.Minutes);
                }
                else
                {
                    item.DewTime = 0;
                    item.TimeOver = true;
                }

                //remaining time counting end


                var post = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == item.Id);
                foreach (var item2 in post)
                {
                    if (item2.Confirmed == true)
                    {

                        item.Confirm = true;
                    }
                }
            }

            PostVM postVM = new()
            {
                objPostTruck =  objPostList.OrderByDescending(u => u.Id),

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
        public IActionResult Create(Post_Truck obj, IFormFile file)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            //countDown timer
            DateTime dateTime = DateTime.Now;
            obj.PostTime = dateTime;
            obj.TargetTime = dateTime.AddMinutes(30);

            //countDown timer end

            string wwwRootPath = _hostEnvironment.WebRootPath;
			if (file != null)
			{
				string fileName = Guid.NewGuid().ToString();
				var uploads = Path.Combine(wwwRootPath, @"images/products");
				var extension = Path.GetExtension(file.FileName);
				//copying file in the product folder inside wwwroot
				using (var filestreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
				{
					file.CopyTo(filestreams);
				}
				//copying file in the product folder end
				obj.ImageUrl = @"\images\products\" + fileName + extension;
			}

			obj.ApplicationUserId = claim.Value;

          
                _unitOfWork.Post_Truck.Add(obj);
                _unitOfWork.Save();
               

            return RedirectToAction(nameof(Index));

        }

        

        public IActionResult Auction(int postId)
        {
            double? lowest;
            IEnumerable<TruckBid>? bidList = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == postId);
            if (bidList.Count() != 0)
            {
                lowest = bidList.Min(u => u.Bidding);
            }
            else
            {
                lowest = 0;
            }


            TruckBid truckbid = new TruckBid()
            {
                TruckPostId = postId,
                MinBid=lowest,
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
            return View(truckbid);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Auction(TruckBid bid)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            bid.ApplicationUserId = claim.Value;

            
            _unitOfWork.TruckBid.Add(bid);
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Plus(TruckBid bidObj,  int truckpostId)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            bidObj.ApplicationUserId = claim.Value;
            
            IEnumerable<TruckBid> bidsFromDb = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == truckpostId);
            int minBid = bidsFromDb.Min(u => u.Bidding);

            bidObj.Bidding=minBid+100;
            bidObj.TruckPostId = truckpostId;
            _unitOfWork.TruckBid.Add(bidObj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(TruckBid bidObj, int truckpostId)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end

            bidObj.ApplicationUserId = claim.Value;
            

            IEnumerable<TruckBid> bidsFromDb = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == truckpostId);
            int minBid = bidsFromDb.Min(u => u.Bidding);

            bidObj.Bidding = minBid - 100;
            bidObj.TruckPostId = truckpostId;
            _unitOfWork.TruckBid.Add(bidObj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult BiddingHistory(int truckpostId)
        {
            //IEnumerable<TruckBid> truckbidHistory = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == truckpostId, includeProperties: "ApplicationUser");


            //return View(truckbidHistory);

            IEnumerable<TruckBid> biddings = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == truckpostId, includeProperties: "ApplicationUser");

            // return View(bidHistory);

            TruckBiddingHistoryVM biddingHistory = new()
            {
                bidHistory = _unitOfWork.TruckBid.GetAll(u => u.TruckPostId == truckpostId, includeProperties: "ApplicationUser"),

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
            var bidItem = _unitOfWork.TruckBid.GetFirstOrDefault(u => u.Id == bidId);
            var postOwner=_unitOfWork.Post_Truck.GetFirstOrDefault(u => u.Id == bidItem.TruckPostId);
            if (postOwner.ApplicationUserId == claim.Value)
            {
                bidItem.Confirmed = true;
                _unitOfWork.TruckBid.Update(bidItem);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("You dont have right to confirm");
            }

            return RedirectToAction(nameof(Index));
        }

        //Summary

        public IActionResult Summary(int postId)
        {
            Post_Truck post = _unitOfWork.Post_Truck.GetFirstOrDefault(u => u.Id == postId,  includeProperties: "ApplicationUser");
            TruckBid bidding = _unitOfWork.TruckBid.GetFirstOrDefault(u => u.TruckPostId == postId && u.Confirmed == true, includeProperties: "ApplicationUser");
            SummaryVM summary = new()
            {
                postTruck = post,
                bidTruck = bidding,
            };

            return View(summary);
        }

    }
}
