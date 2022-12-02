using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.Models.ViewModels
{
    public class ProfileVM
    {
        public  IEnumerable<Post_Car> post_Cars { get; set; }
        public Bid bidHistory { get; set; }
        public IEnumerable<ApplicationUser> applicationUser { get; set; }
    }
}
