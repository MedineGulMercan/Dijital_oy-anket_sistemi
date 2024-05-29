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

        [Column("question_description", TypeName = "nvarchar(max)")]
        public string QuestionDescription { get; set; }
       
        [Column("image_url", TypeName = "nvarchar(max)")]
        public string ImageUrl { get; set; } 
    }
}
