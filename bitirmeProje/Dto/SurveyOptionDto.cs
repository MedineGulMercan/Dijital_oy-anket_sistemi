namespace bitirmeProje.Dto
{
    public class SurveyOptionDto
    {
        public string SurveyOption { get; set; }
        public Guid QuestionId { get; set; }
        public Guid Id { get; set; }
        public bool Is_Active { get; set; }
        public bool Image_Url { get; set; }
        public bool IsVote { get; set; }
    }
}
