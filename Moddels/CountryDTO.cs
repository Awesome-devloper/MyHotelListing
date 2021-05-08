﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotelListing.Moddels
{

    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "نام کشور طولانی است")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "نام مخقق کشور طولانی است")]
        public string ShortName { get; set; }

    }
    public class UpdateCountryDTO: CreateCountryDTO
    {
        public IList<CreateHotelDTO> Hotels { get; set; }
    }

    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }
        public virtual List<HotelDTO> Hotels { get; set; }

    }
}
