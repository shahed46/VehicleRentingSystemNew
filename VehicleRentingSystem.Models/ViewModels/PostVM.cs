using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.Models.ViewModels
{
    public class PostVM
    {
        public IEnumerable<Post_Car> objPostCart { get; set; }
		public IEnumerable<Post_Truck> objPostTruck { get; set; }
		public Bid Bid { get; set; }
    }
}
