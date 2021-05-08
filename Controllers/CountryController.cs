using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHotelListing.Data;
using MyHotelListing.IRepository;
using MyHotelListing.Moddels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception)
            {
                _logger.LogError($"somthing went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception)
            {
                _logger.LogError($"somthing went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
                throw;
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetCountry", new { Id = country.Id }, country);
            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(CreateCountry)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }


        //[Authorize(Roles = "Administrator")]
        [HttpPut("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int Id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || Id < 1)
            {
                _logger.LogError(500, $"Invalid Update Attap to {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == Id);
                if (country == null)
                {
                    _logger.LogError(500, $"Invalid Update Attap to {nameof(UpdateCountry)}");
                    return BadRequest("submit data in invalid");
                }
                _mapper.Map(countryDTO, country);//سورس اصلی دی تو هست برای همین اول می نویسیم
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int Id)
        {
            if (Id < 1)
            {
                _logger.LogError(500, $"Invalid Delete Attap to {nameof(DeleteCountry)}");
                return BadRequest();
            }
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == Id);
                if (country == null)
                {
                    _logger.LogError(500, $"Invalid Update Attap to {nameof(DeleteCountry)}");
                    return BadRequest("submit data in invalid");
                }
                await _unitOfWork.Countries.Delete(Id);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception)
            {
                _logger.LogError(500, $"somthing went wrong in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Intrnall server Error.plessa try agin Later.");
            }
        }
    }
}
