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
    [Authorize] 
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