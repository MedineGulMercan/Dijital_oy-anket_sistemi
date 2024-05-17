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
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IRoleRepository _roleRepository;

        public SignUpController(IGenderRepository genderRepository, ICityRepository cityRepository, ICountryRepository countryRepository, IDistrictRepository districtRepository, IUserRepository userRepository, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, IRoleRepository roleRepository)
        {
            _genderRepository = genderRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _districtRepository = districtRepository;
            _userRepository = userRepository;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _roleRepository = roleRepository;
        }

        public IActionResult Index()
        {
            var genders = _genderRepository.GetAll(x=>true);
            var countries= _countryRepository.GetAll(x=>true);
            ViewBag.Genders = genders.ToList();
            ViewBag.Countries = countries.ToList();


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserSignUp(User user)
        {
            user.IsActive = true;
            user.IsAdmin = false;
            var data = await _userRepository.CreateAsync(user);
            var roleId = await _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye");
            var publicGroup = await _groupRepository.FirstOrDefaultAsync(x => x.GroupName == "Herkese Açık");
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
        public async Task<IActionResult> GetCities(Guid countryId)
        {
            var cities= _cityRepository.GetAll(x=>x.CountryId==countryId);
            return Json(cities);

        }
        public async Task<IActionResult> GetDistricts(Guid cityId)
        {
            var districts= _districtRepository.GetAll(x=>x.CityId== cityId);
            return Json(districts);

        }
    }
}
