using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            CitiesDataStore citiesDataStore,
            IMapper mapper,
            ICityInfoRepository cityInfoRepository) 
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            //_citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));


            // We can always request a service from the container directly, If we want to dependency injection throughe constructor
            //HttpContext.RequestServices.GetService(SERVICE_NAME)

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            /* We will use Entities instead */
            ///* throw new NotImplementedException("Custom Exception sample") */
            //try
            //{
            //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //    //Console.WriteLine("FFFFFFFF "+ cityId);

            //    if (city == null)
            //    {
            //        _logger.LogInformation($"City with id {cityId} wasn't found....!");
            //        return NotFound();
            //    }
                
            //    return Ok(city.PointsOfInterest);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogCritical(
            //        $"City with id {cityId} wasn't found....!",
            //        ex);
            //    // As we are catching the error, We need to manually send the error to the response
            //    return StatusCode(500, "Custom msg - A problem happend while handling your reqeust.");
            //}
            
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found....!");
                return NotFound();
            }

            var PointOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(PointOfInterestForCity));
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            /* We will use Entities instead */
            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null) 
            //{
            //    return NotFound(); 
            //}
            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            //if (pointOfInterest == null) { return NotFound(); }
            //return Ok(pointOfInterest);

            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }


        /*
        [HttpPost]
        public ActionResult<PointOfInterestForCreationDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterestForCreationDto) //We can explicitly say it is [FromBody] attribute
        {
            // We can explicitly check wheteher the given input is valid according to the model's specified data annotation
            // But it is done automatically
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}


            //Create new instance of the object each time this action is called 
            //var citiesDynamic = new CitiesDataStore().Cities;

            //Preserve the same instances throughout the application life cycle
            //var citiesStatic = CitiesDataStore.Current.Cities;

            var cities = _citiesDataStore.Cities;

            var city = cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestForCreationDto.Name,
                Description = pointOfInterestForCreationDto.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            // Returning generic response
            //return Ok(finalPointOfInterest);

            // Returning created object with how the new object can be found by API calling
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                finalPointOfInterest);
        }


        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null) { return NotFound(); }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null) { return NotFound(); }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };

            // To check if there are any error while applying the given payload to the actual data
            // like, client send an invalid property that is not present in the actual object
            // and any error of that type make the "ModelState" invalid, so we pass it to the "ApplyTo" method
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // To check if there are any error after applying the given payload against the model's data annotation / rules
            // like, after successfully applying the patch, the actual model's rule can't be satisfied
            //ex. "Required" filed removed or "Maxlength" field cross the limit
            if (!TryValidateModel(pointOfInterestToPatch)) { return BadRequest(ModelState); }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description= pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null) { return NotFound(); }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send(
                "Point of interest deleted",
                $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");

            return NoContent();
        }*/

    }
}
