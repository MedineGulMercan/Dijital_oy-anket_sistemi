using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class Option : BaseEntity
    {

        [Required]
        [Column("survey_option", TypeName = "nvarchar(max)")]
        public string SurveyOption { get; set; }

        [Required]
        [Column("question_id")]
        [ForeignKey("question_id")]
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }


        [Required]
        [Column("is_active")]
        public bool Is_Active { get; set; }

        [Column("image_url")]
        public bool Image_Url { get; set; }
    }
}
