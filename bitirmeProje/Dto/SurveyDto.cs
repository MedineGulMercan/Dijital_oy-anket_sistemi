namespace bitirmeProje.Dto
{
    public class SurveyDto
    {
        public string SurveyQuestion { get; set; }
        public string SurveyTittle { get; set; }

        public string SurveyDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public Guid GroupId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
        public List<OptionDto> SurveyOptions { get; set; }

    }
}
