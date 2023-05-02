using CityInfo.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class City
    {
        /* 
         * ## By default Entity framework will automatically recognize it as a Key
                but by convention we will explicitly declare it as Key as data annotations
           ## The key generation responsibility is assign to the DB 
        */
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        /* ## This is not a outer facing model, "NumberOfPointsOfInterest" field is not saved in DB
           ## We will use it only API response
        public int NumberOfPointsOfInterest { get { return PointsOfInterest.Count; }}
        */

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();

        // This class can accept parameters when created
        public City(string name)
        {
            Name = name;
        }
    }
}
