using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitirmeProje.Domain.Entities
{
    public class District:BaseEntity
    {
        [Required]
        [Column("district_name", TypeName ="nvarchar(max)")]
        public string DistrictName { get; set; }

        [Required]
        [Column("city_id")]
        [ForeignKey("city_id")]
        public Guid CityId { get; set; }
        public virtual City City { get; set; }

    }
}
