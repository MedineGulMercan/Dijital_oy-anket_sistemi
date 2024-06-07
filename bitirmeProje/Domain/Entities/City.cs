using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class City : BaseEntity
    {
        [Required]
        [Column("city_name", TypeName = "nvarchar(max)")]
        public string CityName { get; set; }

        [Required]
        [Column("country_id")]
        [ForeignKey("country_id")]
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; } 
    }
}
