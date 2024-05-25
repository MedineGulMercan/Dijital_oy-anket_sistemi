using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using bitirmeProje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace bitirmeProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginUserHelper _loginUserHelper;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;

        public HomeController(ILogger<HomeController> logger, ILoginUserHelper loginUserHelper, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository)
        {
            _logger = logger;
            _loginUserHelper = loginUserHelper;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _loginUserHelper.GetLoginUserId();
            //var datax = await (from gu in _groupUserRepository.GetAll(x => x.UserId == userId)
            //                   join gr in _groupRepository.GetAll(x => true) on gu.GroupId equals gr.Id
            //                   join sr in _surveyRepository.GetAll(x => true) on gr.Id equals sr.GroupId
            //                   join qs in _questionRepository.GetAll(x => true) on sr.QuestionId equals qs.Id
            //                   select new SurveyInfoDto
            //                   {
            //                       QuestionId = qs.Id,
            //                       GroupName = gr.GroupName,
            //                       GroupDescription = gr.GroupDescription,
            //                       GroupId = gu.GroupId,
            //                       CanCreateSurvey = gr.CanCreateSurvey,
            //                       Private = gr.Private,
            //                       UserId = userId,
            //                       SurveyQuestion = qs.SurveyQuestion,
            //                       SurveyDescription = sr.SurveyDescription,
            //                       StartDate = sr.StartDate,
            //                       SurveyTittle = sr.SurveyTittle,
            //                       EndDate = sr.EndDate,
            //                   }).ToListAsync();

            //datax.ForEach(x => x.SurveyOptions = _optionRepository.GetAll(x => x.QuestionId == x.QuestionId));
            //ViewBag.SurveyInfo = datax;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}