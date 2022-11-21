using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? LicenseNo { get; set; }
        public string? VehicleNo { get; set; }
        public string? CarPicUrl { get; set; }



    }
}
