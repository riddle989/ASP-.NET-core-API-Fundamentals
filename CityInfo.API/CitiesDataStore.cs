using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        // This is like singletone pattern,
        // this make sure we are working on the same datastore as long as we don't restart the web server
        //public static CitiesDataStore Current { get; set; } = new CitiesDataStore();

        public CitiesDataStore() { 
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Dhaka",
                    Description = "This is Dhaka City.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park-11",
                            Description = "The most Visited park-11"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Central Park-12",
                            Description = "The most Visited park-12"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Khulna",
                    Description = "This is Khulna City.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park-21",
                            Description = "The most Visited park-21"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Central Park-22",
                            Description = "The most Visited park-22"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Sylhet",
                    Description = "This is Sylhet City.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park-31",
                            Description = "The most Visited park-31"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Central Park-32",
                            Description = "The most Visited park-32"
                        }
                    }
                }
            };
        }
    }
}
