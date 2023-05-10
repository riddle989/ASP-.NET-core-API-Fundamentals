using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            // add order by name of the return result   
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }


        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            /* We will always use pagination   */
            //if (string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    return await GetCitiesAsync();
            //}

            var collection = _context.Cities as IQueryable<City>;
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery)) 
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => (c.Name.Contains(searchQuery)) 
                || (c.Description != null && c.Description.Contains(searchQuery)));
            }

            return await collection
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            /* Search and filtering combined */
            //name = name.Trim();
            //return await _context.Cities
            //    .Where(c => c.Name == name)
            //    .OrderBy(c => c.Name)
            //    .ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            // if client need points of interest
            if(includePointOfInterest)
            {
                return await _context.Cities.Include(p => p.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            // else only single city
            return await _context.Cities
                .Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            //Need to consider cityId and Pointof interest id
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync(); // This line successfully execute query
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId)
                .ToListAsync();// This is line create a list and successfully execute query
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                /* ## This method save the object in the inmemory representation of the object
                   ## We have to save it explicitly in the database to save it in the database
                   ## To save it directly on the database we can use 'AddAsync()' method
                */
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0); // returns true when zero or more entries would be changed
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
        }
    }
}
