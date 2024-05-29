using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [Column("name", TypeName = "nvarchar(max)")]
        public string Name { get; set; }

        [Required]
        [Column("surname", TypeName = "nvarchar(max)")]
        public string Surname { get; set; } 
        
        [Required]
        [Column("username", TypeName = "nvarchar(max)")]
        public string Username { get; set; }

        [Required]
        [Column("mail", TypeName = "nvarchar(max)")]
        public string Mail { get; set; }

        [Required]
        [Column("password", TypeName = "nvarchar(max)")]
        public string Password { get; set; }

        [Column("image_url", TypeName = "nvarchar(max)")]
        public string? ImageUrl { get; set; }

        [Required]
        [Column("phone_number", TypeName = "nvarchar(max)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("birthday")]
        public DateTime Birthday { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; }

        [Required]
        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        [Required]
        [Column("gender_id")]
        [ForeignKey("gender_id")]
        public Guid GenderId { get; set; }
        public virtual Gender Gender { get; set; }  
    }
}
