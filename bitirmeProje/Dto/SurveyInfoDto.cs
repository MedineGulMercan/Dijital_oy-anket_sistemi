﻿using bitirmeProje.Domain.Entities;

namespace bitirmeProje.Dto
{
    public class SurveyInfoDto
    {
        public Guid Id { get; set; }
        public string SurveyQuestion { get; set; }
        public string SurveyTittle { get; set; }

        public string SurveyDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string ImageUrl { get; set; }
        public bool Private { get; set; }
        public bool CanCreateSurvey { get; set; }
        public string UserImage { get; set; }
        public Guid QuestionId { get; set; }
        public List<SurveyOptionDto> SurveyOptions { get; set; }

        public Guid UserId { get; set; }

    }
}
