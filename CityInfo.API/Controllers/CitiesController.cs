using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore) 
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            var cities = _citiesDataStore.Cities;

            return Ok(cities);
        }

        [HttpGet("{cityId}")]
        public ActionResult<CityDto> GetCity(int cityId) 
        { 
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}
