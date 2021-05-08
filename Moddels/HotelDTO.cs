using MyHotelListing.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotelListing.Moddels
{

    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "نام هتل طولانی است")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "آدرس هتل طولانی است")]
        public string Adderss { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        public int CountryId { get; set; }
    }
    public class UpdateHotelDTO: CreateHotelDTO
    {
        
    }
    public class HotelDTO: CreateHotelDTO
    {
        public int Id { get; set; }
        public Country Country { get; set; }
    }
}
