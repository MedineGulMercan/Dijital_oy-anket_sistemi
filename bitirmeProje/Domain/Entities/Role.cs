using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Role :BaseEntity
    {
        [Required]
        [Column("role_name", TypeName = "nvarchar(max)")]
        public string RoleName { get; set; }
    }
}
