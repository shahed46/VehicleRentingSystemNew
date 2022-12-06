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
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
       
        public int Bidding { get; set; }
        public bool? Confirmed { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }
		[NotMapped]
		public double? MinBid { get; set; }
	}
}
