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
        private readonly IVoteRepository _voteRepository;

        public SurveyComponent(ILoginUserHelper loginUserHelper, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository, IVoteRepository voteRepository)
        {
            _loginUserHelper = loginUserHelper;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _voteRepository = voteRepository;
        }
        public IViewComponentResult Invoke(Guid? groupId)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            if (groupId == null)
            {
                var datax = (from gu in _groupUserRepository.GetAll(x => x.UserId == userId)
                             join gr in _groupRepository.GetAll(x => true) on gu.GroupId equals gr.Id
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

                foreach (var question in datax)
                {
                    var options = _optionRepository.GetAll(w => w.QuestionId == question.QuestionId)
                        .Select(x => new SurveyOptionDto
                        {
                            Id = x.Id,
                            Image_Url = x.Image_Url,
                            Is_Active = x.Is_Active,
                            QuestionId = x.QuestionId,
                            SurveyOption=x.SurveyOption
                        }).ToListAsync().Result;
                    foreach (var w in options)
                    {
                        var isVote =_voteRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.OptionId == w.Id).Result;
                        if (isVote != null)
                        {
                            w.IsVote = true;
                        }
                    }
                    question.SurveyOptions = options;
                }
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
                foreach (var question in datax)
                {
                    var options = _optionRepository.GetAll(w => w.QuestionId == question.QuestionId)
                        .Select(x => new SurveyOptionDto
                        {
                            Id = x.Id,
                            Image_Url = x.Image_Url,
                            Is_Active = x.Is_Active,
                            QuestionId = x.QuestionId,
                            SurveyOption=x.SurveyOption,
                        }).ToListAsync().Result;
                    foreach (var w in options)
                    {
                        var isVote = _voteRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.OptionId == w.Id).Result;
                        if (isVote != null)
                        {
                            w.IsVote = true;
                        }
                    }
                    question.SurveyOptions = options;
                }
                ViewBag.SurveyInfo = datax;
            }
            return View();
        }
    }
}
