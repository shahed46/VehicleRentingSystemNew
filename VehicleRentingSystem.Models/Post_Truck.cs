using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentingSystem.Models
{
    public class Post_Truck
    {
        [Key]
        public int Id { get; set; }
        public string PickUpLocation { get; set; }
        public string DropOutLoaction { get; set; }
        public string ProductName { get; set; }
        public string? Product_Des { get; set; }
        public string? PickUpDate { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }


    }
}
