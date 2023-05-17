using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {

        public CitiesController() 
        { }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            var cities = CitiesDataStore.Current.Cities;

            return Ok(cities);
        }

        [HttpGet("{cityId}")]
        public ActionResult<CityDto> GetCity(int cityId) 
        { 
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}
