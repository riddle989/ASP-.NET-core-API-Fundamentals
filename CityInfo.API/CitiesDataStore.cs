using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        // This is like singletone pattern, this make sure we are working on the same datastore as long as we restart the web server
        public static CitiesDataStore Current { get; set; } = new CitiesDataStore();

        public CitiesDataStore() { 
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Dhaka",
                    Description = "This is Dhaka City."
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Khulna",
                    Description = "This is Khulna City."
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Sylhet",
                    Description = "This is Sylhet City."
                }
            };
        }
    }
}
