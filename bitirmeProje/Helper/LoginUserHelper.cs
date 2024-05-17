using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Helper.Interface;
using System.Security.Claims;

namespace bitirmeProje.Helper
{
    public class LoginUserHelper : ILoginUserHelper
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;


        public LoginUserHelper(IUserRepository userRepository, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _httpContext = httpContext;
        }

        public Guid GetLoginUserId()
        {
            #region owner Id
            var loginUserId = Guid.Empty;
            var userId = _httpContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is not null)
            {
                var owner = _userRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId)).Result;
                if (owner is not null)
                    loginUserId = owner.Id;
            }
            #endregion
            return loginUserId;
        }
    }
}
