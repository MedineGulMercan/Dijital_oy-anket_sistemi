using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Group :BaseEntity
    {
        [Required]
        [Column("group_owner_id")]
        //[ForeignKey("group_owner_id")]

        public Guid GroupOwnerId { get; set; }
        //public virtual User GroupOwner { get; set; }


        [Required]
        [Column("group_name", TypeName = "nvarchar(max)")]
        public string GroupName{ get; set; }

        [Column("group_description", TypeName = "nvarchar(max)")]
        public string GroupDescription{ get; set; }

        [Column("image_url", TypeName = "nvarchar(max)")]
        public string ImageUrl{ get; set; }

        [Required]
        [Column("private")]
        public bool Private { get; set; }

        [Required]
        [Column("can_create_survey")]
        public bool CanCreateSurvey { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
