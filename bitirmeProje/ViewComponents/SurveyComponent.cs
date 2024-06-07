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
        private readonly IUserRepository _userRepository;

        public SurveyComponent(ILoginUserHelper loginUserHelper, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository, IVoteRepository voteRepository, IUserRepository userRepository)
        {
            _loginUserHelper = loginUserHelper;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _voteRepository = voteRepository;
            _userRepository = userRepository;
        }
        public IViewComponentResult Invoke(Guid? groupId)
        {
            ViewBag.UserId = _loginUserHelper.GetLoginUserId();
            var userId = _loginUserHelper.GetLoginUserId();
            if (groupId == null)
            {
                var datax = (from gu in _groupUserRepository.GetAll(x => x.UserId == userId)
                             join gr in _groupRepository.GetAll(x => true) on gu.GroupId equals gr.Id
                             join sr in _surveyRepository.GetAll(x => x.IsActive) on gr.Id equals sr.GroupId
                             join qs in _questionRepository.GetAll(x => true) on sr.QuestionId equals qs.Id
                             select new SurveyInfoDto
                             {
                                 Id = sr.Id,
                                 QuestionId = qs.Id,
                                 GroupName = gr.GroupName,
                                 CreatedDate = sr.CreatedDate,
                                 GroupDescription = gr.GroupDescription,
                                 GroupId = gu.GroupId,
                                 CanCreateSurvey = gr.CanCreateSurvey,
                                 Private = gr.Private,
                                 UserId = sr.UserId,
                                 SurveyQuestion = qs.SurveyQuestion,
                                 SurveyDescription = sr.SurveyDescription,
                                 StartDate = sr.StartDate,
                                 SurveyTittle = sr.SurveyTittle,
                                 EndDate = sr.EndDate,
                                 ImageUrl = gr.ImageUrl
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
                var result = datax.OrderByDescending(x => x.CreatedDate).ToList();
                ViewBag.SurveyInfo = result;
            }
            else
            {
                //Grubun anketlerini ve anket sorularını çekiyoruz.
                var datax = (from g in _groupRepository.GetAll(x => x.Id == groupId)
                             join sr in _surveyRepository.GetAll(x => x.IsActive) on g.Id equals sr.GroupId
                             join qu in _questionRepository.GetAll(x => true) on sr.QuestionId equals qu.Id
                             select new SurveyInfoDto
                             {
                                 Id = sr.Id,
                                 QuestionId = qu.Id,
                                 CreatedDate = sr.CreatedDate,
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
                {   // Option tablosundan o grubun sorularının şıklarını çekiyoruz.
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
                    {  //Kullanıcı daha önce oylamış mı diye kontrol ediyoruz
                        var isVote = _voteRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.OptionId == w.Id).Result;
                        if (isVote != null)
                        {
                            w.IsVote = true;
                        }
                    }
                    question.SurveyOptions = options;
                }
                var result = datax.OrderByDescending(x => x.CreatedDate).ToList();
                ViewBag.SurveyInfo = result;
            }
            return View();
        }
    }
}
