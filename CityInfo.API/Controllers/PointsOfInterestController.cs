using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //Console.WriteLine("FFFFFFFF "+ cityId);

            if (city == null) { return NotFound(); }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null) { return NotFound(); }

            return Ok(pointOfInterest);
        }

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
            var citiesDynamic = new CitiesDataStore().Cities;

            //Preserve the same instances throughout the application life cycle
            var citiesStatic = CitiesDataStore.Current.Cities;

            var city = citiesStatic.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null) { return NotFound(); }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            // To check if there are any error while applying the given payload to the actual data
            // like, client send an invalid property that is not present in the actual object
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) { return NotFound(); }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null) { return NotFound(); }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }

    }
}
