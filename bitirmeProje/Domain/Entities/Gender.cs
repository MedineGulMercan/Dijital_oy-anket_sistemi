using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Gender :BaseEntity
    {
        [Required]
        [Column("gender_name", TypeName = "nvarchar(max)")]
        public string GenderName { get; set; }
    }
}
