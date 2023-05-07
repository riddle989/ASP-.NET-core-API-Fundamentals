using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper) 
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            // We only make async method of data fetching from the database
            // As soon as data retrived, we start processing the data, so don't need the asynchronous method
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

            // We will use instead of below code
            //var results = new List<CityWithoutPointsOfInterestDto>();
            //foreach (var city in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = city.Id,
            //        Description = city.Description,
            //        Name = city.Name,
            //    });
            //}
            //return Ok(results);


            // We will use Entity and database instead
            //return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id) 
        {
            //var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            //if (cityToReturn == null) 
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);

            return Ok();
        }
    }
}
