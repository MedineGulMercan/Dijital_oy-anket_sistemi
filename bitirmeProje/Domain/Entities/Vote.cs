using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Vote : BaseEntity
    {
        [Required]
        [Column("user_id")]
        [ForeignKey("user_id")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [Column("option_id")]
        [ForeignKey("option_id")]
        public Guid OptionId { get; set; }
        public virtual Option Option { get; set; }
    }
}
