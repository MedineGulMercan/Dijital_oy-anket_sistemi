using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Survey : BaseEntity
    {

        [Required]
        [Column("survey_tittle", TypeName = "nvarchar(max)")]
        public string SurveyTittle { get; set; }

        [Required]
        [Column("survey_description", TypeName = "nvarchar(max)")]
        public string SurveyDescription { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; }

        [Required]
        [Column("group_id")]
        [ForeignKey("group_id")]
        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }  

        [Required]
        [Column("question_id")]
        [ForeignKey("question_id")]
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [Required]
        [Column("user_id")]
        [ForeignKey("user_id")]

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

    }
}
