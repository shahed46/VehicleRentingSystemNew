using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Collections.Generic;
using VehicleRentingSystem.Models;

namespace VehicleRentingSystem.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Post_Car> Post_Cars { get; set; }
        public DbSet<City> Cities { get; set; }
       
        public DbSet<Test> Tests { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Post_Truck> Post_Trucks { get; set; }
		public DbSet<TruckBid> TruckBids { get; set; }


	}
}
