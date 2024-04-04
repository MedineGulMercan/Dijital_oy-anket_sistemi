using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Domain.Repositories;
using bitirmeProje.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bitirmeProje.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IGenderRepository _genderRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IUserRepository _userRepository;

        public SignUpController(IGenderRepository genderRepository, ICityRepository cityRepository, ICountryRepository countryRepository, IDistrictRepository districtRepository, IUserRepository userRepository)
        {
            _genderRepository = genderRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _districtRepository = districtRepository;
            _userRepository = userRepository;
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
