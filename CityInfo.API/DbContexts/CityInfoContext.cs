using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!; // "null" forgiving operator
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

        /*
         // This is one way of configuring the database
         */
        public CityInfoContext(DbContextOptions<CityInfoContext> options) 
            : base(options) // this will pass the configured "options" to the base class "DbContext" at the moment we register our dbContext in the program.cs
        {
        }

        /*
        //This is another way of configuring connection string of the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("CONNECTION_STRING");
            base.OnConfiguring(optionsBuilder);
        }
        */


        // Providing seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                    new City("Dhaka")
                    {
                        Id = 1,
                        Description = "This is Dhaka City."
                    },
                    new City("Sylhet")
                    {
                        Id = 2,
                        Description = "This is Sylhet City."
                    },
                    new City("Khulna")
                    {
                        Id = 3,
                        Description = "This is Khulna City."
                    });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(

                    new PointOfInterest("Central Park-11")
                    {
                        Id = 1,
                        CityId = 1,
                        Description = "The most Visited park-11"
                    },
                    new PointOfInterest("Central Park-12")
                    {
                        Id = 2,
                        CityId = 1,
                        Description = "The most Visited park-12"
                    },
                    new PointOfInterest("Central Park-21")
                    {
                        Id = 3,
                        CityId = 2,
                        Description = "The most Visited park-21"
                    },
                    new PointOfInterest("Central Park-22")
                    {
                        Id = 4,
                        CityId = 2,
                        Description = "The most Visited park-22"
                    },
                    new PointOfInterest("Central Park-31")
                    {
                        Id = 5,
                        CityId = 3,
                        Description = "The most Visited park-31"
                    },
                    new PointOfInterest("Central Park-32")
                    {
                        Id = 6,
                        CityId = 3,
                        Description = "The most Visited park-32"
                    });

            base.OnModelCreating(modelBuilder);
        }


    }
}
