using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitirmeProje.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ILoginUserHelper _loginUserHelper;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOptionRepository _optionRepository;

        public GroupController(IGroupRepository groupRepository, ILoginUserHelper loginUserHelper, IGroupUserRepository groupUserRepository, IRoleRepository roleRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IUserRepository userRepository, IOptionRepository optionRepository)
        {
            _groupRepository = groupRepository;
            _loginUserHelper = loginUserHelper;
            _groupUserRepository = groupUserRepository;
            _roleRepository = roleRepository;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _optionRepository = optionRepository;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var data = await (from gr in _groupRepository.GetAll(x => x.Id == id)
                              join sr in _surveyRepository.GetAll(x => true) on gr.Id equals sr.GroupId
                              join qs in _questionRepository.GetAll(x => true) on sr.QuestionId equals qs.Id
                              //join gu in _groupUserRepository.GetAll(x=>true) on gr.Id equals gu.GroupId 
                              //join u in _userRepository.GetAll(x=>true) on gu.UserId equals u.Id
                              select new SurveyInfoDto
                              {
                                  GroupId = id,
                                  QuestionId = qs.Id,
                                  GroupName = gr.GroupName,
                                  GroupDescription = gr.GroupDescription,
                                  CanCreateSurvey = gr.CanCreateSurvey,
                                  Private = gr.Private,
                                  SurveyQuestion = qs.SurveyQuestion,
                                  SurveyDescription = sr.SurveyDescription,
                                  StartDate = sr.StartDate,
                                  SurveyTittle = sr.SurveyTittle,
                                  EndDate = sr.EndDate,
                              }).ToListAsync();
            data.ForEach(x => x.SurveyOptions = _optionRepository.GetAll(x => x.QuestionId == x.QuestionId));
            var groupOwner = await _groupUserRepository.FirstOrDefaultAsync(x => x.GroupId == id && x.UserId == userId);
            ViewBag.AllGroupUsers = await GetAllGroupUsers(id);
            ViewBag.Role = groupOwner != null ? await _roleRepository.FirstOrDefaultAsync(x => x.Id == groupOwner.RoleId) : new Role();
            ViewBag.GroupInfo = data;
            ViewBag.User = userId;
            ViewBag.Group = await _groupRepository.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.IsMember = await _groupUserRepository.FirstOrDefaultAsync(x => x.GroupId == id && x.UserId == userId);
            ViewBag.UserRequest = await UserRequest(id);

            return View();
        }


        #region 
        public async Task<List<User>> UserRequest(Guid id)
        {
            var datxa = await _groupUserRepository.GetAll(x => x.GroupId == id && x.IsMember == false && x.IsActive == true)
                                      .Select(x => x.UserId) // İlgili UserId'leri seç
                                      .ToListAsync();
            var users = new List<User>();
            foreach (var userId in datxa)
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    users.Add(new User
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                    });
                }
            }
            if (users.Count == 0)
            {
                Console.WriteLine("User list is empty."); // Eğer liste boşsa, uyarı mesajı
            }
            return users; // Listeyi döndür
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> GroupCreate(Group group)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            group.GroupOwnerId = userId;
            group.IsActive = true;
            group.ImageUrl = "asd";
            var groups = await _groupRepository.CreateAsync(group);
            var role = await _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Yönetici");
            await _groupUserRepository.CreateAsync(new GroupUser
            {
                GroupId = groups.Id,
                UserId = userId,
                RoleId = role.Id,
                IsMember = true,
                IsActive = true,
            });
            return Json(new Response<Group>
            {
                Success = true,
                Message = "Kayıt Başarılı",
                Result = groups
            });

        }
        [HttpGet]
        public IActionResult SearchGroups(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Query cannot be empty");
            }

            var results = _groupRepository
    .GetAll(g => g.GroupName.ToLower().Contains(q.ToLower())) // SQL'e çevrilebilir
    .Select(g => new { g.Id, g.GroupName })
    .ToList();

            return Json(results);
        }
        [HttpPost]
        public async Task<IActionResult> MemberRequest(Guid id)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var role = await _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye");
            var group=await _groupRepository.FirstOrDefaultAsync(x=>x.Id==id);

            if (role != null)
            {
                if(group.Private==false)
                {
					var data = await _groupUserRepository.CreateAsync(new GroupUser
					{
						UserId = userId,
						GroupId = id,
						RoleId = role.Id,
						IsActive = true,
						IsMember = false
					});
					return Json(new Response<GroupUser>
					{
						Success = true,
						Message = "İstek Gönderildi",
						Result = data
					});
				}
                else
                {
					var data = await _groupUserRepository.CreateAsync(new GroupUser
					{
						UserId = userId,
						GroupId = id,
						RoleId = role.Id,
						IsActive = true,
						IsMember = true
					});
					return Json(new Response<GroupUser>
					{
						Success = true,
						Message = "Üye olundu.",
						Result = data
					});
				}
              
            }
            return Json(new Response<GroupUser>
            {
                Success = false,
                Message = "Kayıt Başarısız",
            });
        }
        public async Task<IActionResult> CheckMembershipStatus(Guid groupId)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var data = await _groupUserRepository.FirstOrDefaultAsync(x => x.GroupId == groupId && x.UserId == userId);
            if (data == null || (data.IsMember == false && data.IsActive == false))
            {
                // Kullanıcı grup üyesi değil ve istek de göndermemiş
                return Json(new
                {
                    membershipStatus = "not_member"
                });
            }
            else
            {
                if (data.IsMember == true)
                {
                    // Kullanıcı grup üyesi
                    return Json(new
                    {
                        membershipStatus = "approved"
                    });
                }
                else
                {
                    // Kullanıcı gruba üyelik isteği göndermiş ama onaylanmamış
                    return Json(new
                    {
                        membershipStatus = "pending"
                    });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(Guid groupId)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var data = await _groupUserRepository.FirstOrDefaultAsync(x => x.GroupId == groupId && x.UserId == userId);
            if (data != null) // Bu kontrol, data null olmamasını sağlamak için eklendi
            {
                await _groupUserRepository.DeleteAsync(data);

                return Json(new Response<GroupUser>
                {
                    Success = true,
                    Message = "Kayıt Silindi",
                });
            }
            return Json(new Response<GroupUser>
            {
                Success = false,
                Message = "Kayıt Başarısız",
            });
        }
        public async Task<IActionResult> UserRequestApproval(Guid userId, Guid groupId)
        {
            var data = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            if (data != null)
            {
                {
                    data.IsActive = true;
                    data.IsMember = true;
                    await _groupUserRepository.UpdateAsync(data);

                }
                return Json(new Response<GroupUser>
                {
                    Success = true,
                    Message = "Kayıt Başarılı",
                    Result = data
                });
            }
            return Json(new Response<GroupUser>
            {
                Success = false,
                Message = "Kayıt Başarısız",
            });

        }
        public async Task<IActionResult> UserRequestRejection(Guid userId, Guid groupId)
        {
            var data = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            if (data != null)
            {
                await _groupUserRepository.DeleteAsync(data);
                return Json(new Response<GroupUser>
                {
                    Success = true,
                    Message = "Başarılı",
                    Result = data
                });
            }
            return Json(new Response<GroupUser>
            {
                Success = false,
                Message = "Başarısız",
            });

        }
        public async Task<List<UserRoleDto>> GetAllGroupUsers(Guid id)
        {
            var userAndRole = await (from gu in _groupUserRepository.GetAll(x => x.GroupId == id && x.IsMember == true && x.IsActive == true)
                                     join u in _userRepository.GetAll(x => true) on gu.UserId equals u.Id
                                     join ro in _roleRepository.GetAll(x => true) on gu.RoleId equals ro.Id
                                     select new UserRoleDto
                                     {
                                         Name = u.Name,
                                         Surname = u.Surname,
                                         RoleName = ro.RoleName,
                                         UserId = u.Id,
                                     }).ToListAsync();
            return userAndRole;
        }
        public async Task<IActionResult> MakeGroupAdmin(Guid userId, Guid groupId)
        {
            var role = _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Yönetici").Result;
            var groupUser = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);

            if (role != null)
            {
                if (groupUser != null)
                {
                    groupUser.RoleId = role.Id;
                    await _groupUserRepository.UpdateAsync(groupUser);
                    return Json(new Response<GroupUser>
                    {
                        Success = true,
                        Message = "Başarılı",
                        Result = groupUser
                    });
                }
            }
            return Json(new Response<GroupUser>
            {
                Success = false,
                Message = "Başarısız",
            });
        }
        public async Task<IActionResult> TakeGroupAdmin(Guid userId, Guid groupId)
        {
            var role = _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye").Result;
            var groupUser = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);

            if (role != null)
            {
                if (groupUser != null)
                {
                    groupUser.RoleId = role.Id;
                    await _groupUserRepository.UpdateAsync(groupUser);
                    return Json(new Response<GroupUser>
                    {
                        Success = true,
                        Message = "Başarılı",
                        Result = groupUser
                    });
                }
            }
            return Json(new Response<GroupUser>
            {
                Success = false,
                Message = "Başarısız",
            });
        }


    }
}
