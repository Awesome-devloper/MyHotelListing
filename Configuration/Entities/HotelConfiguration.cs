﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelListing.Data;

namespace MyHotelListing.Configuration.Entities
{
    public class HotelConfiguration: IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> _builder)
        {
            _builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "HotelEstglal",
                    Adderss = "Tehran",
                    CountryId = 1,
                    Rating = 4.5

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
    }
}
