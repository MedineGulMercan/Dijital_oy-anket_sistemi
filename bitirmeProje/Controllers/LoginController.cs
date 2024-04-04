using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bitirmeProje.Controllers
{
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
            try
            {
                var medinemDg = DateTime.UtcNow;
                var data = await _userRepository.FirstOrDefaultAsync(x => x.Username == loginDto.Username && x.Password == loginDto.Password);
                if (data == null)
                {
                    return Json(new Response<object>
                    {
                        Success = false,
                        Message = "Böyle bir kullanıcı bulunmamaktadır.",
                    });
                }
                return Json(new Response<object>
                {
                    Success = true,
                    Message = "Giriş Yapıldı",
                    Result = data
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
