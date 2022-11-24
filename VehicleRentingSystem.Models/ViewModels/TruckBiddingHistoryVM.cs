using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.Models.ViewModels
{
    public class TruckBiddingHistoryVM
    {
        public IEnumerable<TruckBid> bidHistory { get; set; }
        IEnumerable<ApplicationUser> applicationUser { get; set; }
        public bool booked { get; set; }
    }
}
