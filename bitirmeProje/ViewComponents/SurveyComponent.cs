using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitirmeProje.ViewComponents
{
    public class SurveyComponent : ViewComponent
    {
        private readonly ILoginUserHelper _loginUserHelper;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;

        public SurveyComponent(ILoginUserHelper loginUserHelper, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository)
        {
            _loginUserHelper = loginUserHelper;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
        }
        public IViewComponentResult Invoke(Guid? groupId)
        {
            var userId = _loginUserHelper.GetLoginUserId();

            if (groupId == null)
            {
                var datax =  (from gu in _groupUserRepository.GetAll(x => x.UserId == userId)
                                   join gr in _groupRepository.GetAll(x =>true) on gu.GroupId equals gr.Id
                                   join sr in _surveyRepository.GetAll(x => true) on gr.Id equals sr.GroupId
                                   join qs in _questionRepository.GetAll(x => true) on sr.QuestionId equals qs.Id
                                   select new SurveyInfoDto
                                   {
                                       QuestionId = qs.Id,
                                       GroupName = gr.GroupName,
                                       GroupDescription = gr.GroupDescription,
                                       GroupId = gu.GroupId,
                                       CanCreateSurvey = gr.CanCreateSurvey,
                                       Private = gr.Private,
                                       UserId = userId,
                                       SurveyQuestion = qs.SurveyQuestion,
                                       SurveyDescription = sr.SurveyDescription,
                                       StartDate = sr.StartDate,
                                       SurveyTittle = sr.SurveyTittle,
                                       EndDate = sr.EndDate,
                                   }).ToListAsync().Result;
                datax.ForEach(x => x.SurveyOptions = _optionRepository.GetAll(x => x.QuestionId == x.QuestionId));
                ViewBag.SurveyInfo = datax;
            }
            else
            {
                var datax = (from g in _groupRepository.GetAll(x => x.Id == groupId)
                                  join sr in _surveyRepository.GetAll(x => true) on g.Id equals sr.GroupId
                                  join qu in _questionRepository.GetAll(x => true) on sr.QuestionId equals qu.Id
                                  select new SurveyInfoDto
                                  {
                                      QuestionId = qu.Id,
                                      GroupName = g.GroupName,
                                      GroupDescription = g.GroupDescription,
                                      GroupId = g.Id,
                                      CanCreateSurvey = g.CanCreateSurvey,
                                      Private = g.Private,
                                      UserId = userId,
                                      SurveyQuestion = qu.SurveyQuestion,
                                      SurveyDescription = sr.SurveyDescription,
                                      StartDate = sr.StartDate,
                                      SurveyTittle = sr.SurveyTittle,
                                      EndDate = sr.EndDate,
                                  }).ToListAsync().Result;
                datax.ForEach(x => x.SurveyOptions = _optionRepository.GetAll(x => x.QuestionId == x.QuestionId));
                ViewBag.SurveyInfo = datax;
            }
            return View();
        }
    }
}
