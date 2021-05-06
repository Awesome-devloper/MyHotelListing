using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder _builder)
        {
            _builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Iran",
                    ShortName = "IR"
                },
                                new Country
                                {
                                    Id = 2,
                                    Name = "UnitedStetad",
                                    ShortName = "US"
                                },
                                                new Country
                                                {
                                                    Id = 3,
                                                    Name = "Germany",
                                                    ShortName = "GM"
                                                }
                );
            _builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "HotelEstglal",
                    Adderss = "Tehran",
                    CountryId = 1,
                    Rating=4.5
                   
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Malhatan Tower",
                    Adderss = "NewYork",
                    CountryId = 2,
                    Rating = 5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "BerlinHotel",
                    Adderss = "Berlin",
                    CountryId = 3,
                    Rating = 4.25
                }
                );
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

    }
}
