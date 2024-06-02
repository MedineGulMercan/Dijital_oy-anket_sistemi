using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitirmeProje.Controllers
{
    [Authorize]
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GroupController(IGroupRepository groupRepository, ILoginUserHelper loginUserHelper, IGroupUserRepository groupUserRepository, IRoleRepository roleRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IUserRepository userRepository, IOptionRepository optionRepository, IWebHostEnvironment webHostEnvironment)
        {
            _groupRepository = groupRepository;
            _loginUserHelper = loginUserHelper;
            _groupUserRepository = groupUserRepository;
            _roleRepository = roleRepository;
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _optionRepository = optionRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var groupOwner = await _groupUserRepository.FirstOrDefaultAsync(x => x.GroupId == id && x.UserId == userId);
            ViewBag.AllGroupUsers = await GetAllGroupUsers(id);
            ViewBag.Role = groupOwner != null ? await _roleRepository.FirstOrDefaultAsync(x => x.Id == groupOwner.RoleId) : new Role();
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
                        ImageUrl = user.ImageUrl,
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
        public async Task<IActionResult> GroupCreate(Group group, IFormFile img)
        {
            var userId = _loginUserHelper.GetLoginUserId();// Giriş yapan kullanıcı ID'sini alır
            group.GroupOwnerId = userId;
            group.IsActive = true; // Grubu aktif olarak işaretler
            group.ImageUrl = "";
            if (img != null)
            {
                // wwwroot yolunu alır
                var rootPath = _webHostEnvironment.WebRootPath;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(img.FileName); // Uzantı olmadan dosya adı
                var fileExtension = Path.GetExtension(img.FileName); // Dosya uzantısı
                // Dosyanın kaydedileceği yolu oluşturur                                            
                var filePath = Path.Combine(rootPath, "groupImage", fileNameWithoutExtension + "-" + userId.ToString() + fileExtension);
                // Dosya dizininin var olduğundan emin olur
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                // Dosya akışı oluşturur ve dosyayı kaydeder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);// Dosyayı asenkron olarak kopyalar
                }
                // Grup resmi URL'sini ayarlar
                group.ImageUrl = "/groupImage/" + fileNameWithoutExtension + "-" + userId.ToString() + fileExtension;
            }
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
      //grup isimlerinin küçük harfe çevrilip, arama sorgusunu içerip içermediği kontrol edilir.
            var results = _groupRepository
    .GetAll(g => g.GroupName.ToLower().Contains(q.ToLower())) // SQL'e çevrilebilir
    .Select(g => new { g.Id, g.GroupName,g.ImageUrl })
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
            //İstek atan kişi ve grup bilgileri çekilir
            var data = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            if (data != null)
            {
                {
                    // Kişinin üye mi bilgisi true olarak güncellenir
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
            //Kişi ve üye olduğu grubun bilgileri çekilir 
            var data = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            if (data != null)
            {
                await _groupUserRepository.DeleteAsync(data);// Kişi grup üyeliğinden çıkarılır/silinir
                return Json(new Response<GroupUser>
                {
                    Success = true,
                    Message = "Kişinin üyeliği iptal edildi",
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
                                         ImageUrl = u.ImageUrl
                                     }).ToListAsync();
            return userAndRole;
        }
        public async Task<IActionResult> MakeGroupAdmin(Guid userId, Guid groupId)
        {
            //Role tablosundan yönetici rolünün id'si çekilir
            var role = _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Yönetici").Result;
            //Yönetici yapılmak istenen kişi ve grubun bilgileri çekilir
            var groupUser = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);

            if (role != null)
            {
                if (groupUser != null)
                {
                    //Kişinin rolü Üye -> Yönetici olarak güncellenir
                    groupUser.RoleId = role.Id;
                    await _groupUserRepository.UpdateAsync(groupUser);
                    return Json(new Response<GroupUser>
                    {
                        Success = true,
                        Message = "Kişi grup yönetici olarak ayarlandı.",
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
            //Role tablosundan yönetici rolünün id'si çekilir
            var role = _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye").Result;
            //Yönetici yapılmak istenen kişi ve grubun bilgileri çekilir
            var groupUser = await _groupUserRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);

            if (role != null)
            {
                if (groupUser != null)
                {
                    //Kişinin rolü Yönetici -> Üye olarak güncellenir

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
        [HttpPost]
        public async Task<IActionResult> GroupUpdate(IFormFile img, Group group)
        {
            var data = await _groupRepository.FirstOrDefaultAsync(x => x.Id == group.Id);
            if (data != null)
            {
                var path = "";
                if (img != null)
                {
                    // wwwroot path
                    var rootPath = _webHostEnvironment.WebRootPath;
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(img.FileName); // Uzantı olmadan dosya adı
                    var fileExtension = Path.GetExtension(img.FileName); // Dosya uzantısı
                                                                         // Path to save the uploaded file
                    var filePath = Path.Combine(rootPath, "groupImage", fileNameWithoutExtension + "-" + group.Id.ToString() + fileExtension);
                    // Ensure the uploads directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    path = "/groupImage/" + fileNameWithoutExtension + "-" + group.Id.ToString() + fileExtension;
                }

                if (data != null)
                {
                    data.GroupName = group.GroupName;
                    data.GroupDescription = group.GroupDescription;
                    data.ImageUrl = img != null ? path : group.ImageUrl;
                    data.Private = group.Private;
                    data.CanCreateSurvey = group.CanCreateSurvey;
                    await _groupRepository.UpdateAsync(data);
                }
                return Json(new Response<Group>
                {
                    Success = true,
                    Message = "Kayıt Başarılı",
                    Result = data
                });
            }
            return Json(new Response<Group>
            {
                Success = false,
                Message = "Kayıt Başarısız",
            });
        }

    }
}
