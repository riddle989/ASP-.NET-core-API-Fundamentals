using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper) 
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities(
            [FromQuery(Name = "filteronname")] string? name, string? searchQuery,
            int pageNumber = 1, int pageSize = 10) /*We can omit this explicit annotation, as it is not complex type, 
                                                              * if parameter name is same as query parameter name, 
                                                              * then it will automatically take it from query string
                                                              */
        {

            if(pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            // We only make async method of data fetching from the database
            // As soon as data retrived, we start processing the data, so don't need the asynchronous method
            var (cityEntities, paginationMetadata) = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add(
                "X-Pagination", JsonSerializer.Serialize(paginationMetadata));

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

        /* The parameter "includePointsOfInterest" will be automatically mapped, but its good parctice to explicity declare it */
        [HttpGet("{id}")]
        /* The return type is not correct here, type can be "CityDto" or "CityWithoutPointsOfInterestDto" */
        //public async Task<ActionResult<CityDto>> GetCity(

        public async Task<IActionResult> GetCity(
            int id, bool includePointsOfInterest = false) 
        {
            /* We will use Entity class instead*/
            //var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
            //if (cityToReturn == null) 
            //{
            //    return NotFound();
            //}
            //return Ok(cityToReturn);

            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }
            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
