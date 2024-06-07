using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Domain.Repositories;
using bitirmeProje.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace bitirmeProje.Controllers
{
    [AllowAnonymous]
    public class SignUpController : Controller
    {
        private readonly IGenderRepository _genderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IRoleRepository _roleRepository;

        public SignUpController(IGenderRepository genderRepository, IUserRepository userRepository, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, IRoleRepository roleRepository)
        {
            _genderRepository = genderRepository;
            _userRepository = userRepository;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _roleRepository = roleRepository;
        }

        public IActionResult Index()
        {
            var genders = _genderRepository.GetAll(x=>true);
            ViewBag.Genders = genders.ToList();


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserSignUp(User user)
        {
            user.IsActive = true;
            user.IsAdmin = false;
            var data = await _userRepository.CreateAsync(user);
            var roleId = await _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye");
            var publicGroup = await _groupRepository.FirstOrDefaultAsync(x => x.GroupName == "Public");
            if(roleId != null && publicGroup!= null)
            {
                await _groupUserRepository.CreateAsync(new GroupUser
                {
                    GroupId = publicGroup.Id,
                    UserId = data.Id,
                    RoleId = roleId.Id,
                    IsMember = true,
                    IsActive = true,
                });
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
