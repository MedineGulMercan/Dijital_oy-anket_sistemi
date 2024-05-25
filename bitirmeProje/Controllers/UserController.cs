using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitirmeProje.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginUserHelper _loginUserHelper;
        private readonly IRoleRepository _roleRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public UserController(IUserRepository userRepository, ILoginUserHelper loginUserHelper, IRoleRepository roleRepository, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, IDistrictRepository districtRepository, IGenderRepository genderRepository, ICityRepository cityRepository, ICountryRepository countryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _loginUserHelper = loginUserHelper;
            _roleRepository = roleRepository;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _districtRepository = districtRepository;
            _genderRepository = genderRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task <IActionResult> Index()
        {
            var userId = _loginUserHelper.GetLoginUserId();
            ViewBag.UserInfo=await _userRepository.FirstOrDefaultAsync(x=>x.Id==userId);
            ViewBag.Gender = _genderRepository.GetAll(x => true);
            ViewBag.City = _cityRepository.GetAll(x=>true);
            ViewBag.Country = _countryRepository.GetAll(x=>true);
            ViewBag.District = _districtRepository.GetAll(x=>true);
            ViewBag.GroupsManagedBy =await GroupsManagedBy(userId);
            ViewBag.GroupMemberInfo = await GroupMember(userId);
            return View();

        }

        public async Task<List<Group>> GroupsManagedBy(Guid userId)
        {
            var adminRoleId=await _roleRepository.FirstOrDefaultAsync(x=>x.RoleName=="Yönetici");

            var groupsId = await _groupUserRepository.GetAll(x => x.UserId == userId && x.RoleId == adminRoleId.Id)
                .Select(x => x.GroupId)
                .ToListAsync();
           var groupInfo= new List<Group>();
            foreach (var groupId in groupsId)
            {
              var group= await  _groupRepository.FirstOrDefaultAsync(x => x.Id == groupId);
                if(group != null)
                {
                    groupInfo.Add(new Group
                    {
                        Id = groupId,
                        GroupName= group.GroupName,
                        ImageUrl= group.ImageUrl,
                        GroupDescription=group.GroupDescription,
                    });
                }
            }
            return groupInfo;
        }

        public async Task<List<Group>> GroupMember(Guid userId)
        {
            var adminRoleId = await _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye");
            var groupsId = await _groupUserRepository.GetAll(x => x.UserId == userId && x.RoleId == adminRoleId.Id)
               .Select(x => x.GroupId)
               .ToListAsync();
            var groupMemberInfo = new List<Group>();
            foreach (var groupId in groupsId)
            {
                var group = await _groupRepository.FirstOrDefaultAsync(x => x.Id == groupId);
                if (group != null)
                {
                    groupMemberInfo.Add(new Group
                    {
                        Id = groupId,
                        GroupName = group.GroupName,
                        ImageUrl = group.ImageUrl,
                        GroupDescription = group.GroupDescription,
                    });
                }
            }
            return groupMemberInfo;
        }
        [HttpPost]
        public async Task<IActionResult> UserUpdate(IFormFile img, User user)
        {
            var userId = _loginUserHelper.GetLoginUserId();

            var path = "";
            if (img != null)
            {
                // wwwroot path
                var rootPath = _webHostEnvironment.WebRootPath;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(img.FileName); // Uzantı olmadan dosya adı
                var fileExtension = Path.GetExtension(img.FileName); // Dosya uzantısı
                                                                     // Path to save the uploaded file
                var filePath = Path.Combine(rootPath, "userImage", fileNameWithoutExtension + "-" + userId.ToString() + fileExtension);
                // Ensure the uploads directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }
                path = "/userImage/" + fileNameWithoutExtension + "-" + userId.ToString() + fileExtension;
            }
            
            var data =await _userRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (data != null)
            {
                data.Name= user.Name;
                data.Surname = user.Surname;
                data.PhoneNumber = user.PhoneNumber;
                data.Birthday = user.Birthday;
                data.Mail = user.Mail;
                data.ImageUrl = img != null ? path : null;
               await _userRepository.UpdateAsync(data);
            }
            return Json(new Response<User>
            {
                Success = true,
                Message = "Kayıt Başarılı",
                Result = data
            });
        }
    }
}
