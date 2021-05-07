using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHotelListing.Data;
using MyHotelListing.IRepository;
using MyHotelListing.Moddels;
using MyHotelListing.Services;

namespace MyHotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        //سرویس های هستند که به صورت اماده توست ا اس پی  کور پیاده سازی شده اند و نیازی برای پیاده سازی این سرویس ها در یونتیت اف ورک نیست
        public readonly UserManager<ApiUser> _userManager;
        //public readonly SignInManager<ApiUser> _signInManager;
        //public readonly RoleManager<ApiUser> _roleManager;

        public AccountController(ILogger<AccountController> logger, IMapper mapper, UserManager<ApiUser> userManager, IAuthManager authManager)//, SignInManager<ApiUser> signInManager)
        {
            _userManager = userManager;
           // _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attemp to {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        _logger.LogError($"User Registration ERROR Code:{error.Code},Desc:{error.Description}");
                    }
                  
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception)
            {

                _logger.LogError($"somthing went wrong in the {nameof(Register)}");
                return Problem($"Something went Wrrong in {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"LoginDTO Attemp to {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authManager.ValidateUser(userDTO))
                    return Unauthorized();

                return Accepted(new { Token = await _authManager.CreateToken() });
            }
            catch (Exception)
            {
                _logger.LogError($"somthing went wrong in the {nameof(Login)}");
                return Problem($"Something went Wrrong in {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
