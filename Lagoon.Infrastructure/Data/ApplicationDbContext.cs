using Lagoon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //add-migration  run krnna kain meka uncaomment krnna one. 
            // mokada table tika hadenakota onmodel creteing method eka override wenna one.
            // Hethuwa thama the keys for the identity table are map on the onModelCreating method of the IdentityDbContex
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                 new Villa
                 {
                     id = 1,
                     Name = "Royal Villa",
                     Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                     ImageUrl = "https://placehold.co/600x400",
                     Occupancy = 4,
                     Price = 200,
                     Sqft = 550,
                 },
                    new Villa
                    {
                        id = 2,
                        Name = "Premium Pool Villa",
                        Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://placehold.co/600x401",
                        Occupancy = 4,
                        Price = 300,
                        Sqft = 550,
                    },
                    new Villa
                    {
                        id = 3,
                        Name = "Luxury Pool Villa",
                        Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                        ImageUrl = "https://placehold.co/600x402",
                        Occupancy = 4,
                        Price = 400,
                        Sqft = 750,
                    }
                );
        }
        }
}
