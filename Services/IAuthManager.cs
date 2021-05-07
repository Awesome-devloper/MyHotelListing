using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHotelListing.Moddels;

namespace MyHotelListing.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO loginDTO);
        Task<string> CreateToken();
    }
}
