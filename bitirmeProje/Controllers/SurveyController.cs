using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace bitirmeProje.Controllers
{
    public class SurveyController : Controller
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILoginUserHelper _loginUserHelper;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IOptionRepository _optionRepository;



        public SurveyController(IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, ILoginUserHelper loginUserHelper, IQuestionRepository questionRepository, ISurveyRepository surveyRepository, IOptionRepository optionRepository)
        {
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _loginUserHelper = loginUserHelper;
            _questionRepository = questionRepository;
            _surveyRepository = surveyRepository;
            _optionRepository = optionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetAllGroup()
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var data = await _groupUserRepository.GetAll(x => true).Include(x => x.Role).Include(x => x.Group).Where(x => x.UserId == userId && (x.Role.RoleName == "Yönetici" || x.Group.CanCreateSurvey == true)).ToListAsync();
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> SurveyCreate(SurveyDto surveyDto)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            surveyDto.UserId = userId;
            surveyDto.IsActive = true;
          
                var data = await _questionRepository.CreateAsync(new Question 
                {
                    SurveyQuestion= surveyDto.SurveyQuestion,
                    QuestionDescription = surveyDto.SurveyDescription,
                    QuestionTypeId = new Guid("67020fa7-62ba-4c0f-a6b0-067865d5f6f9"),
                });
            await _surveyRepository.CreateAsync(new Survey
            {
                SurveyTittle = surveyDto.SurveyTittle,
                SurveyDescription = surveyDto.SurveyDescription,
                StartDate = surveyDto.StartDate,
                EndDate = surveyDto.EndDate,
                GroupId = surveyDto.GroupId,
                QuestionId = data.Id,
                UserId = userId,
                IsActive = true,

            });
            try
            {
                foreach (var item in surveyDto.SurveyOptions)
                {
                    await _optionRepository.CreateAsync(new Option
                    {
                        QuestionId =data.Id,
                        Is_Active = true,
                        SurveyOption = item.SurveyOption,
                        Image_Url = false,
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
       
            return Json(new Response<Survey>
            {
                Success = true,
                Message = "Kayıt Başarılı",
            });

        }
    }
}
