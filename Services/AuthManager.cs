using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyHotelListing.Data;
using MyHotelListing.Moddels;

namespace MyHotelListing.Services
{
    public class AuthManager : IAuthManager
    {
        public readonly UserManager<ApiUser> _userManager;
        public readonly IConfiguration configuration;
        private  ApiUser _user;
        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var sgningCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOption = GenerateTokenOptions(sgningCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOption);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials sgningCredentials, List<Claim> claims)
        {
            var jwtSetting = configuration.GetSection("Jwt");
            var expirtion = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSetting.GetSection("lifetime").Value));

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSetting.GetSection("Issuer").Value,
                claims: claims,
                expires: expirtion,
                signingCredentials: sgningCredentials,
                audience: jwtSetting.GetSection("Issuer").Value
                );
            return jwtSecurityToken;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var Claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name,_user.UserName)
           };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return Claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            //var key = Environment.GetEnvironmentVariable("KEY");
            var key = configuration.GetSection("Jwt").GetSection("Key").Value;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginUserDTO loginDTO)
        {
            _user = await _userManager.FindByNameAsync(loginDTO.Email);
            return _user != null
                && await _userManager.CheckPasswordAsync(_user, loginDTO.Password);
        }
    }
}
