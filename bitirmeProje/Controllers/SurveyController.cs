using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
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
        private readonly IVoteRepository _voteRepository;
        public SurveyController(IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, ILoginUserHelper loginUserHelper, IQuestionRepository questionRepository, ISurveyRepository surveyRepository, IOptionRepository optionRepository, IVoteRepository voteRepository)
        {
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _loginUserHelper = loginUserHelper;
            _questionRepository = questionRepository;
            _surveyRepository = surveyRepository;
            _optionRepository = optionRepository;
            _voteRepository = voteRepository;
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
                SurveyQuestion = surveyDto.SurveyQuestion,
                QuestionDescription = surveyDto.SurveyDescription,
                QuestionTypeId = new Guid("67020fa7-62ba-4c0f-a6b0-067865d5f6f9"),
                ImageUrl = ""
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
                        QuestionId = data.Id,
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
        [HttpPost]
        public async Task<IActionResult> SetVote(Guid optionId, Guid questionId)
        {
            var survey = await _surveyRepository.FirstOrDefaultAsync(x => x.QuestionId == questionId);

            if (survey?.EndDate >= DateTime.Now)
            {
                var userId = _loginUserHelper.GetLoginUserId();
                var options = _optionRepository.GetAll(x => x.QuestionId == questionId);
                foreach (var item in options)
                {
                    var optionVote = await _voteRepository.FirstOrDefaultAsync(x => x.OptionId == item.Id && x.UserId==userId);
                    if (optionVote != null)
                    {
                        await _voteRepository.DeleteAsync(optionVote);
                    }
                }
                await _voteRepository.CreateAsync(new Vote
                {
                    OptionId = optionId,
                    UserId = userId,
                });
                return Json(new Response<Vote>
                {
                    Success = true,
                    Message = "Cevabınız Kayıt Edilmiştir",
                });
            }
            return Json(new Response<Vote>
            {
                Success = false,
                Message = "Kayıt Başarısız",
            });

        }

        [HttpPost]
        public async Task<IActionResult> GetVoteReport(Guid questionId)
        {
            var questionOptions = await _optionRepository.GetAll(x => x.QuestionId == questionId).ToListAsync();
            var ids = questionOptions.Select(w => w.Id);
            var data = _voteRepository.GetAll(x => ids.Contains(x.OptionId))
                 .GroupBy(x => x.OptionId)
                 .Select(group => new
                 {
                     OptionId = group.Key, // Grup ID'si (OptionId)
                     Count = group.Count() // Grup içindeki eleman sayısı
                 }).ToList();

            var list = new List<SurveyReportDto>();
            foreach (var item in data)
            {
                list.Add(new SurveyReportDto
                {
                    Name = questionOptions.FirstOrDefault(x=>x.Id == item.OptionId).SurveyOption,
                    Count = item.Count
                });
            }
            return Json(list);
        }
    }
}
