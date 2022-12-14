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
            //IEnumerable<Post_Truck> objPostTruckyList = _unitOfWork.Post_Truck.GetAll(includeProperties: "Bid");
            //return View(objPostTruckyList);
            PostVM postVM = new()
            {
                objPostTruck = _unitOfWork.Post_Truck.GetAll(),
                
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
        public IActionResult Create(Post_Truck obj, IFormFile file)
        {
            //getting logged user id

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //getting logged user id end
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
            TruckBid truckbid = new TruckBid()
            {
                TruckPostId = postId,
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

    }
}
