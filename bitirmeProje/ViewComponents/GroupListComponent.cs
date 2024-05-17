using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace bitirmeProje.ViewComponents
{
    public class GroupListComponent : ViewComponent
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILoginUserHelper _loginUserHelper;

        public GroupListComponent(IGroupUserRepository groupUserRepository, ILoginUserHelper loginUserHelper, IGroupRepository groupRepository)
        {

            _groupUserRepository = groupUserRepository;
            _loginUserHelper = loginUserHelper;
            _groupRepository = groupRepository;
        }

        public IViewComponentResult Invoke()
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var groupUser = _groupUserRepository.GetAll(x => x.UserId == userId).Select(x => x.GroupId).Distinct().ToListAsync().Result;
            var group = new List<Group>();


            var userGroupIds =  _groupUserRepository
            .GetAll(x => x.UserId == userId) // Kullanıcının grupları
            .Select(x => x.GroupId) // Grup ID'lerini alın
            .ToListAsync().Result;

            // Tüm grupları alın
            var allGroups =  _groupRepository
                .GetAll(x=>true) // Tüm grupları alın
                .ToListAsync().Result;

            // Üye olunmayan grupları filtreleyin
            var nonMemberGroups = allGroups
                .Where(g => !userGroupIds.Contains(g.Id)) // Kullanıcının üye olmadığı gruplar
                .ToList();
            ViewBag.Groups = nonMemberGroups;
            return View();
        }
    }
}

