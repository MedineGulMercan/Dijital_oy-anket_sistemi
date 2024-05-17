using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace bitirmeProje.Domain.Entities
{
    public class Question :BaseEntity
    {

        [Required]
        [Column("survey_question", TypeName = "nvarchar(max)")]
        public string SurveyQuestion { get; set; }
        //askım isimle ne ilgisi var anket sorusu işte doğru tasarımla aynı 
        [Column("question_description", TypeName = "nvarchar(max)")]
        public string QuestionDescription { get; set; }
       
        [Required]
        [Column("question_type_id")]
        [ForeignKey("question_type_id")]
        public Guid QuestionTypeId { get; set; }
        public virtual QuestionType QuestionType { get; set; }

        [Column("image_url", TypeName = "nvarchar(max)")]
        public string ImageUrl { get; set; } 
    }
}
