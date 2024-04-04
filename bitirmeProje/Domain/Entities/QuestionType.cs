using bitirmeProje.Domain.Entities.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bitirmeProje.Domain.Entities
{
    public class QuestionType : BaseEntity
    {
        [Required]
        [Column("question_type_name", TypeName = "nvarchar(max)")]
        public string QuestionTypeName { get; set; }
    }
}
