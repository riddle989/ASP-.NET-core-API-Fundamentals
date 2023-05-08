using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        // "IQueryable" allows us to order/query etc over the data before we fetched the data,
        // But it violates the repository pattern, because that is the data layer's responsibility
        /* IQueryable<City> GetCities(); */

        // This is a sychronous method
        /* IEnumerable<City> GetCities(); */

        // This is Asynchronous method
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<bool> CityExistsAsync(int cityId);

        // Result can be null if no city is found, so made it nullable 
        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);

        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
            int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        Task<bool> SaveChangesAsync();
    }
}
