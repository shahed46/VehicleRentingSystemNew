using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.Models.ViewModels
{
    public class SummaryVM
    {
        public Post_Car post { get; set; }
        public Bid bid { get; set; }
        public Post_Truck postTruck { get; set; }
        public TruckBid bidTruck { get; set; }
        public ApplicationUser user { get; set; }
    }
}
