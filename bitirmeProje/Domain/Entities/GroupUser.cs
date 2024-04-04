using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace bitirmeProje.Domain.Entities
{
    public class GroupUser :BaseEntity 
    {

        [Required]
        [Column("user_id")]
        [ForeignKey("user_id")]
        public Guid UserId{ get; set; }
        public virtual User User { get; set; }

        [Required]
        [Column("role_id")]
        [ForeignKey("role_id")]
        public Guid RoleId{ get; set; }
        public virtual Role Role { get; set; }

        [Required]
        [Column("group_id")]
        [ForeignKey("group_id")]
        public Guid GroupId{ get; set; }
        public virtual Group Group { get; set; }

        [Required]
        [Column("is_member")]
        public bool IsMember{ get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive{ get; set; }
    }
}
