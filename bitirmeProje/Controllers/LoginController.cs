using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bitirmeProje.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserLogin(LoginDto loginDto)
        {

            var loginUserData = await _userRepository.FirstOrDefaultAsync(x => x.Username == loginDto.Username && x.Password == loginDto.Password);

            if (loginUserData == null)
            {
                return Json(new Response<object>
                {
                    Success = false,
                    Message = "Kullanıcı adı veya şifre hatalı!",
                });
            }

            var claims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, loginUserData.Username),
                    new Claim(ClaimTypes.Role, loginUserData.IsAdmin ?"admin" : "user"),
                    new Claim(ClaimTypes.NameIdentifier,loginUserData.Id.ToString()),//claimin içine kullanıcının id sini kaydettik, artık istediğimiz her yerden erişebiliriz.
                };
            var userIdentity = new ClaimsIdentity(claims, "Login");
            var principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal); //burada tokenı oluşturuyor.

            return Json(new Response<object>
            {
                Success = true,
                Message = "Giriş Yapıldı",
                Result = loginUserData
            });
        }
        [HttpPost]
        public async Task<IActionResult> UserLogout()
        {
            await HttpContext.SignOutAsync(); // Kullanıcının oturumunu sonlandırır.

            return Json(new Response<object>
            {
                Success = true,
                Message = "Çıkış Yapıldı"
            });
        }
    }
}
