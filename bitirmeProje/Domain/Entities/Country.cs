using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Country : BaseEntity
    {
        [Required]
        [Column("country_name", TypeName = "nvarchar(max)")]
        public string CountryName { get; set; }
    }
}
