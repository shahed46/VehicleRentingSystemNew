using System.ComponentModel.DataAnnotations;

namespace VehicleRentingSystem.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string PickUpLocation { get; set; }
        public string DropOutLoaction { get; set; }
        public int NumberOfPassenger { get; set; }
        public string? Description { get; set; }

    }
}
